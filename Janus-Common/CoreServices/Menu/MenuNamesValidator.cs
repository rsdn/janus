using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public static class MenuNamesValidator
	{
		public static bool IsValidMenuName([NotNull] string name)
		{
			if (name == null)
				throw new ArgumentNullException("name");

			return CommonRegexes.Name.IsMatch(name);
		}
	}
}