using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Rsdn.Janus.Framework;

namespace Rsdn.Janus
{
	public delegate void ButtonClickHandler(object sender, ButtonInfo buttonInfo);

	/// <summary>
	/// Тулбар для смайликов. Позволяет добавлять автоматически форматируемые
	/// кнопки содержадие текст или картинки. Список кнопок хратится 
	/// в свойстве ButtonInfos.
	/// </summary>
	[DefaultEvent("ButtonClick")]
	public partial class SmilesToolbar : UserControl
	{
		/// <summary>
		/// Если в списке кнопок найдётся кнопка со следующем текстом, то следующие за ней кнопки
		/// будут отрисовываться с новой строки. Сама кнопка показана не будет.
		/// </summary>
		public static readonly string NewLineButtonText = Environment.NewLine;

		public SmilesToolbar()
		{
			_buttonInfos.ButtonInfoAdded += ButtonInfoAdded;
			_buttonInfos.ButtonInfoRemoved += ButtonInfoRemoved;
			_buttonInfos.ButtonInfoChanged += ButtonInfoChanged;

			InitializeComponent();
		}

		private readonly ButtonInfoCollection _buttonInfos = new ButtonInfoCollection();

		/// <summary>
		/// Список кнопк.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ButtonInfoCollection ButtonInfos
		{
			get { return _buttonInfos; }
		}

		//private ArrayList _buttons = new ArrayList(30);

		private void ButtonInfoAdded(ButtonInfoCollection sender, ButtonInfo buttonInfo)
		{
			var button = buttonInfo.Image == null
				? (Control)new Label { Text = buttonInfo.Text, AutoSize = true }
				: new PictureBox { SizeMode = PictureBoxSizeMode.AutoSize, Image = buttonInfo.Image };

			if (!IsSpecialButton(buttonInfo))
			{
				button.Click += SmilesToolbar_Click;
				button.Cursor = Cursors.Hand;
				_toolTip.SetToolTip(button, buttonInfo.Hint ?? buttonInfo.Text);
			}

			button.Tag = buttonInfo;
			Controls.Add(button);

			if (sender != null)
				UpdateButtons();
		}


		private int FindButton(ButtonInfo buttonInfo)
		{
			for (int i = 0, len = Controls.Count; i < len; i++)
			{
				Control control = Controls[i];
				if (control.Tag == buttonInfo)
					return i;
			}

			return -1;
		}

		private void ButtonInfoRemoved(ButtonInfoCollection sender, ButtonInfo buttonInfo)
		{
			int index = FindButton(buttonInfo);
			if (index < 0) // кнопка не найдена
				throw new ApplicationException(
					"Кнопка '" + buttonInfo.Text + "' не может быть удалена, так как она отсуствует в списке.");

			Control control = Controls[index];
			control.Click -= SmilesToolbar_Click;
			_toolTip.SetToolTip(control, ((ButtonInfo)control.Tag).Hint);
			Controls.RemoveAt(index);

			if (sender != null)
				UpdateButtons();
		}

		/// <summary>
		/// Когда этот флаг установлен, не производится обнавление положения
		/// контролов.
		/// </summary>
		private bool _isInit;

		private void ButtonInfoChanged(ButtonInfoCollection sender,
			ButtonInfo oldButtonInfo, ButtonInfo newButtonInfo)
		{
			ButtonInfoRemoved(null, oldButtonInfo);
			ButtonInfoAdded(null, newButtonInfo);
			UpdateButtons();
		}

		public void BeginUpdate()
		{
			_isInit = true;
		}

		public void EndUpdate()
		{
			_isInit = false;
			UpdateButtons();
		}

		/// <summary>
		/// Рассчитывает максимальный размер контрола входящего в диапазонэ
		/// </summary>
		/// <param name="startIndex">Индекс начала диапазона</param>
		/// <param name="endIndex">Индекс конца диапазона</param>
		/// <returns>Высота самого высокого контрола из диапазана</returns>
		private int CalcMaxHeight(int startIndex, int endIndex)
		{
			if (startIndex < 0)
				throw new ArgumentOutOfRangeException("startIndex");

			if (endIndex >= Controls.Count)
				throw new ArgumentOutOfRangeException("endIndex");

			int maxHeight = 0;

			for (int i = startIndex; i <= endIndex; i++)
			{
				Control control = Controls[i];
				maxHeight = Math.Max(maxHeight, control.Height);
			}

			return maxHeight;
		}

		/// <summary>
		/// Размер отступа между кнопками.
		/// </summary>
		private const int separatorSize = 5;

