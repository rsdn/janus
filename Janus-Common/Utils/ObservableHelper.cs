using System;
using System.Reactive.Subjects;

using CodeJam;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public static class ObservableHelper
	{
		[NotNull]
		public static IDisposable CompleteHelper<T>([CanBeNull] this Subject<T> observable)
		{
			return
				observable == null
					? (IDisposable) Disposable.Empty
					: Disposable.Create(observable.OnCompleted);
		}
	}
}