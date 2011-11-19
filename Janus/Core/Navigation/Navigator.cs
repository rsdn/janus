using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using JetBrains.Annotations;

using Rsdn.Janus.ObjectModel;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Осуществляет навигацию по возможностям программы.
	/// </summary>
	internal class Navigator : IInitable
	{
		private readonly NavigationDummyForm _form;
		private readonly ImageList _featureImages = new ImageList();
		private readonly IServiceProvider _serviceProvider;
		private readonly AsyncOperation _uiAsyncOperation;

		internal Navigator([NotNull] IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException("serviceProvider");

			_serviceProvider = serviceProvider;

			_uiAsyncOperation = _serviceProvider.GetRequiredService<IUIShell>().CreateUIAsyncOperation();
			_form = new NavigationDummyForm(_serviceProvider);
			_serviceProvider.GetRequiredService<DockManager>().RegisterPersistablePane(_form);
		}

		public void RefreshData()
		{
			_form.RefreshTree();
			var cdf = ApplicationManager.Instance.ServiceProvider
				.GetRequiredService<DockManager>()
				.QueryExistingContentPane();

			if (cdf != null)
				if (Features.Instance.ActiveFeature == null)
					Features.Instance.ActiveFeature = Forums.Instance;
				else
				{
					cdf.Text = Features.Instance.ActiveFeature.ToString();
					var fv = cdf.AssociatedControl as IFeatureView;

					if (fv != null)
						fv.Refresh();
				}

			_features = Features.Instance.GetAllFeatures();
		}

		#region IInitable Members

		public void Init()
		{
			InitFeatureImageList();

			RefreshData();

			Features.Instance.AfterFeatureActivate += FeaturesAfterFeatureActivate;
			Features.Instance.AfterFeatureChanged += FeaturesAfterFeatureChanged;

			Features.Instance.ActiveFeature = Features.Instance.FindFeatureByKey(Config.Instance.ActiveFeature);
		}

		#endregion

		private static void FeaturesAfterFeatureActivate(IFeature oldFeature, IFeature newFeature)
		{
			var cdf = ApplicationManager.Instance.ServiceProvider
				.GetRequiredService<DockManager>()
				.QueryContentPane(/*(Control.ModifierKeys & Keys.Shift) == 0*/true);
			cdf.Text = newFeature.ToString();

			var fg = newFeature as IFeatureGui;
			if (fg == null)
			{
				cdf.AssociatedControl = null;
				return;
			}

			var nc = fg.GuiControl;
			// fix from michus
			var fv = nc as IFeatureView;
			if (fv != null)
				fv.Activate(newFeature);

			if ((cdf.AssociatedControl == null) || (nc != cdf.AssociatedControl))
				cdf.AssociatedControl = nc;
		}

		private void FeaturesAfterFeatureChanged(IFeature changedFeature)
		{
			_uiAsyncOperation.Post(
				() =>
				{
					_form.Refresh();
					var cdf = ApplicationManager.Instance.ServiceProvider
						.GetRequiredService<DockManager>()
						.QueryExistingContentPane();
					if (cdf != null)
						cdf.Text = Features.Instance.ActiveFeature.ToString();
				});
		}

		private void InitFeatureImageList()
		{
			const string prefix = @"NavTree\";

			_featureImages.ColorDepth = ColorDepth.Depth32Bit;

			var styleImageManager = _serviceProvider.GetService<IStyleImageManager>();

			Features.Instance.OutboxImageIndex =
				styleImageManager.AppendImage(prefix + "Outcomings", StyleImageType.ConstSize, _featureImages);
			Features.Instance.ForumsImageIndex =
				styleImageManager.AppendImage(prefix + "Incomings", StyleImageType.ConstSize, _featureImages);
			Features.Instance.FavoritesImageIndex =
				styleImageManager.AppendImage(prefix + "Favorites", StyleImageType.ConstSize, _featureImages);
			Features.Instance.SearchImageIndex =
				styleImageManager.AppendImage(prefix + "Search", StyleImageType.ConstSize, _featureImages);
			Features.Instance.UnsubscribedFolderImageIndex =
				styleImageManager.AppendImage(prefix + "Unsubscribed", StyleImageType.ConstSize, _featureImages);
			_form.FeatureImages = _featureImages;
		}

		#region Select Next/Previous

		private IFeature[] _features = new IFeature[0];

		public void SelectNext()
		{
			var index = Array.IndexOf(_features,
				Features.Instance.ActiveFeature);

			if (index != -1 && index + 1 < _features.Length)
				Features.Instance.ActiveFeature = _features[index + 1];
		}

		public void SelectPrevious()
		{
			var index = Array.IndexOf(_features,
				Features.Instance.ActiveFeature);

			if (index > 0)
				Features.Instance.ActiveFeature = _features[index - 1];
		}

		/// <summary>
		/// Поиск форума с непрочитанными сообщениями, 
		/// начиная от текущего, включительно.
		/// </summary>
		/// <returns><see cref="Forum"/> с непрочитанными сообщениями,
		/// иначе - <c>null</c>.</returns>
		public static Forum FindNextUnreadForum()
		{
			var list =
				Forums
					.Instance
					.ForumList
					.Concat(
						Forums
							.Instance
							.UnsubscribedForums
							.OfType<Forum>());
			var active = Forums.Instance.ActiveForum ?? list.FirstOrDefault();

			return
				list
					.SkipWhile(f => f != active)
					.FirstOrDefault(f => f.Unread != 0)
				?? list.TakeWhile(f => f != active).FirstOrDefault(f => f.Unread != 0);
		}

		#endregion
	}
}
