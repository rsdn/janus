using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using CodeJam.Extensibility;

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

		private static readonly ApplicationManager _instance =
			new ApplicationManager();

		[Obsolete("Use IServiceProvider instance, supplied by call argument instead")]
		public static ApplicationManager Instance
		{
			[DebuggerStepThrough]
			get
			{
				return _instance;
			}
		}

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
			((OutboxManager)ServiceProvider.GetRequiredService<IOutboxManager>()).Init();
#if DEBUG
			new FileLog(Logger);
#endif
			Navigator.Init();

			GoJanusListener.Start(_serviceProvider);

			Logger.LogInfo(
				string.Format("{0} , .NET Runtime {1}",
					Environment.OSVersion.GetOSNameWithVersion(),
					Environment.Version));
			Logger.LogInfo(ApplicationInfo.NameWithVersionAndCopyright);
		}

		private static void CheckJanusProtocolInstallation()
		{
			try
			{
				JanusProtocol.SetDataSource(new JanusInternalResourceProvider());

				var rs = new RegistrationServices();
				rs.RegisterAssembly(typeof(JanusProtocol).Assembly,
					AssemblyRegistrationFlags.SetCodeBase);
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					string.Format(SR.Application.ProtocolInstallationError, ex.Message),
					ApplicationInfo.ApplicationName,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private static void CheckGoJanusNetInstallation()
		{
			try
			{
				var rs = new RegistrationServices();
				rs.RegisterAssembly(typeof(GoJanusNet.GoUrl).Assembly,
					AssemblyRegistrationFlags.SetCodeBase);
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					string.Format(SR.Application.GoJanusInstallationError, ex.Message),
					ApplicationInfo.ApplicationName,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		internal void Run(IServiceProvider serviceProvider)
		{
			serviceProvider.SetSplashMessage(SR.Splash.InitApplication);

			CheckJanusProtocolInstallation();
			CheckGoJanusNetInstallation();

			ProtocolDispatcher = new JanusProtocolDispatcher(serviceProvider);

			Forums.BeforeLoadData += ForumsBeforeLoadData;

			foreach (var forum in Forums.Instance.ForumList)
				forum.BeforeLoadData += ActiveForumBeforeLoadData;

			Init(serviceProvider);
			serviceProvider.SetSplashMessage(SR.Splash.RunApplication);

			Application.Run(MainForm);
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

		public static Forums Forums
		{
			get { return Forums.Instance; }
		}

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
		public OutboxManager OutboxManager
		{
			get { return (OutboxManager)ServiceProvider.GetRequiredService<IOutboxManager>(); }
		}

		/// <summary>
		/// Экземпляр диспатчера протокола.
		/// </summary>
		public JanusProtocolDispatcher ProtocolDispatcher { get; private set; }

		public static string HomeDirectoryPath
		{
			get { return AppDomain.CurrentDomain.BaseDirectory; }
		}

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
