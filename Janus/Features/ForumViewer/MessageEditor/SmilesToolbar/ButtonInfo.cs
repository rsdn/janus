using System;
using System.Drawing;

namespace Rsdn.Janus
{
	/// <summary>
	/// Описание кнопки для контрола SmilesToolbar.
	/// </summary>
	public class ButtonInfo
	{
		public static readonly ButtonInfo NewLineButton =
			new ButtonInfo(Environment.NewLine, null, null, null);

		public ButtonInfo(string text, string hint, Image image, object tag)
		{
			_hint = hint;
			_tag = tag;
			_text = text;
			_image = image;
		}

		private readonly Image _image;

		/// <summary>
		/// Иконка выводимая в кнопке. Если она не задана выводится текст.
		/// </summary>
		public Image Image
		{
			get { return _image; }
		}

		private readonly object _tag;

		/// <summary>
		/// Тэг позволяющий ассоциировать с кнопкой другой объект.
		/// Вместо использования тэга можно создать наследника класса с нужными
		/// полями ButtonInfo и добавлять уже его.
		/// </summary>
		public object Tag
		{
			get { return _tag; }
		}

		private readonly string _text;

		/// <summary>
		/// Текст вывадимый в кнопке без картинок и в хинтах кнопок.
		/// В принципе можно ввести отедьные текст для тултипов.
		/// </summary>
		public string Text
		{
			get { return _text; }
		}

		private readonly string _hint;

		/// <summary>
		/// Подсказка (тултип).
		/// </summary>
		public string Hint
		{
			get { return _hint; }
		}
	}
}