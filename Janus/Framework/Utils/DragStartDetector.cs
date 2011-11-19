using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Rsdn.Janus.Framework
{
	#region Using directives
	
	#endregion Using directives

	/// <summary>
	/// Класс, помогающий определять начало операции перетаскивания.
	/// </summary>
	public sealed class DragStartDetector
	{
		#region Fields
		/// <summary>
		/// Флаг, показывающий что было определено начало операции перетаскивания.
		/// </summary>
		private bool _isDetected;

		/// <summary>
		/// Флаг, показывающий, что операция инициирована.
		/// </summary>
		private bool _isDetecting;

		/// <summary>
		/// Позиция указателя мыши при начале детектирования операции перетаскивания (в экранных координатах).
		/// </summary>
		private Point _startLocation;
		#endregion Fields

		#region Constructor\Descructor
		/// <summary>
		/// Конструктор класса.
		/// </summary>
		public DragStartDetector()
		{
			Reset();
		}
		#endregion Constructor\Descructor

		#region Methods
		/// <summary>
		/// Обработчик события <see cref="Control.MouseDown"/>.
		/// </summary>
		/// <param name="e">Экземпляр <see cref="MouseEventArgs"/>, содержащий информацию о параметрах события.</param>
		public void MouseDown(MouseEventArgs e)
		{
			if (e == null)
				throw new ArgumentNullException("e");

			_isDetecting = true;
			_isDetected = false;
			_startLocation = new Point(e.X, e.Y);
		}

		/// <summary>
		/// Обработчик события <see cref="Control.MouseMove"/>.
		/// </summary>
		/// <param name="e">Экземпляр <see cref="MouseEventArgs"/>, содержащий инфлрмацию о параметрах события.</param>
		/// <returns><c>true</c>, если была определена операция Drag'n'Drop.</returns>
		public bool MouseMove(MouseEventArgs e)
		{
			if (e == null)
				throw new ArgumentNullException("e");

			if (_isDetecting)
				_isDetected = _isDetected ||
					(Math.Abs(_startLocation.X - e.X) > SystemInformation.DragSize.Height ||
						Math.Abs(_startLocation.Y - e.Y) > SystemInformation.DragSize.Width);
				//if(!_isDetected && 
				//  Math.Abs(_startLocation.X - e.X) > SystemInformation.DragSize.Height || 
				//  Math.Abs(_startLocation.Y - e.Y) > SystemInformation.DragSize.Width)
				//{
				//  _isDetected = true;
				//  return true;
				//}//if

			return _isDetected;
		}

		/// <summary>
		/// Обработчик события <see cref="System.Windows.Forms.Control.MouseUp"/>.
		/// </summary>
		/// <param name="e">Экземпляр <see cref="T:System.Windows.Forms.MouseEventArgs"/>, содержащий инфлрмацию о параметрах события.</param>
		public void MouseUp(MouseEventArgs e)
		{
			if (e == null)
				throw new ArgumentNullException("e");

			if (_isDetecting)
				Reset();
		}

		/// <summary>
		/// Отмена определения операции Drag'n'Drop.
		/// </summary>
		public void Reset()
		{
			_isDetecting = false;
			_isDetected = false;
			_startLocation = Point.Empty;
		}
		#endregion Methods

		#region Properties
		/// <summary>
		/// Current catch state.
		/// </summary>
		public bool IsDetecting
		{
			[DebuggerStepThrough]
			get { return _isDetecting; }
		}

		/// <summary>
		/// Current catch state.
		/// </summary>
		public bool IsDetected
		{
			[DebuggerStepThrough]
			get { return _isDetected; }
		}
		#endregion Properties
	}
}