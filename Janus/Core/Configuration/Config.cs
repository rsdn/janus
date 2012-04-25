using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

namespace Rsdn.Janus
{
	/// <summary>
	/// Конфигурация
	/// </summary>
	public class Config
	{
		#region Менеджмент синглтона и событий, конструктор, инициализация
		private static readonly object _lockFlag = new object();
		private static Config _instance;
		private const string _configXmlFileName = @"config.xml";

		[XmlIgnore]
		public static Config Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (_lockFlag)
						if (_instance == null)
						{
							Config instance = null;
							try
							{
								var path = Path.Combine(LocalUser.DatabasePath, _configXmlFileName);

								if (File.Exists(path))
									using (var fs = File.OpenRead(path))
										instance = (Config)Serializer.Deserialize(fs);
							}
							catch (LocalUserNotFoundException)
							{
								// Ignore.
								//
							}
							catch (Exception e)
							{
								MessageBox.Show(e.Message, ApplicationInfo.ApplicationName,
												MessageBoxButtons.OK, MessageBoxIcon.Error);
							}

							if (instance == null)
							{
								instance = new Config();
								instance.InitEmptyConfig();
							}

							Thread.MemoryBarrier();
							_instance = instance;
						}
				}

				return _instance;
			}
		}

		private static XmlSerializer _serializer;

		private static XmlSerializer Serializer
		{
			get { return _serializer ?? (_serializer = new XmlSerializer(typeof (Config))); }
		}

		public static void Save()
		{
			var path = Path.Combine(LocalUser.DatabasePath, _configXmlFileName);

			// Make sure config directory exist.
			Directory.CreateDirectory(LocalUser.DatabasePath);

			//Create copy of old config
			var oldPath = path + ".old";
			if (File.Exists(oldPath))
				File.Delete(oldPath);
			if (File.Exists(path))
				File.Copy(path, oldPath);

			// Save new
			var tmp = path + ".tmp";
			try
			{
				using (var fs = File.Create(tmp))
					Serializer.Serialize(fs, _instance);
				if (File.Exists(path))
					File.Delete(path);
				File.Copy(tmp, path);
			}
			finally
			{
				if (File.Exists(tmp))
					File.Delete(tmp);
			}
		}

		public static void Reload()
		{
			_instance = null;
		}

		public static Config GetClone()
		{
			// глубокое клонирование экземпляра
			// метод медленный, но в данном случае это неважно
			using (var ms = new MemoryStream())
			{
				Serializer.Serialize(ms, _instance);

				ms.Position = 0;
				var newInstance = (Config)Serializer.Deserialize(ms);
				newInstance.ConfigChanged = _instance.ConfigChanged;

				return newInstance;
			}
		}

		/// <summary>
		/// Вызывается при обновлении конфига.
		/// </summary>
		public event EventHandler ConfigChanged;

		public static void NewConfig(Config config)
		{
			_instance = config;

			if (_instance.ConfigChanged != null)
				_instance.ConfigChanged(_instance, EventArgs.Empty);
		}

		private void InitEmptyConfig()
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(
				(int)UILanguage);

			_searchList = new[] {string.Empty};
		}
		#endregion

		#region 01 Общие настройки
		private const string _categoryNameCommon =
			SR.Config.CategoryName.CommonResourceName;

		private const UILanguage _defaultUILanguage = UILanguage.Russian;
		private UILanguage _uiLanguage = _defaultUILanguage;

		[JanusDisplayName(SR.Config.UILanguage.DisplayNameResourceName)]
		[JanusDescription(SR.Config.UILanguage.DescriptionResourceName)]
		[JanusCategory(_categoryNameCommon)]
		[DefaultValue(_defaultUILanguage)]
		[ChangeProperty(ChangeActionType.Restart)]
		[SortIndex(10)]
		public UILanguage UILanguage
		{
			get { return _uiLanguage; }
			set { _uiLanguage = value; }
		}

		private const bool _defaultShowSplash = true;
		private bool _showSplash = _defaultShowSplash;

		[JanusDisplayName(SR.Config.ShowSplash.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ShowSplash.DescriptionResourceName)]
		[JanusCategory(_categoryNameCommon)]
		[DefaultValue(_defaultShowSplash)]
		[SortIndex(20)]
		public bool ShowSplash
		{
			get { return _showSplash; }
			set { _showSplash = value; }
		}

		private const string _defaultSiteUrl = "http://rsdn.ru/";
		private const string _defaultWebServiceUrl = "http://rsdn.ru/ws/Janus.asmx";

		private string _siteUrl = _defaultSiteUrl;

		[JanusDisplayName(SR.Config.SiteUrl.DisplayNameResourceName)]
		[JanusDescription(SR.Config.SiteUrl.DescriptionResourceName)]
		[JanusCategory(_categoryNameCommon)]
		[DefaultValue(_defaultSiteUrl)]
		[SortIndex(40)]
		public string SiteUrl
		{
			get { return _siteUrl; }
			set
			{
				value = value.Trim();

				// если пустой, используем адрес веб-сервиса
				if (value.Length == 0)
				{
					var configUrl = ConfigurationManager
						.AppSettings["Janus.WebService.JanusSvc"] ?? _defaultWebServiceUrl;

					value = new Uri(configUrl)
						.GetLeftPart(UriPartial.Authority) + "/";
				}
				else
				{
					if (value.IndexOf("://") < 0)
						value = "http://" + value;

					if (value.LastIndexOf('/') != value.Length - 1)
						value += "/";

					// Check URL validity
					new Uri(value);
				}

				_siteUrl = value;
			}
		}

		private ToolbarImageSize _imgToolBar = ToolbarImageSize.Size24;

		[JanusDisplayName(SR.Config.IconSize.DisplayNameResourceName)]
		[JanusDescription(SR.Config.IconSize.DescriptionResourceName)]
		[JanusCategory(_categoryNameCommon)]
		[ChangeProperty(ChangeActionType.Restart)]
		[DefaultValue(ToolbarImageSize.Size24)]
		[SortIndex(50)]
		public ToolbarImageSize ToolbarImageSize
		{
			get { return _imgToolBar; }
			set { _imgToolBar = value; }
		}

		private const string _desktopIniFileName = "Desktop.ini";

		private const string _contentDirIni =
			"[.ShellClassInfo]\r\nConfirmFileOp=0\r\nIconFile={0}\r\nIconIndex=0\r\n"
				+ "InfoTip=Папка для базы данных и файлов настроек RSDN@Home";

		[JanusDisplayName(SR.Config.DBFolderSpecIcon.DisplayNameResourceName)]
		[JanusDescription(SR.Config.DBFolderSpecIcon.DescriptionResourceName)]
		[JanusCategory(_categoryNameCommon)]
		[DefaultValue(false)]
		[SortIndex(60)]
		public bool DBFolderSpecIcon
		{
			get { return File.Exists(Path.Combine(LocalUser.DatabasePath, _desktopIniFileName)); }
			set
			{
				var iniFile = Path.Combine(LocalUser.DatabasePath, _desktopIniFileName);

				if (value)
				{
					if (!File.Exists(iniFile))
					{
						File.AppendAllText(iniFile,
							string.Format(_contentDirIni,
								Assembly.GetExecutingAssembly().Location),
							Encoding.Default);
						File.SetAttributes(iniFile, FileAttributes.Hidden | FileAttributes.ReadOnly);

						// akasoft: По статье MSDN папке надо назначить атрибут "системная", 
						// чтобы explorer стал учитывать desktop.ini, однако ReadOnly тоже хватает
						File.SetAttributes(LocalUser.DatabasePath, FileAttributes.ReadOnly);
					}
				}
				else
				{
					if (File.Exists(iniFile))
					{
						//Не стал пробывать удалять скрытый файл... Sheridan.
						File.SetAttributes(iniFile, FileAttributes.Normal);
						File.Delete(iniFile);
						File.SetAttributes(LocalUser.DatabasePath, FileAttributes.Normal);
					}
				}
			}
		}

		private NavigationComboConfig _navigationComboConfig = new NavigationComboConfig();

		[JanusDisplayName(SR.Config.NavigationCombo.DisplayNameResourceName)]
		[JanusDescription(SR.Config.NavigationCombo.DescriptionResourceName)]
		[JanusCategory(_categoryNameCommon)]
		[SortIndex(70)]
		public NavigationComboConfig NavigationComboConfig
		{
			get { return _navigationComboConfig; }
			set { _navigationComboConfig = value; }
		}

