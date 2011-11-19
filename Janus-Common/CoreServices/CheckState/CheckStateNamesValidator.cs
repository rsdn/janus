using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public static class CheckStateNamesValidator
	{
		public static bool IsValidCheckStateName([NotNull] string name)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			
			return CommonRegexes.FullQualyfiedName.IsMatch(name);
		}
	}
}