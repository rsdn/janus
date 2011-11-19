using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public static class CommandNamesValidator
	{
		public static bool IsValidCommandName([NotNull] string name)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			
			return CommonRegexes.FullQualyfiedName.IsMatch(name);
		}

		public static bool IsValidCommandParameterName([NotNull] string name)
		{
			if (name == null)
				throw new ArgumentNullException("name");

			return CommonRegexes.Name.IsMatch(name);
		}
	}
}