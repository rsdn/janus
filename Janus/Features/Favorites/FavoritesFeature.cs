using System.Windows.Forms;

using Rsdn.TreeGrid;

namespace Rsdn.Janus.ObjectModel
{
	/// <summary>
	/// Подсистема «Избранное»
	/// </summary>
	public class FavoritesFeature : Feature, IGetData
	{
		#region Constructor(s) & Реализация Singleton 

		private FavoritesFeature()
		{
			_description = SR.Favorites.DisplayName;
		}

		private static FavoritesFeature _instance;

		public static FavoritesFeature Instance
		{
			get
			{
				if (_instance == null)
					lock (typeof(Forums))
						if (_instance == null)
							_instance = new FavoritesFeature();
				return _instance;
			}
		}

		#endregion

		#region Members

		public override string Key
		{
			get { return "Favorites"; }
		}

		protected override Control CreateGuiControl()
		{
			return FavoritesDummyForm.Instance;
		}

		#endregion

		#region IGetData

		void IGetData.GetData(NodeInfo nodeInfo, CellInfo[] cellData)
		{
			cellData[0].Text = Description;
			cellData[0].ImageIndex = Features.Instance.FavoritesImageIndex;
		}

		#endregion
	}
}