using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public static class SafeRunExtensions
	{
		public static void SafeForeach<T>(
			[NotNull] this IEnumerable<T> source,
			[NotNull] Action<T> action,
			Action<T, Exception> failAction)
		{
			#region Args check
			if (source == null)
				throw new ArgumentNullException("source");
			if (action == null)
				throw new ArgumentNullException("action");
			#endregion

			foreach (var item in source)
			{
				try
				{
					action(item);
				}
				catch (Exception ex)
				{
					if (failAction != null)
						failAction(item, ex);
				}
			}
		}

		public static void SafeFireEvent<T>(
			this T @event, 
			[NotNull] Action<T> invokeAction, 
			Action<T, Exception> failAction)
			where T : class
		{
			#region Args check
			if (invokeAction == null)
				throw new ArgumentNullException("invokeAction");
			#endregion

			if (@event == null)
				return;
			((Delegate)(object)@event)
				.GetInvocationList()
				.Cast<T>()
				.SafeForeach(invokeAction, failAction);
		}

		public static void SafeOnNext<T>(
			[NotNull] this Subject<T> subject,
			T value,
			[CanBeNull] Action<Exception> onException)
		{
			if (subject == null) throw new ArgumentNullException("subject");
			try
			{
				subject.OnNext(value);
			}
			catch (Exception ex)
			{
				if (onException != null)
					onException(ex);
			}
		}

		public static void SafeOnNext<T>(
			[NotNull] this Subject<T> subject,
			T value)
		{
			SafeOnNext(subject, value, null);
		}
	}
}