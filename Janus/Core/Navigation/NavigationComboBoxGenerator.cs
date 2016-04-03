using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using CodeJam.Extensibility;

using JetBrains.Annotations;

using Rsdn.Janus.ObjectModel;
using Rsdn.TreeGrid;

namespace Rsdn.Janus
{
	public sealed class NavigationComboBoxGenerator : IDisposable
	{
		private const int _leftMargin = 2;

		private readonly ComboBox _comboBox;
		private readonly AsyncOperation _uiAsyncOperation;
		private bool _suspendSelectionTracking;

		public NavigationComboBoxGenerator(
			[NotNull] IServiceProvider serviceProvider, 
			[NotNull] ComboBox comboBox)
		{
			if (serviceProvider == null) 
				throw new ArgumentNullException(nameof(serviceProvider));
			if (comboBox == null)
				throw new ArgumentNullException(nameof(comboBox));

			_comboBox = comboBox;
			_uiAsyncOperation = serviceProvider.GetRequiredService<IUIShell>().CreateUIAsyncOperation();
			_comboBox.DrawMode = DrawMode.OwnerDrawFixed;
			_comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			_comboBox.DrawItem += ComboBoxDrawItem;
			_comboBox.SelectedIndexChanged += ComboBoxSelectedIndexChanged;

			UpdateContent();

			Features.Instance.AfterFeatureActivate += Instance_AfterFeatureActivate;
			Features.Instance.AfterFeatureChanged += Instance_AfterFeatureChanged;

			Instance_AfterFeatureActivate(null, Features.Instance.ActiveFeature);
		}

		public void Dispose()
		{
			_comboBox.DrawItem -= ComboBoxDrawItem;
			_comboBox.SelectedIndexChanged -= ComboBoxSelectedIndexChanged;
		}

		private void ComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_suspendSelectionTracking)
				Features.Instance.ActiveFeature =
					((FeatureContainer)_comboBox.SelectedItem).Feature;
		}

		private void ComboBoxDrawItem(object sender, DrawItemEventArgs e)
		{
			e.DrawBackground();

			if (e.Index < 0)
				return;

			var bounds = e.Bounds;
			var cont = (FeatureContainer)_comboBox.Items[e.Index];

			bounds.X += _leftMargin + cont.Level * Config.Instance.ForumDisplayConfig.GridIndent;

			var cd = new CellInfo[5];
			var ni = new NodeInfo(_comboBox.ForeColor, _comboBox.BackColor, _comboBox.Font, false);
			((IGetData)cont.Feature).GetData(ni, cd);

			if (cd[0].Image != null)
			{
				e.Graphics.DrawImage(
					cd[0].Image,
					bounds.X,
					bounds.Y,
					cd[0].Image.Width,
					cd[0].Image.Height);

				bounds.X += cd[0].Image.Width;
			}

			var brush =
				(e.State & DrawItemState.Selected) == 0
					? _comboBox.DroppedDown
						? new SolidBrush(ni.ForeColor)
						: SystemBrushes.ControlText
					: SystemBrushes.HighlightText;

			e.Graphics.DrawString(
				cont.Feature.ToString(),
				ni.Highlight ? new Font(_comboBox.Font, FontStyle.Bold) : _comboBox.Font,
				brush, 
				bounds);
		}

		private void Instance_AfterFeatureActivate(IFeature oldFeature, IFeature newFeature)
		{
			_suspendSelectionTracking = true;
			try
			{
				_comboBox.SelectedItem = FindContainer(newFeature);
			}
			finally
			{
				_suspendSelectionTracking = false;
			}
		}

		private void Instance_AfterFeatureChanged(IFeature changedFeature)
		{
			_uiAsyncOperation.Post(_comboBox.Invalidate);
		}

		private void UpdateContent()
		{
			_comboBox.BeginUpdate();
			try
			{
				IFeature feature = null;
				if (_comboBox.SelectedItem != null)
					feature = ((FeatureContainer)_comboBox.SelectedItem).Feature;

				_comboBox.Items.Clear();
				FillFeatures(Features.Instance, 0);

				if (feature != null)
					_comboBox.SelectedItem = FindContainer(feature);
			}
			finally
			{
				_comboBox.EndUpdate();
			}
		}

		private void FillFeatures(ITreeNode feature, int level)
		{
			foreach (IFeature f in feature)
			{
				_comboBox.Items.Add(new FeatureContainer(f, level));

				if (f.HasChildren)
					FillFeatures(f, level + 1);
			}
		}

		private FeatureContainer FindContainer(IFeature feature)
		{
			return 
				feature == null
					? null
					: _comboBox
						.Items
						.Cast<FeatureContainer>()
						.FirstOrDefault(container => container.Feature.Key == feature.Key);
		}

		#region Nested type: FeatureContainer
		private class FeatureContainer
		{
			private readonly IFeature _feature;

			public FeatureContainer(IFeature feature, int level)
			{
				_feature = feature;
				Level = level;
			}

			public int Level { get; }

			public IFeature Feature => _feature;

			public override string ToString()
			{
				var str = _feature.ToString();
#if DEBUG
				if (str == null)
					throw new Exception("<Фигово заполняете строки>");
#endif //DEBUG
				return str;
			}
		}
		#endregion
	}
}