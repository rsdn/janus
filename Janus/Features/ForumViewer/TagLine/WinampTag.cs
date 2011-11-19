using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Rsdn.Janus
{
	/// <summary>
	/// Тег с текущей песней из винампа.
	/// </summary>
	public class WinampTag : ITaglineMacroProvider
	{
		private static string[] _macroIds = new string[]{"wanp"};

		private Regex wa3re;
		private Regex wa2re;

		public WinampTag()
		{
			wa3re = new Regex(@"(?<=\d*\.\s+)\S.*\S(?=\s*\(\d+:\d+\)\s+\(.+\))",
				RegexOptions.Compiled);
			wa2re = new Regex(@"(?<=\d*\.\s+)\S.*\S(?=\s+- Winamp)",RegexOptions.Compiled);
		}

		#region ITaglineMacroProvider Members
		public string[] MacroIds
		{
			get { return _macroIds; }
		}

		// приготовимся к платформенной зависимости.
		[DllImport("user32.dll", CharSet=CharSet.Auto)] 
		public static extern int FindWindow(string classname, string windowname);
		
		// "голая" версия. Берем просто байты. 
		[DllImport("user32.dll", CharSet=CharSet.Ansi)] 
		public static extern int GetWindowText(int hwnd, byte[] bytes,int maxcount);
		// "нормальная" версия. корректно показывает русский в 1251
		[DllImport("user32.dll", CharSet=CharSet.Unicode)] 
		public static extern int GetWindowText(int hwnd, StringBuilder text,int maxcount);

		public string GetValue(string macroId)
		{
			string wn;
			Regex re;
			if(Config.Instance.TagLine.WinampTagConfig.PlayerVersion == WinampVersion.Winamp3x)
			{
				wn = "STUDIO";
				re = wa3re;
			}
			else
			{
				wn = "Winamp v1.x";
				re = wa2re;
			}
			int hwa = FindWindow(wn,null);
			string tt;		
			try 
											{ // винумп 3 косячит и сует в заголовок окна UTF-8. 
												byte[] test = new byte[1000];
												GetWindowText(hwa,test,1000);
												tt = new System.Text.UTF8Encoding(false, true).GetString(test);
											} 
											catch (ArgumentException)
											{ // Ой! Это был не UTF8. попробуем взять ASCII...
												StringBuilder sb = new StringBuilder(1000);
												GetWindowText(hwa, sb, 1000);
												tt = sb.ToString();
											}
			string r = re.Match(tt).Value;
			if(r.Length == 0) // возможно это из-за скроллинга. 
				// попробуем выполнить склейку для поиска имени... 
				r = re.Match(tt+tt).Value; 
			if(r.Length == 0) // нет, не из-за скороллинга. 
				r = Config.Instance.TagLine.WinampTagConfig.SilentName; // Значит песен нет.
			return r;
		}
		#endregion
	}
}
