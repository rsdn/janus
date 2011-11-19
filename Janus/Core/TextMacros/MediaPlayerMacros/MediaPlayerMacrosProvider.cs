using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.Win32;

using Rsdn.Janus.Core.TextMacros.MediaPlayerMacros;

namespace Rsdn.Janus
{
	/// <summary>
	/// Макрос текущей песней из медиаплееера.
	/// </summary>
	[TextMacrosProvider]
	internal sealed class MediaPlayerMacrosProvider : ITextMacrosProvider
	{
		#region Windows Media Player Data

		private const string WMP_MetadataKey = "Software\\Microsoft\\MediaPlayer\\CurrentMetadata";
		private const string WMP_TitleValue = "Title";
		private const string WMP_NameValue = "Name";
		private const string WMP_AuthorValue = "Author";

		#endregion

		#region Ultra Data

		private const string UltraConfigDirectoryRegistryKey = @"Software\Digiton\Ultra\1.0\External";
		private const string UltraConfigDirectoryValueName = "ConfigDir";
		private const string UltraAliasesFileName = "Aliases.ini";
		private const string UltraBroadPathSectionName = "PlayList";
		private const string UltraBroadPathValueName = "BroadPath";
		private const string UltraBroadStateFileName = "Broad.int";
		private const int UltraMaxPath = 255;
		private const int UltraBroadStateRecordCharsCount = 0x30b;
		private const int UltraBroadStateTitleCharsCount = 0x10d;
		private const int UltraBroadStateDurationStartCharsCount = 0x20c;

		#endregion

		private static readonly Regex _wa3re =
			new Regex(@"(?<=\d*\.\s+)\S.*\S(?=\s*\(\d+:\d+\)\s+\(.+\))", RegexOptions.Compiled);
		private static readonly Regex _wa2re =
			new Regex(@"(?<=\d*\.\s+)\S.*\S(?=\s+- Winamp)", RegexOptions.Compiled);

		private static string GetValue(IServiceProvider serviceProvider)
		{
			string wn;
			Regex re;

			switch (Config.Instance.TagLine.MediaPlayerTagConfig.MediaPlayer)
			{
				case MediaPlayerType.Winamp2x:
					wn = "Winamp v1.x";
					re = _wa2re;
					break;
				case MediaPlayerType.Winamp3x:
					wn = "STUDIO";
					re = _wa3re;
					break;
				case MediaPlayerType.Foobar2000:
					//Класс окна плагина foo_rsdn_np.dll
					wn = "FOOBAR_RSDN_NP";
					re = null;
					break;
				case MediaPlayerType.WindowsMedia:
					return RetrieveMediaPlayerInfo();
				case MediaPlayerType.Ultra:
					return RetrieveUltraInfo();
				default:
					throw new Exception();
			}//switch

			var hw = FindWindow(wn, null);
			string wt;

			var sb = new StringBuilder(1000);
			GetWindowText(hw, sb, 1000);
			if (Config.Instance.TagLine.MediaPlayerTagConfig.MediaPlayer == MediaPlayerType.Winamp3x)
			{
				//Если убрать перекодировку то будет косяк с русскими буквами
				var test = Encoding.Default.GetBytes(sb.ToString());
				wt = new UTF8Encoding(false, true).GetString(test);
			}
			else
				wt = sb.ToString();

			string s;

			if (Config.Instance.TagLine.MediaPlayerTagConfig.MediaPlayer == MediaPlayerType.Winamp3x |
				Config.Instance.TagLine.MediaPlayerTagConfig.MediaPlayer == MediaPlayerType.Winamp2x)
			{
				s = re.Match(wt).Value;
				if (s.Length == 0) // возможно это из-за скроллинга. 
					// попробуем выполнить склейку для поиска имени... 
					s = re.Match(wt + wt).Value;
			}
			else
				s = wt;

			return s.Length != 0 ? s : Config.Instance.TagLine.MediaPlayerTagConfig.SilentString;

		}

		#region Private methods

		// приготовимся к платформенной зависимости.
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern int FindWindow(string classname, string windowname);

		// "голая" версия. Берем просто байты. 
		[DllImport("user32.dll", CharSet = CharSet.Ansi)]
		private static extern int GetWindowText(int hwnd, byte[] bytes, int maxcount);
		// "нормальная" версия. корректно показывает русский в 1251
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		private static extern int GetWindowText(int hwnd, StringBuilder text, int maxcount);

		// For Ultra player
		[DllImport("Kernel32.dll")]
		private static extern uint GetPrivateProfileString(string section, string entry, string defaultValue, StringBuilder result, uint resultSize, string fileName);

