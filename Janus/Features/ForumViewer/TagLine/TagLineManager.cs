using System;
using System.Linq;

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
		private const int _maxTagLength = 128;

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

			return res.Length > _maxTagLength
				? res.Substring(0, _maxTagLength)
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
					else if (tgi.Forums.Any(fid => fid == forumId))
						return tgi.Format;
				}

			return defaultTagLine;
		}
	}
}