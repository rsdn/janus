using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public abstract class MenuItemWithTextAndImage 
		: MenuItem, IMenuItemWithTextAndImage, IEquatable<MenuItemWithTextAndImage>
	{
		private readonly string _text;
		private readonly string _image;
		private readonly string _description;
		private readonly MenuItemDisplayStyle _displayStyle;

		protected MenuItemWithTextAndImage(
			string text, 
			string image, 
			string description, 
			MenuItemDisplayStyle displayStyle,
			int orderIndex)
			: base(orderIndex)
		{
			_text = text;
			_image = image;
			_description = description;
			_displayStyle = displayStyle;
		}

		[CanBeNull]
		public string Text
		{
			get { return _text; }
		}

		[CanBeNull]
		public string Image
		{
			get { return _image; }
		}

		[CanBeNull]
		public string Description
		{
			get { return _description; }
		}

		public MenuItemDisplayStyle DisplayStyle
		{
			get { return _displayStyle; }
		}

		public override string ToString()
		{
			return Text ?? string.Empty;
		}

		public bool Equals(MenuItemWithTextAndImage other)
		{
			return base.Equals(other) 
				&& Equals(other._text, _text) 
				&& Equals(other._image, _image) 
				&& Equals(other._description, _description) 
				&& Equals(other._displayStyle, _displayStyle);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as MenuItemWithTextAndImage);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var result = base.GetHashCode();
				result = (result*397) ^ (_text != null ? _text.GetHashCode() : 0);
				result = (result*397) ^ (_image != null ? _image.GetHashCode() : 0);
				result = (result*397) ^ (_description != null ? _description.GetHashCode() : 0);
				result = (result*397) ^ _displayStyle.GetHashCode();
				return result;
			}
		}
	}
}