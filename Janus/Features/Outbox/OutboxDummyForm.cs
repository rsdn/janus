using System;
using System.Collections.Generic;
using System.Windows.Forms;

using CodeJam.Services;

using Rsdn.Janus.ObjectModel;
using Rsdn.Shortcuts;
using Rsdn.TreeGrid;

namespace Rsdn.Janus
{
	/// <summary>
	/// OutboxDummyForm - control for manage outgoing messages/rates.
	/// </summary>
	public sealed partial class OutboxDummyForm : UserControl, IOutboxForm, IFeatureView
	{
		#region Private Fields

		private readonly ServiceContainer _serviceManager;
		private StripMenuGenerator _contextMenuGenerator;
		private readonly OutboxManager _manager;

		#endregion

		#region Constructor(s) & Dispose

		public OutboxDummyForm(IServiceProvider provider, OutboxManager manager)
		{
			if (provider == null)
				throw new ArgumentNullException(nameof(provider));

			_serviceManager = new ServiceContainer(provider);
			_manager = manager;

			InitializeComponent();

			_serviceManager.Publish<IDefaultCommandService>(
				new DefaultCommandService("Janus.Outbox.EditItem"));

			MessageForm.MessageSent += MessageSent;

			_grid.Indent = Config.Instance.ForumDisplayConfig.GridIndent;
			_grid.SmallImageList = OutboxImageManager.ImageList;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_contextMenuGenerator?.Dispose();
				components?.Dispose();
			}

			base.Dispose(disposing);
		}

		#endregion

		#region IFeatureView

		void IFeatureView.Activate(IFeature feature)
		{
			RefreshList();
		}

		void IFeatureView.Refresh() { }

		void IFeatureView.ConfigChanged() { }

		#endregion IFeatureView

		#region Public Methods
		ICollection<ITreeNode> IOutboxForm.SelectedNodes => SelectedNodes;

		public List<ITreeNode> SelectedNodes => _grid.SelectedNodes;

		public event EventHandler SelectedNodesChanged;

		public void RefreshList()
		{
			if (InvokeRequired)
			{
				Invoke((Action)RefreshList);
				return;
			}

			using (new ActiveNodeHoldHelper(_grid))
				_grid.Nodes = _manager;
		}

		#endregion

		#region Protected Members

		protected override void OnLoad(EventArgs e)
		{
			_contextMenuGenerator = new StripMenuGenerator(_serviceManager, _contextMenuStrip, "Outbox.ContextMenu");
			base.OnLoad(e);
		}

		#endregion

		#region Private Methods

		private void OnSelectedNodesChanged(EventArgs e)
		{
			SelectedNodesChanged?.Invoke(this, e);
		}

		private void MessageSent(object sender, int mid)
		{
			if (Visible)
				RefreshList();
			//Удаляем делегат чтобы позволить GC убрать форму, иначе так и будет висеть в памяти
			//((WriteMessageForm) sender).OnSendMessage -= new SendMessageHandler(MessageSent);
		}

		private void GridSelectedIndexChanged(object sender, EventArgs e)
		{
			OnSelectedNodesChanged(EventArgs.Empty);
		}

		private void GridDoubleClick(object sender, EventArgs e)
		{
			_serviceManager.ExecuteDefaultCommand();
		}

		private void GridMouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && _grid.ActiveNode != null)
				_contextMenuStrip.Show(_grid, e.Location);
		}

		#endregion

		#region Shortcurts

		[MethodShortcut(Shortcut.CtrlE, "Редактировать", "Редактировать сообщение.")]
		private void Edit()
		{
			_serviceManager.TryExecuteCommand("Janus.Outbox.EditItem", new Dictionary<string, object>());
		}

		[MethodShortcut((Shortcut)Keys.Delete, "Удалить", "Удалить сообщение.")]
		private void Delete()
		{
			_serviceManager.TryExecuteCommand("Janus.Outbox.DeleteItem", new Dictionary<string, object>());

		}

		#endregion
	}
}