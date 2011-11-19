using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public static class CommandContextHelper
	{
		public static void WriteLineToOutput(
			[NotNull] this ICommandContext context)
		{
			WriteLineToOutput(context, "");
		}

		public static void WriteLineToOutput(
			[NotNull] this ICommandContext context,
			[NotNull] string text)
		{
			if (context == null)
				throw new ArgumentNullException("context");
			if (text == null)
				throw new ArgumentNullException("text");

			context.WriteToOutput(text + Environment.NewLine);
		}
	}
}