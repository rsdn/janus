using System;
using System.Drawing;
using System.Reactive.Disposables;
using System.Windows.Forms;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public static class WinFormsHelper
	{
		public static int AddImage([NotNull] this ImageList imageList, [NotNull] Image image)
		{
			if (imageList == null)
				throw new ArgumentNullException("imageList");
			if (image == null)
				throw new ArgumentNullException("image");

			return imageList.Images.Add(image, Color.Transparent);
		}

		public static CheckState Combine(this CheckState checkState1, CheckState checkState2)
		{
			if (checkState1 == CheckState.Checked && checkState2 == CheckState.Checked)
				return CheckState.Checked;
			if (checkState1 == CheckState.Unchecked && checkState2 == CheckState.Unchecked)
				return CheckState.Unchecked;
			return CheckState.Indeterminate;
		}

		public static Icon ToIcon([NotNull] this Image image)
		{
			if (image == null)
				throw new ArgumentNullException("image");

			return Icon.FromHandle(
				(image as Bitmap ?? new Bitmap(image, SystemInformation.IconSize)).GetHicon());
		}

		public static IDisposable UpdateScope([NotNull] this ListView listView)
		{
			return UpdateScope(listView, false);
		}

		public static IDisposable UpdateScope([NotNull] this ListView listView, bool clear)
		{
			if (listView == null) throw new ArgumentNullException("listView");
			listView.BeginUpdate();
			if (clear)
				listView.Items.Clear();
			return Disposable.Create(listView.EndUpdate);
		}

		public static IDisposable UpdateScope([NotNull] this TreeView treeView)
		{
			return UpdateScope(treeView, false);
		}

		public static IDisposable UpdateScope([NotNull] this TreeView treeView, bool clear)
		{
			if (treeView == null) throw new ArgumentNullException("treeView");
			treeView.BeginUpdate();
			if (clear)
				treeView.Nodes.Clear();
			return Disposable.Create(treeView.EndUpdate);
		}
	}
}
