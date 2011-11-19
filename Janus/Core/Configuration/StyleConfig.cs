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
		private readonly bool _detail;

		private static readonly DetailedAttribute _detailed = new DetailedAttribute(true);

		public static DetailedAttribute DetailedInstance
		{
			get { return _detailed; }
		}

		public DetailedAttribute()
		{
			_detail = true;
		}

		public DetailedAttribute(bool detail)
		{
			_detail = detail;
		}

		public bool Detailed
		{
			get { return _detail; }
		}
	}

	/// <summary>
	/// Конфигурация стиля
	/// </summary>
	public class StyleConfig
	{
		#region Настройки по умолчанию

		public static readonly Color DefaultMessageTreeBack = Color.FromArgb(224, 240, 224);
		public static readonly Font  DefaultMessageTreeFont = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);

		public static readonly Color DefaultMessageColor              = Color.Black;
		public static readonly Color DefaultSelfMessageColor          = Color.SteelBlue;
		public static readonly Color DefaultRepliesToSelfMessageColor = Color.DarkRed;

		public static readonly Color DefaultFavoriteMessageColor = Color.Chocolate;

		public static readonly Color DefaultParentActiveMessageBackColor = Color.LightSteelBlue;
		public static readonly Color DefaultChildActiveMessageBackColor = Color.PowderBlue;

		public static readonly Color DefaultNavigationTreeForumColor = Color.Black;
		public static readonly Color DefaultNavigationTreeBack       = Color.FromArgb(244, 255, 244);
		public static readonly Font  DefaultNavigationTreeFont       = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);

		public static readonly Color DefaultOptionsGridBack     = Color.FromArgb(192, 192, 244);
		public static readonly Color DefaultOptionsGridViewBack = Color.FromArgb(244, 244, 255);
		public static readonly Color DefaultOptionsGridLine     = Color.FromArgb(224, 224, 244);
		public static readonly Color DefaultOptionsGridText     = SystemColors.WindowText;

		public static readonly Color DefaultQuotaPrefixColor = Color.FromArgb(0, 60, 180);

		public static readonly Color[] DefaultQuotaColors = new[]
			{
				Color.FromArgb(  0,  85,   0),
				Color.FromArgb( 60, 140,  60),
				Color.FromArgb(120, 200, 120)
			};

		public static readonly Color[] DefaultForumPriorityColor = new[]
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

		protected static ColorConverter _colorConverter = new ColorConverter();
		protected static FontConverter _fontConverter = new FontConverter();

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
				_msgTreeFont = value ?? DefaultMessageTreeFont;
				OnStyleChangeEvent(StyleChangeEventArgs.FontColorStyle);
			}
		}

		private Font _msgTreeFont = DefaultMessageTreeFont;

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
			set { SetColorNonTransp(ref _msgTreeBack, value, DefaultMessageTreeBack); }
		}

		[Browsable(false)]
		public string MessageTreeBackStr
		{
			get { return _colorConverter.ConvertToInvariantString(_msgTreeBack); }
			set { SetColorNonTranspStr(ref _msgTreeBack, value, DefaultMessageTreeBack); }
		}

		private Color _msgTreeBack = DefaultMessageTreeBack;

		[JanusDisplayName("Config.StyleConfig.MessageColor.DisplayName")]
		[JanusCategory("Config.CategoryName.Style.MessageTree")]
		[SortIndex(2)]
		[XmlIgnore]
		public Color MessageColor
		{
			get { return _msgColor; }
			set { SetColorNonTransp(ref _msgColor, value, DefaultMessageColor); }
		}

		[Browsable(false)]
		public string MessageColorStr
		{
			get { return _colorConverter.ConvertToInvariantString(_msgColor); }
			set { SetColorNonTranspStr(ref _msgColor, value, DefaultMessageColor); }
		}

		private Color _msgColor = DefaultMessageColor;

		[JanusDisplayName("Config.StyleConfig.OwnMessagesColor.DisplayName")]
		[JanusCategory("Config.CategoryName.Style.MessageTree")]
		[SortIndex(3)]
		[XmlIgnore]
		public Color SelfMessageColor
		{
			get { return _selfMsgColor; }
			set { SetColorNonTransp(ref _selfMsgColor, value, DefaultSelfMessageColor); }
		}

		[Browsable(false)]
		public string SelfMessageColorStr
		{
			get { return _colorConverter.ConvertToInvariantString(_selfMsgColor); }
			set { SetColorNonTranspStr(ref _selfMsgColor, value, DefaultSelfMessageColor); }
		}

		private Color _selfMsgColor = DefaultSelfMessageColor;

		[JanusDisplayName("Config.StyleConfig.MessagesAnswerColor.DisplayName")]
		[JanusCategory("Config.CategoryName.Style.MessageTree")]
		[SortIndex(4)]
		[XmlIgnore]
		public Color RepliesToSelfMessageColor
		{
			get { return _repliesMsgColor; }
			set { SetColorNonTransp(ref _repliesMsgColor, value, DefaultRepliesToSelfMessageColor); }
		}

		[Browsable(false)]
		public string RepliesToSelfMessageColorStr
		{
			get { return _colorConverter.ConvertToInvariantString(_repliesMsgColor); }
			set { SetColorNonTranspStr(ref _repliesMsgColor, value, DefaultRepliesToSelfMessageColor); }
		}

		private Color _repliesMsgColor = DefaultRepliesToSelfMessageColor;

		[JanusDisplayName("Config.StyleConfig.BrokenColor.DisplayName")]
		[JanusCategory("Config.CategoryName.Style.MessageTree")]
		[SortIndex(5)]
		[XmlIgnore]
		public Color MissingTopicColor
		{
			get { return _missingTopicColor; }
			set { SetColorNonTransp(ref _missingTopicColor, value, Color.FromKnownColor(KnownColor.Green)); }
		}

		protected Color _missingTopicColor = Color.FromKnownColor(KnownColor.Green);

		[Browsable(false)]
		public string MissingTopicColorStr
		{
			get { return _colorConverter.ConvertToInvariantString(_missingTopicColor); }
			set { SetColorNonTranspStr(ref _missingTopicColor, value, Color.FromKnownColor(KnownColor.Green)); }
		}

		protected Color _favoriteMessageColor = DefaultFavoriteMessageColor;

		[JanusDisplayName("Config.StyleConfig.FavoriteMessageColor.DisplayName")]
		[JanusCategory("Config.CategoryName.Style.MessageTree")]
		[SortIndex(6)]
		[XmlIgnore]
		public Color FavoriteMessageColor
		{
			get { return _favoriteMessageColor; }
			set { SetColorNonTransp(ref _favoriteMessageColor, value, DefaultFavoriteMessageColor); }
		}

		[Browsable(false)]
		public string FavoritesMessageColorStr
		{
			get { return _colorConverter.ConvertToInvariantString(_favoriteMessageColor); }
			set { SetColorNonTranspStr(ref _favoriteMessageColor, value, DefaultFavoriteMessageColor); }
		}

		protected Color _parentActiveMessageColor = DefaultParentActiveMessageBackColor;

		[JanusDisplayName("Config.StyleConfig.ParentActiveMessageBackgroundColor.DisplayName")]
		[JanusCategory("Config.CategoryName.Style.MessageTree")]
		[SortIndex(7)]
		[XmlIgnore]
		public Color ParentActiveMessageBackColor
		{
			get { return _parentActiveMessageColor; }
			set { SetColorNonTransp(ref _parentActiveMessageColor, value, DefaultParentActiveMessageBackColor); }
		}

		[Browsable(false)]
		public string ParentActiveMessageBackColorStr
		{
			get { return _colorConverter.ConvertToInvariantString(_parentActiveMessageColor); }
			set { SetColorNonTranspStr(ref _parentActiveMessageColor, value, DefaultParentActiveMessageBackColor); }
		}
		
		protected Color ChildActiveMessageColor = DefaultChildActiveMessageBackColor;
		
		[JanusDisplayName("Config.StyleConfig.ChildActiveMessageBackgroundColor.DisplayName")]
		[JanusCategory("Config.CategoryName.Style.MessageTree")]
		[SortIndex(8)]
		[XmlIgnore]
		public Color ChildActiveMessageBackColor
		{
			get { return ChildActiveMessageColor; }
			set { SetColorNonTransp(ref ChildActiveMessageColor, value, DefaultChildActiveMessageBackColor); }
		}

		[Browsable(false)]
		public string ChildActiveMessageBackColorStr
		{
			get { return _colorConverter.ConvertToInvariantString(ChildActiveMessageColor); }
			set { SetColorNonTranspStr(ref ChildActiveMessageColor, value, DefaultChildActiveMessageBackColor); }
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
				_navTreeFont = value ?? DefaultNavigationTreeFont;
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

		private Font _navTreeFont = DefaultNavigationTreeFont;

		[JanusDisplayName("StyleConfigDisplayNameForumNamesColor")]
		[JanusCategory("Config.CategoryName.Style.NavigationTree")]
		[SortIndex(1)]
		[XmlIgnore]
		[Detailed]
		public Color NavigationTreeForumColor
		{
			get { return _navTreeForumColor; }
			set { SetColorNonTransp(ref _navTreeForumColor, value, DefaultNavigationTreeForumColor); }
		}

		[Browsable(false)]
		public string NavigationTreeForumColorStr
		{
			get { return _colorConverter.ConvertToInvariantString(_navTreeForumColor); }
			set { SetColorNonTranspStr(ref _navTreeForumColor, value, DefaultNavigationTreeForumColor); }
		}

		private Color _navTreeForumColor = DefaultNavigationTreeForumColor;

		[JanusDisplayName("StyleConfigDisplayNameNavigationTreeBackground")]
		[JanusCategory("Config.CategoryName.Style.NavigationTree")]
		[SortIndex(2)]
		[XmlIgnore]
		[Detailed]
		public Color NavigationTreeBack
		{
			get { return _navTreeBack; }
			set { SetColorNonTransp(ref _navTreeBack, value, DefaultNavigationTreeBack); }
		}

		[Browsable(false)]
		public string NavigationTreeBackStr
		{
			get { return _colorConverter.ConvertToInvariantString(_navTreeBack); }
			set { SetColorNonTranspStr(ref _navTreeBack, value, DefaultNavigationTreeBack); }
		}

		private Color _navTreeBack = DefaultNavigationTreeBack;

		[Browsable(false)]
		public Color[] ForumPriorityColor
		{
			get { return DefaultForumPriorityColor; }
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
			set { SetColorNonTransp(ref _propGridBack, value, DefaultOptionsGridBack); }
		}

		[Browsable(false)]
		public string OptionsGridBackStr
		{
			get { return _colorConverter.ConvertToInvariantString(_propGridBack); }
			set { SetColorNonTranspStr(ref _propGridBack, value, DefaultOptionsGridBack); }
		}

		private Color _propGridBack = DefaultOptionsGridBack;

		[JanusDisplayName("StyleConfigDisplayNameOptionsBackgroundColor")]
		[JanusCategory("Config.CategoryName.Style.OptionsDialog")]
		[SortIndex(1)]
		[XmlIgnore]
		[Detailed]
		public Color OptionsGridViewBack
		{
			get { return _propGridViewBack; }
			set { SetColorNonTransp(ref _propGridViewBack, value, DefaultOptionsGridViewBack); }
		}

		[Browsable(false)]
		public string OptionsGridViewBackStr
		{
			get { return _colorConverter.ConvertToInvariantString(_propGridViewBack); }
			set { SetColorNonTranspStr(ref _propGridViewBack, value, DefaultOptionsGridViewBack); }
		}

		private Color _propGridViewBack = DefaultOptionsGridViewBack;

		[JanusDisplayName("StyleConfigDisplayNameOptionsGridLinesColor")]
		[JanusCategory("Config.CategoryName.Style.OptionsDialog")]
		[SortIndex(2)]
		[XmlIgnore]
		[Detailed]
		public Color OptionsGridLine
		{
			get { return _propGridLine; }
			set { SetColorNonTransp(ref _propGridLine, value, DefaultOptionsGridLine); }
		}

		[Browsable(false)]
		public string OptionsGridLineStr
		{
			get { return _colorConverter.ConvertToInvariantString(_propGridLine); }
			set { SetColorNonTranspStr(ref _propGridLine, value, DefaultOptionsGridLine); }
		}

		private Color _propGridLine = DefaultOptionsGridLine;

		[JanusDisplayName("StyleConfigDisplayNameOptionsTextColor")]
		[JanusCategory("Config.CategoryName.Style.OptionsDialog")]
		[SortIndex(3)]
		[XmlIgnore]
		[Detailed]
		public Color OptionsGridText
		{
			get { return _propGridText; }
			set { SetColorNonTransp(ref _propGridText, value, DefaultOptionsGridText); }
		}

		[Browsable(false)]
		public string OptionsGridTextStr
		{
			get { return _colorConverter.ConvertToInvariantString(_propGridText); }
			set { SetColorNonTranspStr(ref _propGridText, value, DefaultOptionsGridText); }
		}

		private Color _propGridText = DefaultOptionsGridText;

		#endregion

		#region Просмотр сообщений

		#endregion

		#region 4.Редактирование сообщений

		private readonly Color[] _levelQuotaColors = (Color[])DefaultQuotaColors.Clone();

		[JanusDisplayName("WriteMessageQuotaLevel1Color")]
		[JanusDescription("WriteMessageQuotaLevel1Color")]
		[JanusCategory("Config.CategoryName.Style.WriteMessage")]
		[SortIndex(1)]
		[XmlIgnore]
		public Color Level1QuotaColor
		{
			get { return _levelQuotaColors[0]; }
			set { SetColorNonTransp(ref _levelQuotaColors[0], value, DefaultQuotaColors[0]); }
		}

		[JanusDisplayName("WriteMessageQuotaLevel2Color")]
		[JanusDescription("WriteMessageQuotaLevel2Color")]
		[JanusCategory("Config.CategoryName.Style.WriteMessage")]
		[SortIndex(2)]
		[XmlIgnore]
		public Color Level2QuotaColor
		{
			get { return _levelQuotaColors[1]; }
			set { SetColorNonTransp(ref _levelQuotaColors[1], value, DefaultQuotaColors[1]); }
		}

		[JanusDisplayName("WriteMessageQuotaLevel3Color")]
		[JanusDescription("WriteMessageQuotaLevel3Color")]
		[JanusCategory("Config.CategoryName.Style.WriteMessage")]
		[SortIndex(3)]
		[XmlIgnore]
		public Color Level3QuotaColor
		{
			get { return _levelQuotaColors[2]; }
			set { SetColorNonTransp(ref _levelQuotaColors[2], value, DefaultQuotaColors[2]); }
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
					SetColorNonTranspStr(ref _levelQuotaColors[i], value[i], DefaultQuotaColors[i]);
			}
		}

		private Color _quotaPrefixColor = DefaultQuotaPrefixColor;

		[JanusDisplayName("QuotaPrefixColor")]
		[JanusDescription("QuotaPrefixColor")]
		[JanusCategory("Config.CategoryName.Style.WriteMessage")]
		[SortIndex(4)]
		[XmlIgnore]
		public Color QuotaPrefixColor
		{
			get { return _quotaPrefixColor; }
			set { SetColorNonTransp(ref _quotaPrefixColor, value, DefaultQuotaPrefixColor); }
		}

		[Browsable(false)]
		public string QuotaPrefixColorStr
		{
			get { return _colorConverter.ConvertToInvariantString(_quotaPrefixColor); }
			set { SetColorNonTranspStr(ref _quotaPrefixColor, value, DefaultQuotaPrefixColor); }

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
		private static readonly object lockFlag = new object();
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
				lock (lockFlag)
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

		private void SetColorNonTransp(ref Color _store, Color val, Color def)
		{
			// Прозрачные цвета не подходят
			_store = val.A != 0xff ? def : val;

			OnStyleChangeEvent(StyleChangeEventArgs.FontColorStyle);
		}

		private void SetColorNonTranspStr(ref Color _store, string val, Color def)
		{
			// Прозрачные цвета не подходят
			_store = (Color) _colorConverter.ConvertFromInvariantString(val);
			if (_store.A != 0xff)
				_store = def;

			OnStyleChangeEvent(StyleChangeEventArgs.FontColorStyle);
		}

		#endregion

		#region Загрузка/сохранение

		private static XmlSerializer serializer;

		private static XmlSerializer Serializer
		{
			get
			{
				if (serializer == null)
				{
					serializer = new XmlSerializer(typeof (StyleConfig));
				}
				return serializer;
			}
		}

		/// <summary>
		/// Загрузить схему.
		/// </summary>
		/// <param name="path">Файл, откуда будет загружена схема.</param>
		public static void Load(string path)
		{
			lock (lockFlag)
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
			lock (lockFlag)
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

		public StyleChangeEventArgs(Style style)
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