using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml.Serialization;

namespace Rsdn.Janus
{
	/// <summary>
	/// Разделяет свойства на основные и на более детализированные
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class DetailedAttribute : Attribute
	{
		private static readonly DetailedAttribute _detailed = new DetailedAttribute(true);

		public static DetailedAttribute DetailedInstance
		{
			get { return _detailed; }
		}

		public DetailedAttribute()
		{
		}

		private DetailedAttribute(bool detail)
		{
		}
	}

	/// <summary>
	/// Конфигурация стиля
	/// </summary>
	public class StyleConfig
	{
		#region Настройки по умолчанию

		private static readonly Color _defaultMessageTreeBack = Color.FromArgb(224, 240, 224);
		private static readonly Font  _defaultMessageTreeFont = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);

		private static readonly Color _defaultMessageColor              = Color.Black;
		private static readonly Color _defaultSelfMessageColor          = Color.SteelBlue;
		private static readonly Color _defaultRepliesToSelfMessageColor = Color.DarkRed;

		private static readonly Color _defaultFavoriteMessageColor = Color.Chocolate;

		private static readonly Color _defaultParentActiveMessageBackColor = Color.LightSteelBlue;
		private static readonly Color _defaultChildActiveMessageBackColor = Color.PowderBlue;

		private static readonly Color _defaultNavigationTreeForumColor = Color.Black;
		private static readonly Color _defaultNavigationTreeBack       = Color.FromArgb(244, 255, 244);
		private static readonly Font  _defaultNavigationTreeFont       = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);

		private static readonly Color _defaultOptionsGridBack     = Color.FromArgb(192, 192, 244);
		private static readonly Color _defaultOptionsGridViewBack = Color.FromArgb(244, 244, 255);
		private static readonly Color _defaultOptionsGridLine     = Color.FromArgb(224, 224, 244);
		private static readonly Color _defaultOptionsGridText     = SystemColors.WindowText;

		private static readonly Color _defaultQuotaPrefixColor = Color.FromArgb(0, 60, 180);

		private static readonly Color[] _defaultQuotaColors = new[]
			{
				Color.FromArgb(  0,  85,   0),
				Color.FromArgb( 60, 140,  60),
				Color.FromArgb(120, 200, 120)
			};

		private static readonly Color[] _defaultForumPriorityColor = new[]
			{
				Color.FromArgb(  0, 0,   0),
				Color.FromArgb(100, 0,   0),
				Color.FromArgb(130, 0,   0),
				Color.FromArgb(160, 0,   0),
				Color.FromArgb(190, 0,   0),
				Color.FromArgb(220, 0,   0),
				Color.FromArgb(255, 0,   0),
				Color.FromArgb(  0, 0, 150),
				Color.FromArgb(  0, 0, 200),
				Color.FromArgb(  0, 0, 255)
			};

		#endregion

		private static readonly ColorConverter _colorConverter = new ColorConverter();
		private static readonly FontConverter _fontConverter = new FontConverter();

		#region 1.Общие настройки

		private const string _themeDefault = "_default";
		private string _theme = _themeDefault;

		[TypeConverter(typeof(ButtonsPropertyConverter))]
		[JanusDisplayName("StyleConfig.ThemeName")]
		[JanusDescription("StyleConfig.ThemeDescription")]
		[JanusCategory("Config.CategoryName.Style.Common")]
		[SortIndex(1)]
		[DefaultValue(_themeDefault)]
		public string Theme
		{
			get { return _theme; }
			set
			{
				_theme = value;
				OnStyleChangeEvent(StyleChangeEventArgs.ImagesStyle);
			}
		}

		private string _externalThemeFile = string.Empty;

		[Editor(typeof(ExternalThemeFileEditor), typeof(UITypeEditor))]
		[JanusDisplayName("StyleConfig.ExternalThemeFileName")]
		[JanusDescription("StyleConfig.ExternalThemeFileDescription")]
		[JanusCategory("Config.CategoryName.Style.Common")]
		[SortIndex(2)]
		public string ExternalThemeFile
		{
			get { return _externalThemeFile; }
			set
			{
				_externalThemeFile = value;
				OnStyleChangeEvent(StyleChangeEventArgs.ImagesStyle);
			}
		}

		private Color _logBackColor = Color.FromArgb(255, 244, 244);

		[JanusDisplayName("StyleConfig.LogBackColorName")]
		[JanusDescription("StyleConfig.LogBackColorDescription")]
		[JanusCategory("Config.CategoryName.Style.Common")]
		[SortIndex(3)]
		public Color LogBackColor
		{
			get { return _logBackColor; }
			set
			{
				_logBackColor = value;
				OnStyleChangeEvent(StyleChangeEventArgs.ImagesStyle);
			}
		}

		private class ExternalThemeFileEditor : FileNameEditor
		{
			protected override void InitializeDialog(OpenFileDialog ofd)
			{
				ofd.Filter = "Theme resource file (*.resx; *.resources)|*.resx;*.resources";
			}
		}

		#endregion

		#region 2.Настройки дерева сообщений

		[JanusDisplayName("Config.StyleConfig.MessageTreeFont.DisplayName")]
		[JanusCategory("Config.CategoryName.Style.MessageTree")]
		[SortIndex(0)]
		[XmlIgnore]
		public Font MessageTreeFont
		{
			get { return _msgTreeFont; }
			set
			{
				_msgTreeFont = value ?? _defaultMessageTreeFont;
				OnStyleChangeEvent(StyleChangeEventArgs.FontColorStyle);
			}
		}

		private Font _msgTreeFont = _defaultMessageTreeFont;

		[Browsable(false)]
		public string MessageTreeFontStr
		{
			get { return _fontConverter.ConvertToInvariantString(_msgTreeFont); }
			set
			{
				_msgTreeFont = (Font) _fontConverter.ConvertFromInvariantString(value);
				OnStyleChangeEvent(StyleChangeEventArgs.FontColorStyle);
			}
		}

		[JanusDisplayName("Config.StyleConfig.TreeBackground.DisplayName")]
		[JanusCategory("Config.CategoryName.Style.MessageTree")]
		[SortIndex(1)]
		[XmlIgnore]
		[Detailed]
		public Color MessageTreeBack
		{
			get { return _msgTreeBack; }
			set { SetColorNonTransp(out _msgTreeBack, value, _defaultMessageTreeBack); }
		}

		[Browsable(false)]
		public string MessageTreeBackStr
		{
			get { return _colorConverter.ConvertToInvariantString(_msgTreeBack); }
			set { SetColorNonTranspStr(out _msgTreeBack, value, _defaultMessageTreeBack); }
		}

		private Color _msgTreeBack = _defaultMessageTreeBack;

		[JanusDisplayName("Config.StyleConfig.MessageColor.DisplayName")]
		[JanusCategory("Config.CategoryName.Style.MessageTree")]
		[SortIndex(2)]
		[XmlIgnore]
		public Color MessageColor
		{
			get { return _msgColor; }
			set { SetColorNonTransp(out _msgColor, value, _defaultMessageColor); }
		}

		[Browsable(false)]
		public string MessageColorStr
		{
			get { return _colorConverter.ConvertToInvariantString(_msgColor); }
			set { SetColorNonTranspStr(out _msgColor, value, _defaultMessageColor); }
		}

		private Color _msgColor = _defaultMessageColor;

		[JanusDisplayName("Config.StyleConfig.OwnMessagesColor.DisplayName")]
		[JanusCategory("Config.CategoryName.Style.MessageTree")]
		[SortIndex(3)]
		[XmlIgnore]
		public Color SelfMessageColor
		{
			get { return _selfMsgColor; }
			set { SetColorNonTransp(out _selfMsgColor, value, _defaultSelfMessageColor); }
		}

		[Browsable(false)]
		public string SelfMessageColorStr
		{
			get { return _colorConverter.ConvertToInvariantString(_selfMsgColor); }
			set { SetColorNonTranspStr(out _selfMsgColor, value, _defaultSelfMessageColor); }
		}

		private Color _selfMsgColor = _defaultSelfMessageColor;

		[JanusDisplayName("Config.StyleConfig.MessagesAnswerColor.DisplayName")]
		[JanusCategory("Config.CategoryName.Style.MessageTree")]
		[SortIndex(4)]
		[XmlIgnore]
		public Color RepliesToSelfMessageColor
		{
			get { return _repliesMsgColor; }
			set { SetColorNonTransp(out _repliesMsgColor, value, _defaultRepliesToSelfMessageColor); }
		}

		[Browsable(false)]
		public string RepliesToSelfMessageColorStr
		{
			get { return _colorConverter.ConvertToInvariantString(_repliesMsgColor); }
			set { SetColorNonTranspStr(out _repliesMsgColor, value, _defaultRepliesToSelfMessageColor); }
		}

		private Color _repliesMsgColor = _defaultRepliesToSelfMessageColor;

		[JanusDisplayName("Config.StyleConfig.BrokenColor.DisplayName")]
		[JanusCategory("Config.CategoryName.Style.MessageTree")]
		[SortIndex(5)]
		[XmlIgnore]
		public Color MissingTopicColor
		{
			get { return _missingTopicColor; }
			set { SetColorNonTransp(out _missingTopicColor, value, Color.FromKnownColor(KnownColor.Green)); }
		}

		private Color _missingTopicColor = Color.FromKnownColor(KnownColor.Green);

		[Browsable(false)]
		public string MissingTopicColorStr
		{
			get { return _colorConverter.ConvertToInvariantString(_missingTopicColor); }
			set { SetColorNonTranspStr(out _missingTopicColor, value, Color.FromKnownColor(KnownColor.Green)); }
		}

		private Color _favoriteMessageColor = _defaultFavoriteMessageColor;

		[JanusDisplayName("Config.StyleConfig.FavoriteMessageColor.DisplayName")]
		[JanusCategory("Config.CategoryName.Style.MessageTree")]
		[SortIndex(6)]
		[XmlIgnore]
		public Color FavoriteMessageColor
		{
			get { return _favoriteMessageColor; }
			set { SetColorNonTransp(out _favoriteMessageColor, value, _defaultFavoriteMessageColor); }
		}

		[Browsable(false)]
		public string FavoritesMessageColorStr
		{
			get { return _colorConverter.ConvertToInvariantString(_favoriteMessageColor); }
			set { SetColorNonTranspStr(out _favoriteMessageColor, value, _defaultFavoriteMessageColor); }
		}

		private Color _parentActiveMessageColor = _defaultParentActiveMessageBackColor;

		[JanusDisplayName("Config.StyleConfig.ParentActiveMessageBackgroundColor.DisplayName")]
		[JanusCategory("Config.CategoryName.Style.MessageTree")]
		[SortIndex(7)]
		[XmlIgnore]
		public Color ParentActiveMessageBackColor
		{
			get { return _parentActiveMessageColor; }
			set { SetColorNonTransp(out _parentActiveMessageColor, value, _defaultParentActiveMessageBackColor); }
		}

		[Browsable(false)]
		public string ParentActiveMessageBackColorStr
		{
			get { return _colorConverter.ConvertToInvariantString(_parentActiveMessageColor); }
			set { SetColorNonTranspStr(out _parentActiveMessageColor, value, _defaultParentActiveMessageBackColor); }
		}

		private Color _childActiveMessageColor = _defaultChildActiveMessageBackColor;
		
		[JanusDisplayName("Config.StyleConfig.ChildActiveMessageBackgroundColor.DisplayName")]
		[JanusCategory("Config.CategoryName.Style.MessageTree")]
		[SortIndex(8)]
		[XmlIgnore]
		public Color ChildActiveMessageBackColor
		{
			get { return _childActiveMessageColor; }
			set { SetColorNonTransp(out _childActiveMessageColor, value, _defaultChildActiveMessageBackColor); }
		}

		[Browsable(false)]
		public string ChildActiveMessageBackColorStr
		{
			get { return _colorConverter.ConvertToInvariantString(_childActiveMessageColor); }
			set { SetColorNonTranspStr(out _childActiveMessageColor, value, _defaultChildActiveMessageBackColor); }
		}

		#endregion

		#region 3.Настройки дерева навигации

		[JanusDisplayName("StyleConfigDisplayNameNavigationTreeFont")]
		[JanusCategory("Config.CategoryName.Style.NavigationTree")]
		[SortIndex(0)]
		[XmlIgnore]
		public Font NavigationTreeFont
		{
			get { return _navTreeFont; }
			set
			{
				_navTreeFont = value ?? _defaultNavigationTreeFont;
				OnStyleChangeEvent(StyleChangeEventArgs.FontColorStyle);
			}
		}

		[Browsable(false)]
		public string NavigationTreeFontStr
		{
			get { return _fontConverter.ConvertToInvariantString(_navTreeFont); }
			set
			{
				_navTreeFont = (Font) _fontConverter.ConvertFromInvariantString(value);
				OnStyleChangeEvent(StyleChangeEventArgs.FontColorStyle);
			}
		}

		private Font _navTreeFont = _defaultNavigationTreeFont;

		[JanusDisplayName("StyleConfigDisplayNameForumNamesColor")]
		[JanusCategory("Config.CategoryName.Style.NavigationTree")]
		[SortIndex(1)]
		[XmlIgnore]
		[Detailed]
		public Color NavigationTreeForumColor
		{
			get { return _navTreeForumColor; }
			set { SetColorNonTransp(out _navTreeForumColor, value, _defaultNavigationTreeForumColor); }
		}

		[Browsable(false)]
		public string NavigationTreeForumColorStr
		{
			get { return _colorConverter.ConvertToInvariantString(_navTreeForumColor); }
			set { SetColorNonTranspStr(out _navTreeForumColor, value, _defaultNavigationTreeForumColor); }
		}

		private Color _navTreeForumColor = _defaultNavigationTreeForumColor;

		[JanusDisplayName("StyleConfigDisplayNameNavigationTreeBackground")]
		[JanusCategory("Config.CategoryName.Style.NavigationTree")]
		[SortIndex(2)]
		[XmlIgnore]
		[Detailed]
		public Color NavigationTreeBack
		{
			get { return _navTreeBack; }
			set { SetColorNonTransp(out _navTreeBack, value, _defaultNavigationTreeBack); }
		}

		[Browsable(false)]
		public string NavigationTreeBackStr
		{
			get { return _colorConverter.ConvertToInvariantString(_navTreeBack); }
			set { SetColorNonTranspStr(out _navTreeBack, value, _defaultNavigationTreeBack); }
		}

		private Color _navTreeBack = _defaultNavigationTreeBack;

		[Browsable(false)]
		public Color[] ForumPriorityColor
		{
			get { return _defaultForumPriorityColor; }
		}

		#endregion

		#region Настройки диалога настроек (obsolete)

		[JanusDisplayName("StyleConfigDisplayNameOptionsHeaderColor")]
		[JanusCategory("Config.CategoryName.Style.OptionsDialog")]
		[SortIndex(0)]
		[XmlIgnore]
		[Detailed]
		public Color OptionsGridBack
		{
			get { return _propGridBack; }
			set { SetColorNonTransp(out _propGridBack, value, _defaultOptionsGridBack); }
		}

		[Browsable(false)]
		public string OptionsGridBackStr
		{
			get { return _colorConverter.ConvertToInvariantString(_propGridBack); }
			set { SetColorNonTranspStr(out _propGridBack, value, _defaultOptionsGridBack); }
		}

		private Color _propGridBack = _defaultOptionsGridBack;

		[JanusDisplayName("StyleConfigDisplayNameOptionsBackgroundColor")]
		[JanusCategory("Config.CategoryName.Style.OptionsDialog")]
		[SortIndex(1)]
		[XmlIgnore]
		[Detailed]
		public Color OptionsGridViewBack
		{
			get { return _propGridViewBack; }
			set { SetColorNonTransp(out _propGridViewBack, value, _defaultOptionsGridViewBack); }
		}

		[Browsable(false)]
		public string OptionsGridViewBackStr
		{
			get { return _colorConverter.ConvertToInvariantString(_propGridViewBack); }
			set { SetColorNonTranspStr(out _propGridViewBack, value, _defaultOptionsGridViewBack); }
		}

		private Color _propGridViewBack = _defaultOptionsGridViewBack;

		[JanusDisplayName("StyleConfigDisplayNameOptionsGridLinesColor")]
		[JanusCategory("Config.CategoryName.Style.OptionsDialog")]
		[SortIndex(2)]
		[XmlIgnore]
		[Detailed]
		public Color OptionsGridLine
		{
			get { return _propGridLine; }
			set { SetColorNonTransp(out _propGridLine, value, _defaultOptionsGridLine); }
		}

		[Browsable(false)]
		public string OptionsGridLineStr
		{
			get { return _colorConverter.ConvertToInvariantString(_propGridLine); }
			set { SetColorNonTranspStr(out _propGridLine, value, _defaultOptionsGridLine); }
		}

		private Color _propGridLine = _defaultOptionsGridLine;

		[JanusDisplayName("StyleConfigDisplayNameOptionsTextColor")]
		[JanusCategory("Config.CategoryName.Style.OptionsDialog")]
		[SortIndex(3)]
		[XmlIgnore]
		[Detailed]
		public Color OptionsGridText
		{
			get { return _propGridText; }
			set { SetColorNonTransp(out _propGridText, value, _defaultOptionsGridText); }
		}

		[Browsable(false)]
		public string OptionsGridTextStr
		{
			get { return _colorConverter.ConvertToInvariantString(_propGridText); }
			set { SetColorNonTranspStr(out _propGridText, value, _defaultOptionsGridText); }
		}

		private Color _propGridText = _defaultOptionsGridText;

		#endregion

		#region Просмотр сообщений

		#endregion

		#region 4.Редактирование сообщений

		private readonly Color[] _levelQuotaColors = (Color[])_defaultQuotaColors.Clone();

		[JanusDisplayName("WriteMessageQuotaLevel1Color")]
		[JanusDescription("WriteMessageQuotaLevel1Color")]
		[JanusCategory("Config.CategoryName.Style.WriteMessage")]
		[SortIndex(1)]
		[XmlIgnore]
		public Color Level1QuotaColor
		{
			get { return _levelQuotaColors[0]; }
			set { SetColorNonTransp(out _levelQuotaColors[0], value, _defaultQuotaColors[0]); }
		}

		[JanusDisplayName("WriteMessageQuotaLevel2Color")]
		[JanusDescription("WriteMessageQuotaLevel2Color")]
		[JanusCategory("Config.CategoryName.Style.WriteMessage")]
		[SortIndex(2)]
		[XmlIgnore]
		public Color Level2QuotaColor
		{
			get { return _levelQuotaColors[1]; }
			set { SetColorNonTransp(out _levelQuotaColors[1], value, _defaultQuotaColors[1]); }
		}

		[JanusDisplayName("WriteMessageQuotaLevel3Color")]
		[JanusDescription("WriteMessageQuotaLevel3Color")]
		[JanusCategory("Config.CategoryName.Style.WriteMessage")]
		[SortIndex(3)]
		[XmlIgnore]
		public Color Level3QuotaColor
		{
			get { return _levelQuotaColors[2]; }
			set { SetColorNonTransp(out _levelQuotaColors[2], value, _defaultQuotaColors[2]); }
		}

		[Browsable(false)]
		public string[] QuotaColorsStrs
		{
			get
			{
				var ret = new string[_levelQuotaColors.Length];
				for (var i = 0; i < _levelQuotaColors.Length; i++)
					ret[i] = _colorConverter.ConvertToInvariantString(_levelQuotaColors[i]);
				return ret;
			}
			set
			{
				var min = Math.Min(value.Length, _levelQuotaColors.Length);
				for (var i = 0; i < min; i++)
					SetColorNonTranspStr(out _levelQuotaColors[i], value[i], _defaultQuotaColors[i]);
			}
		}

		private Color _quotaPrefixColor = _defaultQuotaPrefixColor;

		[JanusDisplayName("QuotaPrefixColor")]
		[JanusDescription("QuotaPrefixColor")]
		[JanusCategory("Config.CategoryName.Style.WriteMessage")]
		[SortIndex(4)]
		[XmlIgnore]
		public Color QuotaPrefixColor
		{
			get { return _quotaPrefixColor; }
			set { SetColorNonTransp(out _quotaPrefixColor, value, _defaultQuotaPrefixColor); }
		}

		[Browsable(false)]
		public string QuotaPrefixColorStr
		{
			get { return _colorConverter.ConvertToInvariantString(_quotaPrefixColor); }
			set { SetColorNonTranspStr(out _quotaPrefixColor, value, _defaultQuotaPrefixColor); }

		}

		private const int _defaultWriteMessageTabSize = 2;
		private int _writeMessageTabSize = _defaultWriteMessageTabSize;

		[JanusDisplayName("WriteMessageTabSizeName")]
		[JanusDescription("WriteMessageTabSizeDescription")]
		[JanusCategory("Config.CategoryName.Style.WriteMessage")]
		[DefaultValue(_defaultWriteMessageTabSize)]
		[SortIndex(5)]
		public int WriteMessageTabSize
		{
			get { return _writeMessageTabSize; }
			set { _writeMessageTabSize = value; }
		}

		#endregion

		#region Менеджмент синглтона и событий

		//----------------- Служебные данные и методы----------------------
		private static readonly object _lockFlag = new object();
		private static StyleConfig _instance;

		/// <summary>
		/// Экземпляр синглтона схемы.
		/// </summary>
		[XmlIgnore]
		public static StyleConfig Instance
		{
			get
			{
				//Config.Instance; // Force config to load
				lock (_lockFlag)
				{
					if (_instance == null)
					{
						_instance = new StyleConfig();
					}
				}
				return _instance;
			}
		}

		/// <summary>
		/// Получить копию схемы.
		/// </summary>
		/// <returns>Клон текущей схемы</returns>
		public static StyleConfig GetClone()
		{
			return (StyleConfig) Instance.MemberwiseClone();
		}

		/// <summary>
		/// Установить новую схему.
		/// </summary>
		/// <param name="cfg">Схема</param>
		public static void NewStyleConfig(StyleConfig cfg)
		{
			_instance = cfg;
			if (_instance != null)
			{
				_instance.OnStyleChangeEvent(StyleChangeEventArgs.AllStyle);
			}
		}

		private void OnStyleChangeEvent(StyleChangeEventArgs e)
		{
			// Only singleton config can send notifications
			if (this == _instance)
			{
				if (StyleChange != null)
					StyleChange(this, e);
			}
		}

		public static event StyleChangeEventHandler StyleChange;

		#endregion

		#region Вспомогательные методы

		private void SetColorNonTransp(out Color store, Color val, Color def)
		{
			// Прозрачные цвета не подходят
			store = val.A != 0xff ? def : val;

			OnStyleChangeEvent(StyleChangeEventArgs.FontColorStyle);
		}

		private void SetColorNonTranspStr(out Color store, string val, Color def)
		{
			// Прозрачные цвета не подходят
			store = (Color) _colorConverter.ConvertFromInvariantString(val);
			if (store.A != 0xff)
				store = def;

			OnStyleChangeEvent(StyleChangeEventArgs.FontColorStyle);
		}

		#endregion

		#region Загрузка/сохранение

		private static XmlSerializer _serializer;

		private static XmlSerializer Serializer
		{
			get { return _serializer ?? (_serializer = new XmlSerializer(typeof (StyleConfig))); }
		}

		/// <summary>
		/// Загрузить схему.
		/// </summary>
		/// <param name="path">Файл, откуда будет загружена схема.</param>
		public static void Load(string path)
		{
			lock (_lockFlag)
			{
				try
				{
					using (var fs = new FileStream(path, FileMode.Open))
						NewStyleConfig((StyleConfig)Serializer.Deserialize(fs));
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message, ApplicationInfo.ApplicationName,
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					_instance = new StyleConfig();
				}
			}
		}

		/// <summary>
		/// Сохранить схему.
		/// </summary>
		/// <param name="path">Файл, куда сохранить схему</param>
		public static void Save(string path)
		{
			lock (_lockFlag)
			{
				var ms = new MemoryStream();
				Serializer.Serialize(ms, Instance);
				ms.Flush();

				var buf = ms.GetBuffer();
				if (ms.Length > 0)
					using (var fs = new FileStream(path, FileMode.Create))
						fs.Write(buf, 0, (int)ms.Length);
			}
		}

		#endregion
	}

	/// <summary>
	/// Обработчик события - изменение настройки стиля.
	/// </summary>
	public delegate void StyleChangeEventHandler(object s, StyleChangeEventArgs e);


	[Flags]
	public enum Style
	{
		All = FontColor | Images,
		FontColor = 1,
		Images    = 2
	}

	/// <summary>
	/// Параметры события - изменения стиля. Пока не содержит ничего, впоследствии
	/// возможно будет содержать название измененного параметра.
	/// </summary>
	public class StyleChangeEventArgs : EventArgs
	{
		public static readonly StyleChangeEventArgs AllStyle = 
			new StyleChangeEventArgs(Style.All);
		public static readonly StyleChangeEventArgs FontColorStyle = 
			new StyleChangeEventArgs(Style.FontColor);
		public static readonly StyleChangeEventArgs ImagesStyle = 
			new StyleChangeEventArgs(Style.Images);

		private readonly Style _style;

		private StyleChangeEventArgs(Style style)
		{
			_style = style;
		}

		/// <summary>
		/// Измененный стиль
		/// </summary>
		public Style Style
		{
			get { return _style; }
		}
	}
}