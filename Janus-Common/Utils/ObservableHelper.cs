using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
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
					? Disposable.Empty
					: Disposable.Create(observable.OnCompleted);
		}
	}
}