using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Rsdn.Janus.Framework
{
	#region Using directives
	
	#endregion Using directives

	/// <summary>
	/// Класс, помогающий определять задержку пользователем мыши в определённой точке экрана.
	/// </summary>
	public sealed class DragExpandDetector
	{
		#region Fields
		/// <summary>
		/// Таймаут по-умолчанию для определения задержки пользователем мыши в определённой точке экрана.
		/// </summary>
		private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(1D);

		/// <summary>
		/// Позиция курсора мыши во время последней операции детектирования.
		/// </summary>
		private Point? _lastDetectPosition;

		/// <summary>
		/// Время последнего вызова операции детектирования.
		/// </summary>
		private DateTime? _lastDetectTime;

		/// <summary>
		/// Таймаут для определения задержки пользователем мыши в определённой точке экрана.
		/// </summary>
		private TimeSpan _timeout = DefaultTimeout;
		#endregion Fields

		#region Constructor\Descructor
		/// <summary>
		/// Конструктор объекта <see cref="Rsdn.Janus.Framework.DragExpandDetector"/>.
		/// </summary>
		public DragExpandDetector()
		{
			Reset();
		}
		#endregion Constructor\Descructor

		#region Methods
		/// <summary>
		/// Детектирование неподвижного нахождения указателя мыши над елументом управления.
		/// </summary>
		/// <returns></returns>
		public bool Detect()
		{
			if (_lastDetectTime.HasValue) // Не первый вызов детектирования
			{
				Debug.Assert(_lastDetectPosition.HasValue, "_lastDetectPosition.HasValue");
				if (_lastDetectPosition.Value == Cursor.Position) // Позиция указателя мыши не сменилась
				{
					if (DateTime.Now - _lastDetectTime.Value > Timeout)
						// Указатель мыши не двигался определённое полем Timeout время.
					{
						Reset();
						return true;
					} //if
					return false; // Мышь не перемещается, ждём следеющего вызова.
				} //if
			} //if

			// Изменилась позиция указателя мыши. Выполняем переинициализацию.
			_lastDetectTime = DateTime.Now;
			_lastDetectPosition = Cursor.Position;
			return false;
		}

		/// <summary>
		/// Метод сбрасывает сохранённые значения предыдущих вызовов..
		/// </summary>
		public void Reset()
		{
			_lastDetectTime = null;
			_lastDetectPosition = null;
		}
		#endregion Methods

		#region Properties
		/// <summary>
		/// Интервал времени, в течении которого указатель мыши должен неподвижно находиться над элементом управления.
		/// </summary>
		public TimeSpan Timeout
		{
			[DebuggerStepThrough]
			get { return _timeout; }

			[DebuggerStepThrough]
			set { _timeout = value; }
		}
		#endregion Properties
	}
}