		/// <summary>
		/// Распологает кнопки в контроле в порядке в котором 
		/// они встречаются в списке контролов.
		/// </summary>
		private void UpdateButtons()
		{
			if (_isInit)
				return;

			// Сдвижка текущего ряда кнопок (в пикселях) от верха контрола.
			int topOffset = separatorSize;
			SuspendLayout();
			try
			{
				// Ширина контрола с учетом отступов.
				int width = Width - separatorSize * 2;
				int leftOffset = separatorSize; // Сдвижка слева (в пикселях)
				// Индекс контрола находящегося в начале строки
				int startRowControl = 0;
				// Индекс верхней границы массива контролов.
				int lowerBound = Controls.Count - 1;
				// Перебираем все контролы/кнопки...
				for (int i = 0; i <= lowerBound; i++)
				{
					Control control = Controls[i];
					ButtonInfo buttonInfo = (ButtonInfo)control.Tag;
					//control.BackColor = Color.Brown;
					// Рассчитываем новый отступ с лева
					int newLeftOffset = leftOffset + control.Width + separatorSize;
					// Если это последняя строка, или текст равен разрыву строки, 
					// или следующий контрол заходит за границы кортрола...
					if (i == lowerBound ||
						buttonInfo.Text == NewLineButtonText ||
						newLeftOffset + Controls[i + 1].Width >= width)
					{
						// Выполняем расчет высоты ряда
						int rowHeight = CalcMaxHeight(startRowControl, i);
						// Располагаем кнопки друг за другом
						AlignRowOfControls(topOffset, rowHeight, startRowControl, i);
						// Инициализируем значения для нового ряда...
						startRowControl = i + 1;
						topOffset += rowHeight + separatorSize;
						leftOffset = separatorSize;
						continue;
					}

					leftOffset = newLeftOffset;
				}
			}
			finally
			{
				ResumeLayout(true);
				// Мы находимся в обработчике WM_SIZE и если попытаться измениь
				// размер контрола прямо здесь, то можно и обломаться.
				// Поэтому посылаем PostMessage в котором и производим изменение
				// размеров контрола.
				if (Site == null) // Деалаем это только если мы не в дизантайме.
					PostMessage(Handle, WM_UpdateHeight, topOffset, 0);
			}
		}

		/// <summary>
		/// Идентификатор сообщения при приходе которого обнавляется 
		/// размеры контрола.
		/// </summary>
		private const int WM_UpdateHeight = NativeMethods.WM_USER + 0x100;

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_UpdateHeight)
			{
				// Объновляем высотку контрола... только если не находимся 
				// в дизантайме (Site == null).
				if (Site == null)
					Height = (int)m.WParam;
				return;
			}
			base.WndProc(ref m);
		}

		[DllImport("user32.dll")]
		private static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		/// <summary>
		/// Распологает контролы из переданного диапазана друг за другом.
		/// </summary>
		/// <param name="topOffset">Отступ сверху контроал (для ряда)</param>
		/// <param name="rowHeight">Высота ряда</param>
		/// <param name="startIndex">Индекс начала диапазона</param>
		/// <param name="endIndex">Индекст конца диапазона</param>
		private void AlignRowOfControls(
			int topOffset, int rowHeight, int startIndex, int endIndex)
		{
			if (startIndex < 0)
				throw new ArgumentOutOfRangeException("startIndex");

			if (endIndex >= Controls.Count)
				throw new ArgumentOutOfRangeException("endIndex");

			// Отступ слева.
			int leftOffset = separatorSize;

			// Перебираем контролы и выравниваем их.
			for (int i = startIndex; i <= endIndex; i++)
			{
				Control control = Controls[i];
				control.Top = topOffset + (rowHeight - control.Height) / 2;
				control.Left = leftOffset;
				leftOffset += separatorSize + control.Width;
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			UpdateButtons();
		}

		// Ловим назатие на кнопки и транслируем событие ButtonClick 
		// криенту.
		private void SmilesToolbar_Click(object sender, EventArgs e)
		{
			ButtonInfo info = (ButtonInfo)((Control)sender).Tag;
			OnButtonClick(info);
		}

		/// <summary>
		/// Вызывается когда пользователь нажимает одну из кнопок.
		/// </summary>
		public event ButtonClickHandler ButtonClick;

		/// <summary>
		/// Вызвает событие ButtonClick.
		/// </summary>
		/// <param name="buttonInfo">Описание нажатой кнопки</param>
		protected virtual void OnButtonClick(ButtonInfo buttonInfo)
		{
			if (ButtonClick != null)
				ButtonClick(this, buttonInfo);
		}

		/// <summary>
		/// Проверка кнопки на её "специальное" использование.
		/// На "специальных" кнопках не меняется курсор и не отлавливается нажатие мыши.
		/// </summary>
		/// <param name="buttonInfo">Описание проверяемой кнопки</param>
		/// <returns><c>true</c>, если кнопка является "специальной"</returns>
		private static bool IsSpecialButton(ButtonInfo buttonInfo)
		{
			if (buttonInfo == null)
				throw new ArgumentNullException("buttonInfo");

			return buttonInfo.Text == NewLineButtonText;
		}
	}
}