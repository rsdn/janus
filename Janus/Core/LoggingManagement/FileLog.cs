using System;
using System.IO;
using System.Text;

namespace Rsdn.Janus
{
#if DEBUG
	/// <summary>
	/// Логгер в файл
	/// </summary>
	public class FileLog
	{
		public FileLog(ILogger logger)
		{
			logger.OnLog += OnLog;
		}

		private readonly string[] logTypeMessages = { "[i] ", "[*] ", "[!] ", "[h]" };

		protected void OnLog(object sender, LogEventArgs a)
		{
			try
			{
				if (Config.Instance.UseFileLog && a.Item.Type != LogEventType.Track)
					using (var sw =
							new StreamWriter(new FileStream(Config.Instance.LogFileName,
								FileMode.Append,
								FileAccess.Write,
								FileShare.Read),
							Encoding.GetEncoding(1251)))
					{
						sw.Write(logTypeMessages[(int)a.Item.Type]);
						sw.WriteLine(a.Item.Message);
					}
			}
//{{{-EmptyGeneralCatchClause
			catch (Exception)
//{{{+EmptyGeneralCatchClause
			{
				//hmm... we can't even create log file. And we shouldn't
				//disturb user every log message with message box.
				//so just ignore it for now.
			}
		}
	}
#endif
}
