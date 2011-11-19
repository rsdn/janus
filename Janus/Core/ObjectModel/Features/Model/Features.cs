using System;
using System.Collections;
using Rsdn.TreeGrid;
using System.Collections.Generic;

namespace Rsdn.Janus.ObjectModel
{
	/// <summary>
	/// Список возможностей. Например, форумы, поиск, outbox...
	/// </summary>
	public sealed class Features : ITreeNode, IDisposable
	{
		#region Constructor(s) & Реализация Singleton 

		private Features()
		{
		}

		private static readonly Features _instance = new Features();

		public static Features Instance
		{
			get { return _instance; }
		}

		#endregion

		public void Init()
		{
			Add(new OutboxFeature());
			Add(Forums.Instance);
			Add(SearchFeature.Instance);
			Add(FavoritesFeature.Instance);
		}

		public void Refresh()
		{
			_activeFeature = null;
		}

		private IFeature _oldFeature;

		private IFeature _activeFeature;

		public IFeature ActiveFeature
		{
			get
			{
				if (_activeFeature == null)
					lock (typeof (Features))
						if (_activeFeature == null)
						{
							var key = Config.Instance.ActiveFeature;
							if (key.Length != 0)
								return FindFeatureByKey(key);
						}
				return _activeFeature;
			}
			set
			{
				lock (typeof (Features))
				{
					if (_activeFeature == value)
						return;
					if (value == null)
					{
						_activeFeature = null;
						Config.Instance.ActiveFeature = string.Empty;
						return;
					}

					if (!FeatureExists(_features, value))
					{
						value = FindFeatureByKey(value.Key);
						if (value == null)
							throw new ApplicationException(
								"Feature '" + value + "' отсутствует в коллекции feature. (v.2)");
					}

					var cancel = OnBeforeFeatureActivate(_activeFeature, value);
					if (!cancel)
					{
						_activeFeature = value;
						Config.Instance.ActiveFeature = value.Key;
						OnAfterFeatureActivate(_oldFeature, value);
						// Prevent loss of old feature while refresh
						_oldFeature = _activeFeature;
					}
				}
			}
		}

		internal void FeatureChanged(IFeature changedFeature)
		{
#if DEBUG
			//if(!FeatureExists(_features, changedFeature))
			//	throw new ApplicationException("Feature '" + changedFeature
			//		+ "' отсутствует в коллекции feature.");
#endif //DEBUG
			OnAfterFeatureChanged(changedFeature);
		}

		public IFeature[] GetAllFeatures()
		{
			var list = new List<IFeature>(100);
			GetAllFeatures(_features, list);
			return list.ToArray();
		}

		private static void GetAllFeatures(ICollection features, IList list)
		{
			foreach (IFeature feature in features)
			{
				list.Add(feature);
				GetAllFeatures(feature, list);
			}
		}


		private static bool FeatureExists(ICollection features, IFeature lookedFor)
		{
			foreach (IFeature feature in features)
			{
				if (feature == lookedFor)
					return true;
				if (FeatureExists(feature, lookedFor))
					return true;
			}
			return false;
		}

		public IFeature FindFeatureByKey(string key)
		{
			var feature = FindFeatureByKey(_features, key);
			return feature;
		}

		private static IFeature FindFeatureByKey(ICollection features, string key)
		{
			foreach (IFeature feature in features)
			{
				if (feature.Key == key)
					return feature;
				var ret = FindFeatureByKey(feature, key);
				if (ret != null)
					return ret;
			}
			return null;
		}


		public void ConfigChanged()
		{
			foreach (IFeature feature in _features)
				FeatureConfigChanged(feature);
		}

		private static void FeatureConfigChanged(IFeature feature)
		{
			foreach (IFeature subFeature in feature)
				FeatureConfigChanged(subFeature);
			var featureGui = feature as IFeatureGui;
			if (featureGui != null)
				featureGui.ConfigChanged();
		}


		// IDisposable
		public void Dispose()
		{
			foreach (IFeature feature in _features)
				DisposeFeature(feature);
		}

		private static void DisposeFeature(IFeature feature)
		{
			foreach (IFeature subFeature in feature)
				DisposeFeature(subFeature);
			var disposable = feature as IDisposable;
			if (disposable != null)
				disposable.Dispose();
		}

		private IFeature this[int index]
		{
			get { return (IFeature) _features[index]; }
		}

		ITreeNode ITreeNode.this[int id]
		{
			get { return this[id]; }
		}

		NodeFlags ITreeNode.Flags { get; set; }

		bool ITreeNode.HasChildren
		{
			get { return _features.Count > 0; }
		}

		ITreeNode ITreeNode.Parent
		{
			get { return null; }
		}

		private void Add(IFeature feature)
		{
			_features.Add(feature);
			((Feature) feature)._parent = this;
		}

		// ICollection
		public int Count
		{
			get { return _features.Count; }
		}

		public bool IsSynchronized
		{
			get { return _features.IsSynchronized; }
		}

		public object SyncRoot
		{
			get { return _features.SyncRoot; }
		}

		public void CopyTo(Array array, int index)
		{
			_features.CopyTo(array, index);
		}

		public IEnumerator GetEnumerator()
		{
			return _features.GetEnumerator();
		}

		#region События.

		public delegate void BeforeFeatureActivateHandler(
			IFeature oldFeature, IFeature newFeature, ref bool cancel);

		public delegate void AfterFeatureActivateHandler(IFeature oldFeature, IFeature newFeature);

		///<summary>Вызывается перед изменением активной фичи.</summary>
		/// <returns>если установить в true активация была отклонена.</returns>
		public event BeforeFeatureActivateHandler BeforeFeatureActivate;

		///<summary>Вызывается после изменения содержимого одного из форумов.</summary>
		public event AfterFeatureActivateHandler AfterFeatureActivate;


		///<summary>Хэлпер для вызова события BeforeFeatureActivate.</summary>
		/// <param name="oldFeature">Старая активная фича.</param>
		/// <param name="newFeature">Активизируемая фича.</param>
		/// <returns>true - если активация была отклонена.</returns>
		private bool OnBeforeFeatureActivate(IFeature oldFeature, IFeature newFeature)
		{
			var cancel = false;
			if (BeforeFeatureActivate != null)
				BeforeFeatureActivate(oldFeature, newFeature, ref cancel);
			return cancel;
		}

		///<summary>Хэлпер для вызова события AfterFeatureActivate.</summary>
		/// <param name="oldFeature">Старая активная фича.</param>
		/// <param name="newFeature">Активируемая фича.</param>
		private void OnAfterFeatureActivate(IFeature oldFeature, IFeature newFeature)
		{
			if (AfterFeatureActivate != null)
				AfterFeatureActivate(oldFeature, newFeature);
		}

		public delegate void FeatureChangedHandler(IFeature changedFeature);

		///<summary>Вызывается перед изменением активной фичи.</summary>
		public event FeatureChangedHandler AfterFeatureChanged;

		private void OnAfterFeatureChanged(IFeature changedFeature)
		{
			if (AfterFeatureChanged != null)
				AfterFeatureChanged(changedFeature);
		}

		#endregion События.

		private readonly ArrayList _features = new ArrayList(20);

		#region Features image indexes

		public int OutboxImageIndex { get; set; }
		public int ForumsImageIndex { get; set; }
		public int FavoritesImageIndex { get; set; }
		public int SearchImageIndex { get; set; }
		public int UnsubscribedFolderImageIndex { get; set; }

		#endregion
	}
}