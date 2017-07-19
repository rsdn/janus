using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using CodeJam;
using CodeJam.Services;

using Rsdn.Janus.GoJanusNet;
using Rsdn.Janus.Log;
using Rsdn.Janus.Protocol;

namespace Rsdn.Janus
{
	// TODO: нужно уничтожать полностью.
	/// <summary>
	/// Контрольный центр приложения.
	/// </summary>
	internal sealed class ApplicationManager
	{
		#region Constructor & Instance support

		private ApplicationManager()
		{
		}

		[Obsolete("Use IServiceProvider instance, supplied by call argument instead")]
		public static ApplicationManager Instance { [DebuggerStepThrough] get; } = new ApplicationManager();

		private IServiceProvider _serviceProvider;

		/// <summary>
		/// Не использовать без крайней необходимости!!!.
		/// </summary>
		public IServiceProvider ServiceProvider
		{
			get { return _serviceProvider; }
			set
			{
				_serviceProvider = value;
				ForumNavigator = new ForumNavigator(value);
			}
		}

		#endregion

		#region Initialization & Run & Events support

		private void Init(IServiceProvider serviceProvider)
		{
			MainForm = new MainForm(_serviceProvider);

			Logger = serviceProvider.GetRequiredService<ILogger>();
			Navigator = new Navigator(serviceProvider);

			serviceProvider.GetRequiredService<DockManager>().Init();
			((OutboxManager) ServiceProvider.GetRequiredService<IOutboxManager>()).Init();
#if DEBUG
			// ReSharper disable once ObjectCreationAsStatement
			new FileLog(Logger);
#endif
			Navigator.Init();

			GoJanusListener.Start(_serviceProvider);

			Logger.LogInfo($"{Environment.OSVersion.GetOSNameWithVersion()} , .NET Runtime {Environment.Version}");
			Logger.LogInfo(ApplicationInfo.NameWithVersionAndCopyright);
		}

		private static bool TryInstallJanusProtocol()
		{
			try
			{
				JanusProtocol.SetDataSource(new JanusInternalResourceProvider());
				JanusProtocol.InstallProtocol();
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					string.Format(SR.Application.ProtocolInstallationError, ex.Message),
					ApplicationInfo.ApplicationName,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}

		private static void UninstallJanusProtocol()
		{
			try
			{
				JanusProtocol.UninstallProtocol();
			}
			catch (Exception ex)
			{
				Code.AssertState(false, "UninstallJanusProtocol: {0}", ex.Message);
			}
		}

		internal static void RegisterGoJanusNet()
		{
			ComHelper.RegisterAssembly(typeof(GoUrl));
		}

		private static void RegisterGoJanusNetAsAdmin()
		{
			var showErrorBox = new Action<string>(reason =>
			{
				var errorMessage = string.Format(SR.Application.GoJanusInstallationError, reason);
				MessageBox.Show(
					errorMessage,
					ApplicationInfo.ApplicationName,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			});

			int runAsAdminResult;
			try
			{
				runAsAdminResult = EnvironmentHelper.RunAsAdmin(CmdLineArg.RegisterGoJanusNet);
			}
			catch (Exception ex)
			{
				showErrorBox(ex.Message);
				return;
			}

			Code.AssertState(
				Enum.IsDefined(typeof(ExitCode), runAsAdminResult),
				"RunAsAdmin return undefuned ExitCode: Value: hex: 0x{0:X02} dec: {0}",
				runAsAdminResult);

			var exitCode = (ExitCode) runAsAdminResult;
			switch (exitCode)
			{
				case ExitCode.Ok:
					// зарегили
					break;

				case ExitCode.ErrorRegisterGoJanusNet:
					// ошибка во время регистрации в процессе стартовавшим от админа
					showErrorBox(SR.Application.GoJanusInstallationErrorReasonErrorCode);
					break;

				default:
					// процесс стартовавший от админа вернул что-то, что есть в энуме ExitCode, но не
					// может возвращаться из Juanus.RegisterGoJanusNet, то есть в том процессе
					// поток управления пошел куда-то не туда
					Code.AssertState(
						false,
						"Unexpected exit code. Name: {0}.{1}. Value: hex: 0x{2:X02} dec: {2}",
						typeof(ExitCode).Name, Enum.GetName(typeof(ExitCode), runAsAdminResult), runAsAdminResult);
					break;
			}
		}

		private static void CheckGoJanusNetInstallation()
		{
			if (ComHelper.IsTypeRegisteredInCom(typeof(GoUrl)))
				return;

			try
			{
				RegisterGoJanusNet();
			}
			catch (UnauthorizedAccessException)
			{
				RegisterGoJanusNetAsAdmin();
			}
		}

		internal void Run(IServiceProvider serviceProvider)
		{
			serviceProvider.SetSplashMessage(SR.Splash.InitApplication);

			CheckGoJanusNetInstallation();

			if (!TryInstallJanusProtocol())
				return;

			try
			{
				ProtocolDispatcher = new JanusProtocolDispatcher(serviceProvider);

				Forums.BeforeLoadData += ForumsBeforeLoadData;

				foreach (var forum in Forums.Instance.ForumList)
					forum.BeforeLoadData += ActiveForumBeforeLoadData;

				Init(serviceProvider);
				serviceProvider.SetSplashMessage(SR.Splash.RunApplication);

				Application.Run(MainForm);
			}
			finally
			{
				UninstallJanusProtocol();
			}
		}

		private void ForumsBeforeLoadData(object sender, EventArgs e)
		{
			ServiceProvider.SetSplashMessage(SR.Splash.LoadForums);
			Forums.BeforeLoadData -= ForumsBeforeLoadData;
		}

		private void ActiveForumBeforeLoadData(object sender, EventArgs e)
		{
			var forum = (Forum)sender;

			ServiceProvider.SetSplashMessage(
				string.Format(SR.Splash.LoadForum, forum.DisplayName));

			forum.BeforeLoadData -= ActiveForumBeforeLoadData;
		}

		#endregion

		#region Management services

		public static Forums Forums => Forums.Instance;

		/// <summary>
		/// Экземпляр формы с деревом навигации.
		/// </summary>
		public Navigator Navigator { get; private set; }

		/// <summary>
		/// Система ведения протокола.
		/// </summary>
		public ILogger Logger { get; private set; }

		/// <summary>
		/// Менеджер исходящих.
		/// </summary>
		[Obsolete("Use GetRequiredService<IOutboxManager>() instead")]
		public OutboxManager OutboxManager => (OutboxManager) ServiceProvider.GetRequiredService<IOutboxManager>();

		/// <summary>
		/// Экземпляр диспатчера протокола.
		/// </summary>
		public JanusProtocolDispatcher ProtocolDispatcher { get; private set; }

		public static string HomeDirectoryPath => AppDomain.CurrentDomain.BaseDirectory;

		/// <summary>
		/// Ссылка на экземпляр класса, обеспечивающего навигацию по форумам.
		/// </summary>
		public ForumNavigator ForumNavigator { get; private set; }

		#endregion

		#region Main form wrapper

		//private readonly object _mainFormLockFlag = new object();

		public MainForm MainForm { get; private set; }

		#endregion
	}
}