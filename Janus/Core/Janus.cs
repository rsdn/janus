#pragma warning disable 1692
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

using BLToolkit.Data;

using Rsdn.Janus.Framework;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Стартовый класс.
	/// </summary>
	public class Janus
	{
		private static bool CheckEnvironment()
		{
			if (Environment.Version < new Version(2, 0, 50727, 1433))
			{
				using (var frm = new SP1RequiredForm())
					frm.ShowDialog();
				return false;
			}

			return true;
		}

		[STAThread]
		public static void Main()
		{
			TraceVerbose("Instance startup");
#if DEBUG
			DbManager.TraceSwitch = new TraceSwitch("DbManager", "DbManager trace switch", "Info");
#endif
			try
			{
				Console.WriteLine(
					@"Janus project. Copyright (C) 2002-2011 by Rsdn Team. " +
					@"See rsdn.ru for more information.");

				// Проверка на единственность экземпляра приложения.
				bool cn;
				using (var m = new Mutex(true, "JanusRunningInstanceDetectionMutex", out cn))
				{
					if (!m.WaitOne(0, false))
					{
						WindowActivator.ActivateWindow(MainForm.GetCaption());
						return;
					}

					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);

					// Создаем контрол, чтобы инициализировать винформсный констекст синхронизации,
					// если он еще не инициализирован
					using (new Control()) { }

					Thread.CurrentThread.CurrentUICulture = new CultureInfo((int)Config.Instance.UILanguage);

					if (!CheckEnvironment())
						return;

					TraceVerbose("ResMgr");

					var rootManager = new ServiceManager(true);

					rootManager.Publish<IUIShell>(
						new UIShell(
							() => ApplicationManager.Instance.MainForm,
							freeze => ApplicationManager.Instance.MainForm.Enabled = freeze));

					if (Config.Instance.ShowSplash)
					{
						EventHandler hider = null;
						IServiceRegistrationCookie informerCookie = null;
						IServiceRegistrationCookie progressCookie = null;

						hider =
							(sender, e) =>
							{
								// ReSharper disable AccessToModifiedClosure
								if (progressCookie != null)
									rootManager.Unpublish(progressCookie);

								rootManager.Publish<IProgressService>(
									new DefaultProgressService(rootManager));

								if (informerCookie != null)
									rootManager.Unpublish(informerCookie);
								SplashHelper.Hide();
								Application.Idle -= hider;
								// ReSharper restore AccessToModifiedClosure
							};

						Application.Idle += hider;

						informerCookie = rootManager.Publish(
							SplashHelper.Show(rootManager));

						progressCookie = rootManager.Publish(
							SplashHelper.GetProgressService());

						//Thread.Sleep(20000);
					}
					else
						rootManager.Publish<IProgressService>(
							new DefaultProgressService(rootManager));

					using (var host = new JanusHost(rootManager))
					{
						using (host.InitScope())
						{
							rootManager.SetSplashMessage(SR.Splash.ApplicationStart);

							TraceVerbose("JanusHost");

							// Подписка сервиса на извещения об изменении конфигурации.
							var configChangedNotifier = host.GetRequiredService<ConfigChangedNotifier>();
							Config.Instance.ConfigChanged +=
								(o, args) => configChangedNotifier.FireConfigChanged(Config.Instance);

							try
							{
								//Проверка наличия пользователя
								if (!LocalUser.UserExists())
									using (var ouf = new OptionsUserForm(host, true))
										ouf.ShowDialog();

								rootManager.SetSplashMessage(SR.Splash.CheckDatabase);

								if (!DBSchemaManager.CheckDB(host))
								{
									// User cancelled.
									//
									TraceVerbose("User cancelled");
								}

								AutoResetEvent signal = new AutoResetEvent(true);
								if (DBSchemaManager.IsNeedRestructuring(host))
								{
									signal.Reset();
									ProgressWorker.Run(rootManager, false,
										progressVisualizer =>
											{
												progressVisualizer.SetProgressText(SR.Splash.RestructureDatabase);
												try
												{
													DBSchemaManager.Restruct(host);
													//DatabaseManager.ClearTopicInfo(host);
													Config.Instance.BadRestruct = false;
												}
												catch
												{
													Config.Instance.BadRestruct = true;
													Config.Save();
													throw;
												}
												finally
												{
													// разрешаем выполнение главного потока
													signal.Set();
												}
											});
								}

								// блокируем поток, пока не завершится реструктуризация БД
								while (!signal.WaitOne(100))
									Application.DoEvents();

								// Проверяем наличие таблички topic_info
								DatabaseManager.CheckTopicInfoIntegrity(host);

								TraceVerbose("DB ready");

								rootManager.Publish<IMainWindowService>(
									new MainWindowService(() => ApplicationManager.Instance.MainForm));
								rootManager.Publish(
									new DockManager(() => ApplicationManager.Instance.MainForm.DockPanel));

								Application.ThreadException +=
									(sender, e) =>
										{
											using (var ef = new ExceptionForm(host, e.Exception, true))
												ef.ShowDialog(
													rootManager
														.GetRequiredService<IUIShell>()
														.GetMainWindowParent());
										};

								TraceVerbose("Ready to Run()");
							}
							catch (LocalUserNotFoundException)
							{
								MessageBox.Show(
									SR.JanusImpossibleWork,
									ApplicationInfo.ApplicationName,
									MessageBoxButtons.OK,
									MessageBoxIcon.Error);
								throw;
							}
						}
						ApplicationManager.Instance.Run(host);
					}
				}
			}
			catch (Exception ex)
			{
				using (var ef = new ExceptionForm(null, ex, false))
					ef.ShowDialog();
			}
		}

		#region Debug

		private static readonly Stopwatch _sw = Stopwatch.StartNew();

		[Conditional("DEBUG")]
		private static void TraceVerbose(string label)
		{
			Debug.WriteLineIf(TraceSwitch.TraceVerbose, _sw.Elapsed + ": " + label, TraceSwitch.DisplayName);
		}

		private static TraceSwitch _traceSwitch;

		private static TraceSwitch TraceSwitch
		{
			get { return _traceSwitch ?? (_traceSwitch = new TraceSwitch("EntryPoint", "Program entry point trace switch")); }
		}

		#endregion
	}
}