using System;
using System.Collections;
using System.Windows.Forms;

using Rsdn.TreeGrid;

namespace Rsdn.Janus.ObjectModel
{
	/// <summary>
	/// Возможностей (feature). Например, форумы, поиск, outbox...
	/// </summary>
	public abstract class Feature : IFeature, IGetData,
		IFeatureGui, IDisposable
	{
		protected Control GuiControl { get; private set; }

		#region IGetData
		void IGetData.GetData(NodeInfo NodeInfo, CellInfo[] aryCellData)
		{
			aryCellData[0].Text = Description;
			aryCellData[0].ImageIndex = ImageIndex;
			aryCellData[1].Text = Info;
		}
		#endregion IGetData

		#region IFeature
		internal ITreeNode _parent;
		internal string _description = string.Empty;
		internal NodeFlags _flags;
		// IFeature		
		public virtual string Description
		{
			get { return _description; }
		}

		public virtual String Info
		{
			get { return String.Empty; }
		}

		public virtual int ImageIndex
		{
			get { return -1; }
		}

		IFeature IFeature.this[int index]
		{
			get { throw new NotSupportedException("this[] не поддерживается."); }
		}

		public virtual string Key
		{
			get { return GetType().FullName; }
		}

		// ITreeNode
		ITreeNode ITreeNode.this[int id]
		{
			get { return null; }
		}

		ITreeNode ITreeNode.Parent
		{
			get { return _parent; }
		}

		NodeFlags ITreeNode.Flags
		{
			get { return _flags; }
			set { _flags = value; }
		}

		bool ITreeNode.HasChildren
		{
			get { return false; }
		}

		// ICollection
		int ICollection.Count
		{
			get { return 0; }
		}

		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		object ICollection.SyncRoot
		{
			get { return typeof (Forum); }
		}

		void ICollection.CopyTo(Array array, int index)
		{
			throw new NotSupportedException("Метод CopyTo не поддерживается.");
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (new Feature[0]).GetEnumerator();
		}
		#endregion IFeature

		#region IFeatureGui

		protected abstract Control CreateGuiControl();

		Control IFeatureGui.GuiControl
		{
			get
			{
				// Создаем форуму динамически, чтобы не занимать по напрасну ресурсы.
				if (GuiControl == null)
					lock (this) // Честно говоря перестраховка, но все же...
					{
						if (GuiControl == null)
							GuiControl = CreateGuiControl();
					}
				return GuiControl; // Возвращаем всегда однин и тот же экземпляр.
			}
		}

		void IFeatureGui.ConfigChanged()
		{
			if (GuiControl == null)
				return;
			var featureView = GuiControl as IFeatureView;
			if (featureView != null)
				featureView.ConfigChanged();
		}

		#endregion IFeatureGui

		#region IDisposable
		public virtual void Dispose()
		{
			if (GuiControl != null)
			{
				GuiControl.Dispose();
				GuiControl = null;
			}
		}
		#endregion IDisposable

		public override string ToString()
		{
			return Description;
		}
	}
}