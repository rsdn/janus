using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public static class ParserExtensions
	{
		[NotNull]
		public static Parser<TInput, TValue> Where<TInput, TValue>(
			[NotNull] this Parser<TInput, TValue> parser,
			[NotNull] Func<TValue, bool> pred)
		{
			if (parser == null)
				throw new ArgumentNullException("parser");
			if (pred == null)
				throw new ArgumentNullException("pred");

			return input =>
				{
					var res = parser(input);
					if (res == null || !pred(res.Value))
						return null;
					return new ParsingResult<TInput, TValue>(res.InputRest, res.Value);
				};
		}

		[NotNull]
		public static Parser<TInput, TReturn> Select<TInput, TValue, TReturn>(
			[NotNull] this Parser<TInput, TValue> parser,
			[NotNull] Func<TValue, TReturn> projector)
		{
			if (parser == null)
				throw new ArgumentNullException("parser");
			if (projector == null)
				throw new ArgumentNullException("projector");

			return input =>
				{
					var res = parser(input);
					return res == null ? null : new ParsingResult<TInput, TReturn>(res.InputRest, projector(res.Value));
				};
		}

		[NotNull]
		public static Parser<TInput, TValue> Or<TInput, TValue>(
			[NotNull] this Parser<TInput, TValue> parser1,
			[NotNull] Parser<TInput, TValue> parser2)
		{
			if (parser1 == null)
				throw new ArgumentNullException("parser1");
			if (parser2 == null)
				throw new ArgumentNullException("parser2");

			return input => parser1(input) ?? parser2(input);
		}

		[NotNull]
		public static Parser<TInput, TReturn> And<TInput, TValue1, TValue2, TReturn>(
			[NotNull] this Parser<TInput, TValue1> parser1,
			[NotNull] Parser<TInput, TValue2> parser2,
			[NotNull] Func<TValue1, TValue2, TReturn> projector)
		{
			if (parser1 == null)
				throw new ArgumentNullException("parser1");
			if (parser2 == null)
				throw new ArgumentNullException("parser2");
			if (projector == null)
				throw new ArgumentNullException("projector");

			return input =>
				{
					var res1 = parser1(input);
					if (res1 == null)
						return null;
					var res2 = parser2(res1.InputRest);
					if (res2 == null)
						return null;
					return new ParsingResult<TInput, TReturn>(res2.InputRest, projector(res1.Value, res2.Value));
				};
		}

		[NotNull]
		public static Parser<TInput, TReturn> SelectMany<TInput, TValue1, TValue2, TReturn>(
			[NotNull] this Parser<TInput, TValue1> parser,
			[NotNull] Func<TValue1, Parser<TInput, TValue2>> selector,
			[NotNull] Func<TValue1, TValue2, TReturn> projector)
		{
			if (parser == null)
				throw new ArgumentNullException("parser");
			if (selector == null)
				throw new ArgumentNullException("selector");
			if (projector == null)
				throw new ArgumentNullException("projector");

			return input =>
				{
					var res = parser(input);
					if (res == null)
						return null;
					var res2 = selector(res.Value)(res.InputRest);
					if (res2 == null)
						return null;
					return new ParsingResult<TInput, TReturn>(res2.InputRest, projector(res.Value, res2.Value));
				};
		}
	}
}