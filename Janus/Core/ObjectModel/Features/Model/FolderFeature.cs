using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

using Rsdn.TreeGrid;

namespace Rsdn.Janus.ObjectModel
{
	/// <summary>
	/// Фича, у которой есть вложенные узлы.
	/// </summary>
	public abstract class FolderFeature : Feature, IFeature
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly List<IFeature> _features = new List<IFeature>(10);

		protected FolderFeature(IServiceProvider provider)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			_serviceProvider = provider;
		}

		/// <summary>
		/// Внутренний список с фичами
		/// </summary>
		protected IList<IFeature> Features
		{
			get { return _features; }
		}

		protected override Control CreateGuiControl()
		{
			return new FolderDummyForm(_serviceProvider, this);
		}

		#region IFeature implementation
		IFeature IFeature.this[int index]
		{
			get { return _features[index];}
		}
		#endregion

		#region ITreeNode implementation
		ITreeNode ITreeNode.this[int index]
		{
			get { return _features[index];}
		}

		bool ITreeNode.HasChildren
		{
			get { return _features.Count > 0; }
		}
		#endregion

		#region ICollection implementation
		int ICollection.Count
		{
			get { return _features.Count; }
		}

		void ICollection.CopyTo(Array array, int index)
		{
			_features.CopyTo((IFeature[])array, index);
		}
		#endregion

		#region IEnumerable implementation
		IEnumerator IEnumerable.GetEnumerator()
		{
			return _features.GetEnumerator();
		}
		#endregion
	}
}
