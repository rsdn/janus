using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class Parsers
	{
		[NotNull]
		public static Parser<TInput, TValue> Optional<TInput, TValue>([NotNull] Parser<TInput, TValue> parser)
			where TValue : class
		{
			if (parser == null)
				throw new ArgumentNullException("parser");

			return input =>
				{
				   var res = parser(input);
				   return res != null
					   ? new ParsingResult<TInput, TValue>(res.InputRest, res.Value)
					   : new ParsingResult<TInput, TValue>(input, null);
				};
		}

		[NotNull]
		public static Parser<TInput, TValue[]> ZeroOrMany<TInput, TValue>([NotNull] Parser<TInput, TValue> parser)
		{
			if (parser == null)
				throw new ArgumentNullException("parser");

			return input =>
				{
					var list = new List<TValue>();
					var rest = input;
					ParsingResult<TInput, TValue> res;
					while (!ReferenceEquals(rest, null) && (res = parser(rest)) != null)
					{
						list.Add(res.Value);
						rest = res.InputRest;
					}
					return new ParsingResult<TInput, TValue[]>(rest, list.ToArray());
				};
		}

		[NotNull]
		public static Parser<TInput, TValue[]> OneOrMany<TInput, TValue>([NotNull] Parser<TInput, TValue> parser)
		{
			if (parser == null)
				throw new ArgumentNullException("parser");

			return input =>
			{
				var firstRes = parser(input);
				if (firstRes == null)
					return null;
				var res = ZeroOrMany(parser)(firstRes.InputRest);
				return new ParsingResult<TInput, TValue[]>(
					res.InputRest,
					new[] { firstRes.Value }.Concat(res.Value).ToArray());
			};
		}
	}
}