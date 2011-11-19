using System;
using System.Collections;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	/// <summary>
	/// Summary description for ComponentCollectionEditorForm.
	/// </summary>
	internal partial class ComponentCollectionEditorForm : Form
	{
		private readonly Type _ItemType;
		private readonly ArrayList _WorkCollection;
		private IList _Collection;

		public ComponentCollectionEditorForm(IList coll)
		{
			InitializeComponent();

			_Collection = coll;
			_ItemType = CollectionItemType();
			var hpc = HasParamlessCtor(_ItemType);
			_AddItemButton.Enabled = hpc;
			_DelItemButton.Enabled = hpc;
			_WorkCollection = new ArrayList(_Collection);

			RefreshList();
		}

		/// <summary>
		/// Редактируемая коллекция
		/// </summary>
		public IList Collection
		{
			get { return _Collection; }
		}

		private static string GetItemText(object item)
		{
			var res = item.ToString();
			if (string.IsNullOrEmpty(res))
				res = "<элемент>";
			return res;
		}

		private void RefreshList()
		{
			try
			{
				_ItemsList.BeginUpdate();
				_ItemsList.Items.Clear();
				foreach (var item in _WorkCollection)
				{
					var lvi = _ItemsList.Items.Add(GetItemText(item));
					lvi.ImageIndex = 0;
					lvi.Tag = item;
				}
			}
			finally
			{
				_ItemsList.EndUpdate();
			}
		}

		private void AcceptChanges()
		{
			if (_Collection.GetType().IsArray)
				_Collection = _WorkCollection.ToArray(_ItemType);
			else
			{
				_Collection.Clear();
				foreach (var item in _WorkCollection)
					_Collection.Add(item);
			}
		}

		private Type CollectionItemType()
		{
			if (_Collection.GetType().IsArray)
				return _Collection.GetType().GetElementType();
			foreach (var pi in _Collection.GetType().GetProperties())
				if ((pi.GetIndexParameters().Length > 0) && (pi.PropertyType != typeof (object)))
					return pi.PropertyType;
			return typeof (object);
		}

		private static bool HasParamlessCtor(Type itemType)
		{
			if (itemType.GetConstructors().Length == 0)
				return true;
			return itemType.GetConstructor(new Type[] {}) != null;
		}

		private void AddItem()
		{
			var item = Activator.CreateInstance(_ItemType);
			_WorkCollection.Add(item);
			RefreshList();
		}

		private void DelItem()
		{
			if (_ItemsList.FocusedItem != null)
			{
				_WorkCollection.Remove(_ItemsList.FocusedItem.Tag);
				RefreshList();
			}
		}

		private void _OkButton_Click(object sender, EventArgs e)
		{
			AcceptChanges();
		}

		private void _AddItemButton_Click(object sender, EventArgs e)
		{
			AddItem();
		}

		private void _ItemPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			RefreshList();
		}

		private void _ItemsList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_ItemsList.FocusedItem != null)
				_ItemPropertyGrid.SelectedObject = new DisplayNameWrapper(_ItemsList.FocusedItem.Tag);
		}

		private void _DelItemButton_Click(object sender, EventArgs e)
		{
			DelItem();
		}
	}
}