using System;
using System.Globalization;
using System.IO;
using System.Resources;

using Rsdn.TreeGrid;

namespace Rsdn.LocUtil.Model
{
	/// <summary>
	/// Коневая категория.
	/// </summary>
	public class RootCategory : Category, ITreeNode
	{
		private readonly string _treeName;
		private ITreeNode _parent;

		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		public RootCategory(string treeName)
			: base(null, null)
		{
			_treeName = treeName;
		}

		/// <summary>
		/// Имя дерева.
		/// </summary>
		public string TreeName
		{
			get { return _treeName; }
		}

		/// <summary>
		/// Родитель для отображения в дереве.
		/// </summary>
		public ITreeNode TreeParent
		{
			get { return _parent; }
			set { _parent = value; }
		}

		/// <summary>
		/// Получить элемент по полному имени.
		/// </summary>
		public ResourceItem GetItem(string itemName)
		{
			string[] parts = itemName.Split('.');
			string rin = parts[parts.Length - 1];
			string[] fcn = new string[parts.Length - 1];
			Array.Copy(parts, fcn, fcn.Length);
			Category cat = FindCategory(this, fcn);
			ResourceItem item = null;
			foreach (ResourceItem ri in cat.ResourceItems)
				if (ri.ShortName == rin)
				{
					item = ri;
					break;
				}
			if (item == null)
			{
				item = new ResourceItem(itemName, cat);
				cat.ResourceItems.Add(item);
			}
			return item;
		}

		/// <summary>
		/// Сохранить дерево ресурсов в каталог.
		/// </summary>
		public void Save(string directory)
		{
			ResourceItem[] ris = GetAllItems();
			foreach (CultureInfo ci in GetCultures())
			{
				using (ResXResourceWriter rxrw = new ResXResourceWriter(
					Path.Combine(directory, TreeName
						+ (ci == CultureInfo.InvariantCulture ? "" : "." + ci.TwoLetterISOLanguageName)
						+ ".resx")))
				{
					foreach (ResourceItem ri in ris)
						if (ri.ValueCollection[ci] != null)
							rxrw.AddResource(ri.Name, ri.ValueCollection[ci]);
					rxrw.Generate();
				}
			}
		}

		private Category FindCategory(Category startCategory, string[] parts)
		{
			if (parts.Length == 0)
				return startCategory;
			Category ic = null;
			foreach (Category c in startCategory.Categories)
				if (c.ShortName == parts[0])
				{
					ic = c;
					break;
				}
			if (ic == null)
			{
				string prefix = startCategory.Name != null ? startCategory.Name + '.' : "";
				ic = new Category(prefix + parts[0], startCategory);
				startCategory.Categories.Add(ic);
			}
			string[] sa = new string[parts.Length - 1];
			Array.Copy(parts, 1, sa, 0, sa.Length);
			return FindCategory(ic, sa);
		}

		ITreeNode ITreeNode.Parent
		{
			get { return TreeParent; }
		}
	}
}