		private static string RetrieveMediaPlayerInfo()
		{
			var key = Registry.CurrentUser.OpenSubKey(WMP_MetadataKey);
			if (key == null)
				return Config.Instance.TagLine.MediaPlayerTagConfig.SilentString;

			try
			{
				var value = key.GetValue(WMP_TitleValue);
				if (value == null)
				{
					value = key.GetValue(WMP_NameValue);
					if (value == null)
						return Config.Instance.TagLine.MediaPlayerTagConfig.SilentString;
				}

				var title = value.ToString();
				if (title.Length == 0)
					return Config.Instance.TagLine.MediaPlayerTagConfig.SilentString;

				value = key.GetValue(WMP_AuthorValue);

				if (value == null)
					return title;

				var author = value.ToString();
				return author.Length == 0
					? title
					: string.Format("{0} - {1}", author, title);
			}
			finally
			{
				key.Close();
			}
		}

		private static string RetrieveUltraInfo()
		{
			using (var regKey = Registry.LocalMachine.OpenSubKey(UltraConfigDirectoryRegistryKey))
			{
				if (regKey != null)
				{
					var configPath = (string)regKey.GetValue(UltraConfigDirectoryValueName);
					if (configPath != null && Directory.Exists(configPath))
					{
						var broadPathBuilder = new StringBuilder(UltraMaxPath);
						if (GetPrivateProfileString(UltraBroadPathSectionName,
													UltraBroadPathValueName,
													String.Empty, // defaultValue
													broadPathBuilder,
													checked((uint)broadPathBuilder.Capacity),
													Path.Combine(configPath, UltraAliasesFileName)) > 0)
						{
							var broadStateFileName = Path.Combine(broadPathBuilder.ToString(), UltraBroadStateFileName);
							if (File.Exists(broadStateFileName))
							{
								using (var stream = new FileStream(broadStateFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
								{
									using (var reader = new BinaryReader(stream, Encoding.ASCII))
									{
										try
										{
											var recordChars = reader.ReadChars(UltraBroadStateRecordCharsCount);
											var startTimeString = new string(recordChars, 0, "ddMMyyhhmmss".Length);
											var day = Int32.Parse(startTimeString.Substring(0, 2), NumberStyles.None);
											var month = Int32.Parse(startTimeString.Substring(2, 2), NumberStyles.None);
											var year = Int32.Parse(startTimeString.Substring(4, 2), NumberStyles.None) + 2000; // Two digits for year
											var hour = Int32.Parse(startTimeString.Substring(6, 2), NumberStyles.None);
											var minute = Int32.Parse(startTimeString.Substring(8, 2), NumberStyles.None);
											var second = Int32.Parse(startTimeString.Substring(10, 2), NumberStyles.None);
											var startTime = new DateTime(year, month, day, hour, minute, second);
											var durationString = new string(recordChars, UltraBroadStateDurationStartCharsCount, "00:00:00".Length);
											var duration = TimeSpan.Parse(durationString);
											if (DateTime.Now - startTime < duration)
											{
												var offsetTag = "ddMMyyhhmmss00".Length;
												var tag = new string(recordChars, offsetTag, UltraBroadStateTitleCharsCount - offsetTag);
												tag = tag.TrimEnd(' ', '\0');
												if (String.IsNullOrEmpty(tag))
												{ // Read path to file.
													offsetTag = UltraBroadStateTitleCharsCount;
													tag = new string(recordChars, offsetTag, UltraBroadStateDurationStartCharsCount - offsetTag);
													tag = tag.TrimEnd(' ');
													try
													{
														tag = Path.GetFileNameWithoutExtension(tag);
													}
													catch (ArgumentException e)
													{
														Trace.TraceError(e.ToString());
														return e.ToString();
													}//try
												}//if
												return tag;
											}//if
										}
										catch (EndOfStreamException e)
										{
											Trace.TraceError(e.ToString());
											return e.ToString();
										}
										catch (IOException e)
										{
											Trace.TraceError(e.ToString());
											return e.ToString();
										}
										catch (FormatException e)
										{
											Trace.TraceError(e.ToString());
											return e.ToString();
										}//try
									}//using
								}//using
							}//if
						}//if
					}//if
				}//if
			}//using

			return Config.Instance.TagLine.MediaPlayerTagConfig.SilentString;
		}

		#endregion

		public IEnumerable<ITextMacros> CreateTextMacroses()
		{
			return new[] { new TextMacros("wanp", MediaPlayerMacrosResources.MediaPlayerTrack, GetValue) };
		}
	}
}