//		private TreeLink[] _treeLinks;
//
//		[JanusDisplayName(SR.Config.TreeLinks.DisplayNameResourceName)]
//		[JanusDescription(SR.Config.TreeLinks.DescriptionResourceName)]
//		[JanusCategory(_categoryNameCommon)]
//		[ChangeProperty(ChangeAction.Refresh)]
//		[SortIndex(80)]
//		public TreeLink[] TreeLinks
//		{
//			get { return _treeLinks; }
//			set { _treeLinks = value; }
//		}

		private UrlBehavior _behavior = UrlBehavior.InternalBrowser;

		[JanusDisplayName(SR.Config.ExternalUrlBehavior.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ExternalUrlBehavior.DescriptionResourceName)]
		[JanusCategory(_categoryNameCommon)]
		[ChangeProperty(ChangeActionType.NoAction)]
		[DefaultValue(UrlBehavior.InternalBrowser)]
		[SortIndex(81)]
		public UrlBehavior Behavior
		{
			get { return _behavior; }
			set { _behavior = value; }
		}

		private const ToolStripsStyle _defaultToolStripsStyle = ToolStripsStyle.Professional;
		private ToolStripsStyle _toolStripsStyle = _defaultToolStripsStyle;

		[JanusDisplayName(SR.Config.ToolStripsStyle.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ToolStripsStyle.DescriptionResourceName)]
		[JanusCategory(_categoryNameCommon)]
		[DefaultValue(_defaultToolStripsStyle)]
		[SortIndex(82)]
		public ToolStripsStyle ToolStripsStyle
		{
			get { return _toolStripsStyle; }
			set
			{
				_toolStripsStyle = value;
				switch (_toolStripsStyle)
				{
					case ToolStripsStyle.System:
						ToolStripManager.RenderMode = ToolStripManagerRenderMode.System;
						break;
					case ToolStripsStyle.Professional:
						ToolStripManager.RenderMode = ToolStripManagerRenderMode.Professional;
						break;
					case ToolStripsStyle.TanColorTable:
						ToolStripManager.Renderer = new ToolStripProfessionalRenderer(new TanColorTable());
						break;
				}
			}
		}

		private const SmartJumpBehavior _defaultSmartJumpBehavior =
			SmartJumpBehavior.NextUnreadForum;

		private SmartJumpBehavior _smartJumpBehavior = _defaultSmartJumpBehavior;

		[JanusDisplayName(SR.Config.SmartJumpBehavior.DisplayNameResourceName)]
		[JanusDescription(SR.Config.SmartJumpBehavior.DescriptionResourceName)]
		[JanusCategory(_categoryNameCommon)]
		[ChangeProperty(ChangeActionType.NoAction)]
		[DefaultValue(_defaultSmartJumpBehavior)]
		[SortIndex(83)]
		public SmartJumpBehavior SmartJumpBehavior
		{
			get { return _smartJumpBehavior; }
			set { _smartJumpBehavior = value; }
		}

		private const int _defaultSmartJumpPageSize = 90;
		private int _smartJumpPageSize = _defaultSmartJumpPageSize;

		[JanusDisplayName(SR.Config.SmartJumpPageSize.DisplayNameResourceName)]
		[JanusDescription(SR.Config.SmartJumpPageSize.DescriptionResourceName)]
		[JanusCategory(_categoryNameCommon)]
		[ChangeProperty(ChangeActionType.NoAction)]
		[DefaultValue(_defaultSmartJumpPageSize)]
		[SortIndex(84)]
		public int SmartJumpPageSize
		{
			get { return _smartJumpPageSize; }
			set
			{
				if (value <= 0 || value > 100)
					throw new ArgumentOutOfRangeException(
						"value", value, SR.Config.SmartJumpPageSizeLegitimateValueResourceName);

				_smartJumpPageSize = value;
			}
		}

		private ConfirmationConfig _confirmationConfig = new ConfirmationConfig();

		[JanusDisplayName(SR.Config.Confirmation.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Confirmation.DescriptionResourceName)]
		[JanusCategory(_categoryNameCommon)]
		[SortIndex(90)]
		public ConfirmationConfig ConfirmationConfig
		{
			get { return _confirmationConfig; }
			set { _confirmationConfig = value; }
		}

		private SearchConfig _searchConfig = new SearchConfig();

		[JanusDisplayName(SR.Config.Search.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Search.DescriptionResourceName)]
		[JanusCategory(_categoryNameCommon)]
		[SortIndex(100)]
		public SearchConfig SearchConfig
		{
			get { return _searchConfig; }
			set { _searchConfig = value; }
		}

		[JanusDisplayName(SR.Config.MsgPosition.DisplayNameResourceName), JanusDescription(SR.Config.MsgPosition.DescriptionResourceName), JanusCategory(_categoryNameCommon), DefaultValue(false), ChangeProperty(ChangeActionType.Restart), SortIndex(110)]
		public bool MsgPosition { get; set; }
		#endregion

		#region 02 Установки форумов
		private const string _categoryNameForums =
			SR.Config.CategoryName.ForumsResourceName;

		private const int _defaultMarkMessageReadInterval = 2000;
		private int _markMessageReadInterval = _defaultMarkMessageReadInterval;

		[JanusDisplayName(SR.Config.MarkMessageReadInterval.DisplayNameResourceName)]
		[JanusDescription(SR.Config.MarkMessageReadInterval.DescriptionResourceName)]
		[JanusCategory(_categoryNameForums)]
		[DefaultValue(_defaultMarkMessageReadInterval)]
		[SortIndex(10)]
		public int MarkMessageReadInterval
		{
			get { return _markMessageReadInterval; }
			set
			{
				if (value <= 0 || value > 10000)
					throw new ArgumentOutOfRangeException("value",
						SR.Config.MarkMessageReadInterval.OutOfRangeException);

				_markMessageReadInterval = value;
			}
		}

		private const bool _defaultMarkNavigatedMessages = false;

		[JanusDisplayName(SR.Config.MarkNavigatedMessages.DisplayNameResourceName)]
		[JanusDescription(SR.Config.MarkNavigatedMessages.DescriptionResourceName)]
		[JanusCategory(_categoryNameForums)]
		[DefaultValue(_defaultMarkNavigatedMessages)]
		[SortIndex(11)]
		public bool MarkNavigatedMessages { get; set; }

		private const bool _defaultRestoreForumPosition = true;
		private bool _restoreForumPosition = _defaultRestoreForumPosition;

		[JanusDisplayName(SR.Config.RestoreForumPosition.DisplayNameResourceName)]
		[JanusDescription(SR.Config.RestoreForumPosition.DescriptionResourceName)]
		[JanusCategory(_categoryNameForums)]
		[DefaultValue(_defaultRestoreForumPosition)]
		[SortIndex(20)]
		public bool RestoreForumPosition
		{
			get { return _restoreForumPosition; }
			set { _restoreForumPosition = value; }
		}

		private const int _defaultHistoryViewSize = 20;
		private int _historyViewSize = _defaultHistoryViewSize;

		/*[JanusDisplayName("HistorySizeName")]
		[JanusDescription("HistorySizeDescription")]
		[JanusCategory(_categoryNameForums)]
		[DefaultValue(_defaultHistoryViewSize)]
		[SortIndex(3)]*/

		[Browsable(false)]
		[XmlIgnore]
		public int HistoryViewSize
		{
			get { return _historyViewSize; }
			set { _historyViewSize = value; }
		}

		private ForumDisplayConfig _forumDisplayConfig = new ForumDisplayConfig();

		[JanusDisplayName(SR.Config.ForumDisplay.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumDisplay.DescriptionResourceName)]
		[JanusCategory(_categoryNameForums)]
		[SortIndex(30)]
		public ForumDisplayConfig ForumDisplayConfig
		{
			get { return _forumDisplayConfig; }
			set { _forumDisplayConfig = value; }
		}

		private TagLineConfig _tagLine = new TagLineConfig();

		[JanusDisplayName(SR.Config.TagLine.DisplayNameResourceName)]
		[JanusDescription(SR.Config.TagLine.DescriptionResourceName)]
		[JanusCategory(_categoryNameForums)]
		[SortIndex(40)]
		public TagLineConfig TagLine
		{
			get { return _tagLine; }
			set { _tagLine = value; }
		}

		private ForumExportConfig _forumExportConfig = new ForumExportConfig();

		[JanusDisplayName(SR.Config.ForumExport.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ForumExport.DescriptionResourceName)]
		[JanusCategory(_categoryNameForums)]
		[SortIndex(50)]
		public ForumExportConfig ForumExportConfig
		{
			get { return _forumExportConfig; }
			set { _forumExportConfig = value; }
		}
		#endregion

		#region 03 Синхронизация
		private const string _categoryNameSync = SR.Config.CategoryName.SyncResourceName;

		private const int _defaultHttpTimeout = 100000;
		private int _httpTimeout = _defaultHttpTimeout;

		[JanusDisplayName(SR.Config.ConnectionTimeout.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ConnectionTimeout.DescriptionResourceName)]
		[JanusCategory(_categoryNameSync)]
		[DefaultValue(_defaultHttpTimeout)]
		[SortIndex(0)]
		public int HttpTimeout
		{
			get { return _httpTimeout; }
			set { _httpTimeout = value; }
		}


		private const int _defaultRetriesCount = 3;
		private int _retriesCount = _defaultRetriesCount;

		[JanusDisplayName(SR.Config.ConnectRetries.DisplayNameResourceName)]
		[JanusDescription(SR.Config.ConnectRetries.DescriptionResourceName)]
		[JanusCategory(_categoryNameSync)]
		[DefaultValue(_defaultRetriesCount)]
		[SortIndex(1)]
		public int RetriesCount
		{
			get { return _retriesCount; }
			set { _retriesCount = value; }
		}

		private const bool _defaultRepairTopic = true;
		private bool _repairTopic = _defaultRepairTopic;

		[JanusDisplayName(SR.Config.RepairTopic.DisplayNameResourceName)]
		[JanusDescription(SR.Config.RepairTopic.DescriptionResourceName)]
		[JanusCategory(_categoryNameSync)]
		[DefaultValue(_defaultRepairTopic)]
		[SortIndex(2)]
		public bool RepairTopic
		{
			get { return _repairTopic; }
			set { _repairTopic = value; }
		}

		private const TopicRepairMode _defaultTopicRepairMode = TopicRepairMode.WholeTopic;
		private TopicRepairMode _topicRepairMode = _defaultTopicRepairMode;

		[JanusDisplayName(SR.Config.TopicRepairMode.DisplayNameResourceName)]
		[JanusDescription(SR.Config.TopicRepairMode.DescriptionResourceName)]
		[JanusCategory(_categoryNameSync)]
		[DefaultValue(_defaultTopicRepairMode)]
		[SortIndex(3)]
		public TopicRepairMode TopicRepairMode
		{
			get { return _topicRepairMode; }
			set { _topicRepairMode = value; }
		}

		private const bool _defaultDownloadUsers = true;
		private bool _downloadUsers = _defaultDownloadUsers;

		[JanusDisplayName(SR.Config.DownloadUsers.DisplayNameResourceName)]
		[JanusDescription(SR.Config.DownloadUsers.DescriptionResourceName)]
		[JanusCategory(_categoryNameSync)]
		[DefaultValue(_defaultDownloadUsers)]
		[SortIndex(4)]
		public bool DownloadUsers
		{
			get { return _downloadUsers; }
			set { _downloadUsers = value; }
		}

		private const int _defaultMaxUsersPerSession = 1000;
		private int _maxUsersPerSession = _defaultMaxUsersPerSession;

		[JanusDisplayName(SR.Config.MaxUsersPerSession.DisplayNameResourceName)]
		[JanusDescription(SR.Config.MaxUsersPerSession.DescriptionResourceName)]
		[JanusCategory(_categoryNameSync)]
		[DefaultValue(_defaultMaxUsersPerSession)]
		[SortIndex(5)]
		public int MaxUsersPerSession
		{
			get { return _maxUsersPerSession; }
			set { _maxUsersPerSession = value; }
		}

		private const int _defaultMaxMessagesPerSession = -1;
		private int _maxMessagesPerSession = _defaultMaxMessagesPerSession;

		[JanusDisplayName(SR.Config.MaxMessagesPerSession.DisplayNameResourceName)]
		[JanusDescription(SR.Config.MaxMessagesPerSession.DescriptionResourceName)]
		[JanusCategory(_categoryNameSync)]
		[DefaultValue(_defaultMaxMessagesPerSession)]
		[SortIndex(6)]
		public int MaxMessagesPerSession
		{
			get { return _maxMessagesPerSession; }
			set { _maxMessagesPerSession = value; }
		}


		private const bool _defaultMarkOwn = true;
		private bool _markOwn = _defaultMarkOwn;

		[JanusDisplayName(SR.Config.MarkOwn.DisplayNameResourceName)]
		[JanusDescription(SR.Config.MarkOwn.DescriptionResourceName)]
		[JanusCategory(_categoryNameSync)]
		[DefaultValue(_defaultMarkOwn)]
		[SortIndex(7)]
		public bool MarkOwn
		{
			get { return _markOwn; }
			set { _markOwn = value; }
		}

		private const bool _defaultAutoSync = false;

		[JanusDisplayName(SR.Config.Autosync.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Autosync.DescriptionResourceName)]
		[JanusCategory(_categoryNameSync)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[DefaultValue(_defaultAutoSync)]
		[SortIndex(8)]
		public bool AutoSync { get; set; }

		private const int _defaultAutoSyncInterval = 300;
		private int _autoSyncInterval = _defaultAutoSyncInterval;

		[JanusDisplayName(SR.Config.AutosyncInterval.DisplayNameResourceName)]
		[JanusDescription(SR.Config.AutosyncInterval.DescriptionResourceName)]
		[JanusCategory(_categoryNameSync)]
		[ChangeProperty(ChangeActionType.Refresh)]
		[DefaultValue(_defaultAutoSyncInterval)]
		[SortIndex(9)]
		public int AutoSyncInterval
		{
			get { return _autoSyncInterval; }
			set { _autoSyncInterval = value; }
		}

		private const bool _defaultUseFileLog = false;

		[JanusDisplayName(SR.Config.UseLog.DisplayNameResourceName)]
		[JanusDescription(SR.Config.UseLog.DescriptionResourceName)]
		[JanusCategory(_categoryNameSync)]
		[DefaultValue(_defaultUseFileLog)]
		[SortIndex(10)]
#if ! DEBUG
		[Browsable(false)]
#endif
			public bool UseFileLog { get; set; }

		private const string _defaultLogFileName = "";
		private string _logFileName = _defaultLogFileName;

		[JanusDisplayName(SR.Config.LogFile.DisplayNameResourceName)]
		[JanusDescription(SR.Config.LogFile.DescriptionResourceName)]
		[Editor(typeof (LogFileEditor), typeof (UITypeEditor))]
		[JanusCategory(_categoryNameSync)]
		[DefaultValue(_defaultLogFileName)]
		[SortIndex(11)]
#if ! DEBUG
		[Browsable(false)]
#endif
			public string LogFileName
		{
			get { return _logFileName; }
			set { _logFileName = value; }
		}

		public class LogFileEditor : FileNameEditor
		{
			protected override void InitializeDialog(OpenFileDialog ofd)
			{
				ofd.CheckFileExists = false;
			}
		}

		private const bool _defaultUseCompression = true;
		private bool _useCompression = _defaultUseCompression;

		[JanusDisplayName(SR.Config.UseCompression.DisplayNameResourceName)]
		[JanusDescription(SR.Config.UseCompression.DescriptionResourceName)]
		[JanusCategory(_categoryNameSync)]
		[DefaultValue(_defaultUseCompression)]
		[SortIndex(13)]
		public bool UseCompression
		{
			get { return _useCompression; }
			set { _useCompression = value; }
		}

		private const SyncThreadPriority _defaultSyncThreadPriority = SyncThreadPriority.Low;
		private SyncThreadPriority _syncThreadPriority = _defaultSyncThreadPriority;

		[JanusDisplayName(SR.Config.SyncThreadPriority.DisplayNameResourceName)]
		[JanusDescription(SR.Config.SyncThreadPriority.DescriptionResourceName)]
		[JanusCategory(_categoryNameSync)]
		[DefaultValue(_defaultSyncThreadPriority)]
		[SortIndex(14)]
		public SyncThreadPriority SyncThreadPriority
		{
			get { return _syncThreadPriority; }
			set { _syncThreadPriority = value; }
		}

		private ProxyConfig _proxyConfig = new ProxyConfig();

		[JanusDisplayName(SR.Config.Proxy.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Proxy.DescriptionResourceName)]
		[JanusCategory(_categoryNameSync)]
		[SortIndex(15)]
		public ProxyConfig ProxyConfig
		{
			get { return _proxyConfig; }
			set { _proxyConfig = value; }
		}
		#endregion

		#region 04 Фоновый (Неактивный) режим
		private const string _categoryNameBackgroundMode =
			SR.Config.CategoryName.BackgroundModeResourceName;

		private const string _categoryNameDB = SR.Config.CategoryName.DBResourceName;

		private const bool _defaultMinimizeToTray = true;
		private bool _minimizeToTray = _defaultMinimizeToTray;

		[JanusDisplayName(SR.Config.MinimizeTray.DisplayNameResourceName)]
		[JanusDescription(SR.Config.MinimizeTray.DescriptionResourceName)]
		[JanusCategory(_categoryNameBackgroundMode)]
		[DefaultValue(_defaultMinimizeToTray)]
		[SortIndex(10)]
		public bool MinimizeToTray
		{
			get { return _minimizeToTray; }
			set { _minimizeToTray = value; }
		}

		private const bool _defaultShowSyncWindow = true;
		private bool _showSyncWindow = _defaultShowSyncWindow;

		[JanusDisplayName(SR.Config.SyncWindow.DisplayNameResourceName)]
		[JanusDescription(SR.Config.SyncWindow.DescriptionResourceName)]
		[JanusCategory(_categoryNameBackgroundMode)]
		[DefaultValue(_defaultShowSyncWindow)]
		[SortIndex(20)]
		public bool ShowSyncWindow
		{
			get { return _showSyncWindow; }
			set { _showSyncWindow = value; }
		}

		[JanusDisplayName(SR.Config.SyncWindowPinned.DisplayNameResourceName)]
		[JanusDescription(SR.Config.SyncWindowPinned.DescriptionResourceName)]
		[JanusCategory(_categoryNameBackgroundMode)]
		[DefaultValue(false)]
		[SortIndex(30)]
		public bool SyncWindowPinned { get; set; }

		private NotificationConfig _notificationConfig = new NotificationConfig();

		[JanusDisplayName(SR.Config.Notification.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Notification.DescriptionResourceName)]
		[JanusCategory(_categoryNameBackgroundMode)]
		[SortIndex(40)]
		public NotificationConfig NotificationConfig
		{
			get { return _notificationConfig; }
			set { _notificationConfig = value; }
		}

		private SoundConfig _soundConfig = new SoundConfig();

		[JanusDisplayName(SR.Config.Sound.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Sound.DescriptionResourceName)]
		[JanusCategory(_categoryNameBackgroundMode)]
		[SortIndex(50)]
		public SoundConfig SoundConfig
		{
			get { return _soundConfig; }
			set { _soundConfig = value; }
		}

		private TickerConfig _tickerConfig = new TickerConfig();

		[JanusDisplayName(SR.Config.Ticker.DisplayNameResourceName)]
		[JanusDescription(SR.Config.Ticker.DescriptionResourceName)]
		[JanusCategory(_categoryNameBackgroundMode)]
		[SortIndex(60)]
		public TickerConfig TickerConfig
		{
			get { return _tickerConfig; }
			set { _tickerConfig = value; }
		}
		#endregion

		#region 05 База данных
		[JanusDisplayName(SR.Config.DB.DbDriverResourceName)]
		[JanusDescription(SR.Config.DB.DbDriverDescriptionResourceName)]
		[JanusCategory(_categoryNameDB)]
		[SortIndex(20)]
		public string DbDriver { get; set; }

		private const int _defaultConnectTimeout = 3;
		private int _connectTimeout = _defaultConnectTimeout;

		[JanusDisplayName(SR.Config.DBConnectTimeout.DisplayNameResourceName)]
		[JanusDescription(SR.Config.DBConnectTimeout.DescriptionResourceName)]
		[JanusCategory(_categoryNameDB)]
		[SortIndex(30)]
		[DefaultValue(_defaultConnectTimeout)]
		public int ConnectTimeout
		{
			get { return _connectTimeout; }
			set { _connectTimeout = value; }
		}

		[JanusDisplayName(SR.Config.DBConnectionString.DisplayNameResourceName)]
		[JanusDescription(SR.Config.DBConnectionString.DescriptionResourceName)]
		[JanusCategory(_categoryNameDB)]
		[SortIndex(40)]
		public string ConnectionString { get; set; }
		#endregion База данных

		#region Горячие клавиши
		public XmlNode XmlShortcuts;
		#endregion

		#region Настройки стиля
		/// <summary>
		/// Текущий стиль. Просто ссылка на синглтон, для сериализации внутрь конфига.
		/// </summary>
		[Browsable(false)]
		public StyleConfig StyleConfig
		{
			get { return StyleConfig.Instance; }
			set { StyleConfig.NewStyleConfig(value); }
		}
		#endregion

		#region Служебные опции не вошедшие в другие разделы

		#region Последние прочитанные сообщения
		[XmlIgnore]
		public readonly IDictionary<int, int> LastReadMessage = new Dictionary<int, int>();

		//Для сериализации
		public struct PositionEntry
		{
			public int forumId;
			public int msgId;
		}

		[Browsable(false)]
		public PositionEntry[] serLastReadMessage
		{
			get
			{
				var pea = new PositionEntry[LastReadMessage.Count];
				var i = 0;
				foreach (var pair in LastReadMessage)
				{
					pea[i].forumId = pair.Key;
					pea[i].msgId = pair.Value;
					i++;
				}
				return pea;
			}
			set
			{
				LastReadMessage.Clear();
				foreach (var pe in value)
					LastReadMessage.Add(pe.forumId, pe.msgId);
			}
		}
		#endregion

		#region Внутренние настройки, не отображаемые в диалоге
		private int[] _navTreeColumnWidth = new[] {140, 120};

		/// <summary>
		/// Размеры колонок дерева навигации
		/// </summary>
		[Browsable(false)]
		public int[] NavTreeColumnWidth
		{
			get { return _navTreeColumnWidth; }
			set { _navTreeColumnWidth = value; }
		}

		/// <summary>
		/// Показывать панель тэгов
		/// </summary>
		[Browsable(false)]
		[DefaultValue(false)]
		public bool ShowMessageFormTagBar { get; set; }

		private const string _defaultValueLastLanguageTag = "c#|c#|1";
		private string _lastLanguageTag = _defaultValueLastLanguageTag;

		/// <summary>
		/// Последний использованный в редакторе таг разметки языка
		/// </summary>
		[DefaultValue(_defaultValueLastLanguageTag)]
		[Browsable(false)]
		public string LastLanguageTag
		{
			get { return _lastLanguageTag; }
			set { _lastLanguageTag = value; }
		}

		private const string _defaultValueLastJRevision = "init";
		private string _lastJRevision = _defaultValueLastJRevision;

		/// <summary>
		/// Последняя ревизия
		/// </summary>
		[DefaultValue(_defaultValueLastJRevision)]
		[Browsable(false)]
		public string LastJRevision
		{
			get { return _lastJRevision; }
			set { _lastJRevision = value; }
		}
		#endregion

		#region Параметры поиска
		[Browsable(false)]
		[DefaultValue(false)]
		public bool AdvancedSearch { get; set; }

		private bool _searchInText = true;

		[Browsable(false)]
		[DefaultValue(true)]
		public bool SearchInText
		{
			get { return _searchInText; }
			set { _searchInText = value; }
		}

		[Browsable(false)]
		[DefaultValue(false)]
		public bool SearchInSubject { get; set; }

		[Browsable(false)]
		[DefaultValue(false)]
		public bool SearchAuthor { get; set; }

		[Browsable(false)]
		[DefaultValue(false)]
		public bool SearchInMarked { get; set; }

		[Browsable(false)]
		[DefaultValue(false)]
		public bool SearchInMyMessages { get; set; }

		[Browsable(false)]
		[DefaultValue(false)]
		public bool SearchAnyWord { get; set; }

		[Browsable(false)]
		[DefaultValue(false)]
		public bool SearchInOverquoting { get; set; }

		private string _searchText = string.Empty;

		[Browsable(false)]
		public string SearchText
		{
			get { return _searchText; }
			set { _searchText = value; }
		}

		private string[] _searchList = new[] {string.Empty};

		[Browsable(false)]
		public string[] SearchList
		{
			get { return _searchList; }
			set { _searchList = value; }
		}

		private int _searchForumId = -1;

		[Browsable(false)]
		[DefaultValue(-1)]
		public int SearchForumId
		{
			get { return _searchForumId; }
			set { _searchForumId = value; }
		}

		[Browsable(false)]
		[DefaultValue(false)]
		public bool SearchInQuestions { get; set; }
		#endregion

		#region Координаты форм 
		public FormBounds MainFormBounds = new FormBounds(0, 0, 800, 600, false);

		public FormBounds MessageFormBounds = new FormBounds(20, 20, 640, 480, false);

		public FormBounds WebBrowserFormBounds = new FormBounds(20, 20, 480, 320, false);

		//[MeansImplicitUse(ImplicitUseFlags.AllMembersUsed)]
		public struct FormBounds
		{
			public int X;
			public int Y;
			public int Width;
			public int Height;

			public FormBounds(int x, int y, int width, int height, bool maximize)
			{
				if (x < 0 || x > short.MaxValue)
					throw new ArgumentOutOfRangeException("x");
				if (y < 0 || y > short.MaxValue)
					throw new ArgumentOutOfRangeException("y");
				if (width <= 0 || width > short.MaxValue)
					throw new ArgumentOutOfRangeException("width");
				if (height <= 0 || height > short.MaxValue)
					throw new ArgumentOutOfRangeException("height");

				X = x;
				Y = y;
				Width = width;
				Height = height;
				_maximized = maximize;
			}

			[XmlIgnore]
			public Rectangle Bounds
			{
				get { return new Rectangle(X, Y, Width, Height); }
				set
				{
					X = value.X;
					Y = value.Y;
					Width = value.Width;
					Height = value.Height;
				}
			}

			private bool _maximized;

			/// <summary>
			/// Обработка максимизации формы
			/// </summary>
			public bool Maximized
			{
				get { return _maximized; }
				set { _maximized = value; }
			}
		}
		#endregion

		#region Экспорт/импорт прочитанных сообщений, маркеров и избранного
		public class StateConfig
		{
			private string _lastFileName = string.Empty;

			private RestoreStateOptions _restoreOptions =
				RestoreStateOptions.ReadedMessages
					| RestoreStateOptions.Markers
						| RestoreStateOptions.Favorites;

			private SaveStateOptions _saveOptions =
				SaveStateOptions.ReadedMessages
					| SaveStateOptions.Markers
						| SaveStateOptions.Favorites;

			public string LastFileName
			{
				get { return _lastFileName; }
				set { _lastFileName = value ?? string.Empty; }
			}

			public SaveStateOptions SaveOptions
			{
				get { return _saveOptions; }
				set { _saveOptions = value; }
			}

			public RestoreStateOptions RestoreOptions
			{
				get { return _restoreOptions; }
				set { _restoreOptions = value; }
			}
		}

		[Browsable(false)]
		public StateConfig JanusStateConfig = new StateConfig();
		#endregion

		/// <summary>
		/// ИД используемый по умолчанию
		/// для различнй ситуаций обработки данных
		/// </summary>
		public int SelfId = -1;

		public string ActiveFeature = string.Empty;

		public ForumFormState ForumFormState = ForumFormState.Normal;

		public string Login = string.Empty;
		public string EncodedPassword = string.Empty;

		public int ForumSplitterPosition = 200;
		public int SearchSplitterPosition = 200;

		public int[] ForumColumnOrder = new[] {0, 1, 2, 3, 4};
		public int[] ForumColumnWidth = new[] {320, 100, 50, 35, 80};
		public int[] SearchColumnOrder = new[] {0, 1, 2, 3, 4};
		public int[] SearchColumnWidth = new[] {320, 100, 50, 35, 80};
		public int[] FavoritesColumnOrder = new[] {0, 1};
		public int[] FavoritesColumnWidth = new[] {400, 400};

		public SortType ForumSortCriteria = SortType.ByDefault;
		public SortType SearchSortCriteria = SortType.ByDefault;
		public SortType FavoritesMessagesSortCriteria = SortType.BySubjectAsc;
		public SortDirection FavoritesFoldersSortDirection = SortDirection.Asc;

		public bool BadRestruct;
		#endregion
	}
}
