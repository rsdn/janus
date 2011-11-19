using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Rsdn.TreeGrid;

namespace Rsdn.Janus.ObjectModel
{
	/// <summary>
	/// Фича поиска.
	/// </summary>
	public class SearchFeature : Feature, IGetData, IMessagesFeature
	{
		private readonly IServiceProvider _provider;
		private SearchDummyForm _searchForm;

		private SearchFeature(IServiceProvider provider)
		{
			_provider = provider;
			_description = SR.Search.DisplayName;
		}

		private static readonly SearchFeature _instance =
			new SearchFeature(ApplicationManager.Instance.ServiceProvider);

		public static SearchFeature Instance
		{
			get { return _instance; }
		}

		public override string Key { get { return "Search"; } }

		public SearchDummyForm SearchForm
		{
			get
			{
				if (_searchForm == null)
					_searchForm = new SearchDummyForm(_provider);
				return _searchForm;
			}
		}

		#region IFeatureGui

		protected override Control CreateGuiControl()
		{
			return SearchForm;
		}

		#endregion

		#region IGetData
		void IGetData.GetData(NodeInfo nodeInfo, CellInfo[] cellData)
		{
			cellData[0].Text = Description;
			cellData[0].ImageIndex = Features.Instance.SearchImageIndex;
			cellData[1].Text = Info;
		}
		#endregion IGetData

		#region Implementation of IMessagesFeature

		public IEnumerable<IMsg> ActiveMessages
		{
			get { return SearchForm.SelectedMessages; }
		}

		event EventHandler IMessagesFeature.ActiveMessagesChanged
		{
			add { SearchForm.SelectedMessagesChanged += value; }
			remove { SearchForm.SelectedMessagesChanged -= value; }
		}

		#endregion
	}
}
