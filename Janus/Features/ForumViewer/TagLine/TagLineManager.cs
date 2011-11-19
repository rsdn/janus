using System;

using JetBrains.Annotations;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Менеджер, создающий таглайн.
	/// </summary>
	[Service(typeof (ITagLineManager))]
	internal class TagLineManager : ITagLineManager
	{
		private const int MaxTagLength = 128;

		private readonly IServiceProvider _serviceProvider;

		public TagLineManager([NotNull] IServiceProvider serviceProvider)
		{
			if (serviceProvider == null) 
				throw new ArgumentNullException("serviceProvider");

			_serviceProvider = serviceProvider;
		}

		/// <summary>
		/// Возвращает таглайн по форматной строке.
		/// </summary>
		public string GetTagLine([NotNull] string format)
		{
			if (format == null)
				throw new ArgumentNullException("format");

			var res = TextMacrosHelper.ReplaceMacroses(_serviceProvider, format);

			return res.Length > MaxTagLength
				? res.Substring(0, MaxTagLength)
				: res;
		}

		/// <summary>
		/// Найти подходящий для форума тег-лайн
		/// </summary>
		public string FindAppropriateTagLine(int forumId)
		{
			var tagLineInfos = Config.Instance.TagLine.TagLineInfos;
			var defaultTagLine = string.Empty;

			if (tagLineInfos != null && tagLineInfos.Infos != null)
				foreach (var tgi in tagLineInfos.Infos)
				{
					if (tgi.Forums.Length == 0)
						continue;

					if (tgi.Forums[0] == TagLineInfo.AllForums)
						defaultTagLine = tgi.Format;
					else
						foreach (var fid in tgi.Forums)
							if (fid == forumId)
								return tgi.Format;
				}

			return defaultTagLine;
		}
	}
}