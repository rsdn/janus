using System;
using System.Drawing;
using System.Reactive.Linq;
using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class StaticNavigationItemHeader : INavigationItemHeader
	{
		private readonly string _displayName;
		private readonly string _info;
		private readonly Image _image;
		private readonly bool _isHighlighted;

		#region Constructors

		public StaticNavigationItemHeader([NotNull] string displayName)
			: this(displayName, null) { }

		public StaticNavigationItemHeader([NotNull] string displayName, Image image)
			: this(displayName, null, image) { }

		public StaticNavigationItemHeader([NotNull] string displayName, string info, Image image)
			: this(displayName, info, image, false) { }

		public StaticNavigationItemHeader(
			[NotNull] string displayName,
			string info,
			Image image,
			bool isHighlighted)
		{
			if (displayName == null)
				throw new ArgumentNullException("displayName");

			_displayName = displayName;
			_isHighlighted = isHighlighted;
			_image = image;
			_info = info;
		}

		#endregion

		#region Implementation of INavigationItemHeader

		public string DisplayName
		{
			get { return _displayName; }
		}

		public string Info
		{
			get { return _info; }
		}

		public Image Image
		{
			get { return _image; }
		}

		public bool IsHighlighted
		{
			get { return _isHighlighted; }
		}

		public IObservable<EventArgs> Changed
		{
			get { return Observable.Empty<EventArgs>(); }
		}

		#endregion
	}
}