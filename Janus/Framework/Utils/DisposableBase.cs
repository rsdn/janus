using System;
using System.Diagnostics;

namespace Rsdn.Janus.Framework
{
	/// <summary>
	/// Базовый класс для объектов, реализующих IDisposable.
	/// Освобождает от некоторого количества ручной работы.
	/// </summary>
	public class DisposableBase : IDisposable
	{
		private bool _disposed;

		#region IDisposable Members
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion

		~DisposableBase()
		{
			Dispose(false);
		}

		/// <summary>
		/// Особенности реализации IDisposable - описаны в MSDN.
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
					FreeManaged();
				FreeNotManaged();
			}
			_disposed = true;
		}

		/// <summary>
		/// Метод должен перекрываться в потомке.
		/// Чистит unmanaged-ресурсы.
		/// </summary>
		protected virtual void FreeNotManaged()
		{}

		/// <summary>
		/// Метод должен перекрываться в потомке.
		/// Чистит managed-ресурсы.
		/// </summary>
		protected virtual void FreeManaged()
		{}

		/// <summary>
		/// Для потомка - отладочная проверка - не был ли объект уже освобождён.
		/// </summary>
		protected void CheckNotDisposed()
		{
			Debug.Assert(!_disposed);
		}
	}
}