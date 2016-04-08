using System;
using System.Linq;
using System.Windows.Forms;

using CodeJam;
using CodeJam.Services;

using Rsdn.Janus.ObjectModel;

using Disposable = System.Reactive.Disposables.Disposable;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обработчик команд Favorites.
	/// </summary>
	[CommandTarget]
	internal sealed class FavoritesCommandTarget : CommandTarget
	{
		private readonly IFavoritesManager _favManager;

		public FavoritesCommandTarget(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_favManager = serviceProvider.GetRequiredService<IFavoritesManager>();
		}

		[CommandExecutor("Janus.Favorites.CreateFolder")]
		public void ExecuteCreateFolder(ICommandContext context)
		{
			using (var pb = new FavoritesFolderForm(string.Empty, string.Empty, true))
				if (pb.ShowDialog(
						context
							.GetRequiredService<IUIShell>()
							.GetMainWindowParent()) == DialogResult.OK)
				{
					var favFormSvc = context.GetRequiredService<IFavoritesDummyFormService>();
					_favManager.AddFolder(
						pb.FolderName,
						pb.FolderComment,
						favFormSvc.SelectedEntries.Any() && !pb.CreateAsRoot
							? (FavoritesFolder)favFormSvc.SelectedEntries.Single()
							: _favManager.RootFolder);
					favFormSvc.Refresh();
				}
		}

		[CommandExecutor("Janus.Favorites.RenameFolder")]
		public void ExecuteRenameFolder(ICommandContext context)
		{
			var favFormSvc = context.GetRequiredService<IFavoritesDummyFormService>();
			var activeFolder = (FavoritesFolder)favFormSvc.SelectedEntries.Single();
			using (var pb = new FavoritesFolderForm(activeFolder.Name, activeFolder.Comment))
				if (pb.ShowDialog(
						context.
							GetRequiredService<IUIShell>().
							GetMainWindowParent()) == DialogResult.OK)
				{
					activeFolder.Name = pb.FolderName;
					activeFolder.Comment = pb.FolderComment;
					activeFolder.Update();
					favFormSvc.Refresh();
				}
		}

		[CommandExecutor("Janus.Favorites.AddLink")]
		public void ExecuteAddLink(ICommandContext context)
		{
			var favFormSvc = context.GetRequiredService<IFavoritesDummyFormService>();
			using (var fi = new FavoritesLinkForm())
				if (fi.ShowDialog(
						context
							.GetRequiredService<IUIShell>()
							.GetMainWindowParent()) == DialogResult.OK)
				{
					_favManager.AddUrlLink(
						fi.Url,
						fi.Comment,
						favFormSvc.SelectedEntries.Any()
							? (FavoritesFolder)favFormSvc.SelectedEntries.Single()
							: _favManager.RootFolder);
					favFormSvc.Refresh();
				}
		}

		[CommandExecutor("Janus.Favorites.OpenLink")]
		public void ExecuteOpenLink(ICommandContext context)
		{
			var link = (FavoritesLink)context
				.GetRequiredService<IFavoritesDummyFormService>()
				.SelectedEntries.Single();

			if (string.IsNullOrEmpty(link.Url))
				ApplicationManager.Instance.ForumNavigator.SelectMessage(link.MessageId);
			else
				context.OpenUrlInBrowser(link.Link);
		}

		[CommandExecutor("Janus.Favorites.EditLink")]
		public void ExecuteEditLink(ICommandContext context)
		{
			var favFrmSvc = context.GetRequiredService<IFavoritesDummyFormService>();
			var activeLink = (FavoritesLink)favFrmSvc.SelectedEntries.Single();
			using (var fi = new FavoritesLinkForm(activeLink.Link, activeLink.Comment))
				if (fi.ShowDialog(
						context.
							GetRequiredService<IUIShell>().
							GetMainWindowParent()) == DialogResult.OK)
				{
					activeLink.Link = fi.Url;
					activeLink.Comment = fi.Comment;
					activeLink.Update();
					favFrmSvc.Refresh();
				}
		}

		[CommandExecutor("Janus.Favorites.MoveItem")]
		public void ExecuteMoveItem(ICommandContext context)
		{
			var favFormSvc = context.GetRequiredService<IFavoritesDummyFormService>();
			using (var selForm =
				new FavoritesSelectFolderForm(
					context,
					_favManager.RootFolder,
					false))
			{
				var windowParent = context.GetRequiredService<IUIShell>().GetMainWindowParent();

				if (selForm.ShowDialog(windowParent) != DialogResult.OK)
					return;

				_favManager.RootFolder.ShowLinks = true;

				if (selForm.SelectedFolder == null)
					return;

				var notMoved = false;
				foreach (var entry in favFormSvc.SelectedEntries)
					notMoved = !entry.Move(selForm.SelectedFolder);

				favFormSvc.LastActiveNode = null;
				favFormSvc.Refresh();

				if (notMoved)
					MessageBox.Show(
						windowParent,
						SR.Favorites.ElementExists.FormatWith(),
						ApplicationInfo.ApplicationName,
						MessageBoxButtons.OK,
						MessageBoxIcon.Information);
			}
		}

		[CommandExecutor("Janus.Favorites.DeleteItem")]
		public void ExecuteDeleteItem(ICommandContext context)
		{
			var favFormSvc = context.GetRequiredService<IFavoritesDummyFormService>();
			if (favFormSvc.SelectedEntries.Any())
				if (MessageBox.Show(
						context.GetRequiredService<IUIShell>().GetMainWindowParent(),
						SR.Favorites.DeleteItemPrompt,
						ApplicationInfo.ApplicationName,
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Warning) == DialogResult.Yes)
				{
					foreach (var entry in favFormSvc.SelectedEntries)
						entry.Delete();

					favFormSvc.LastActiveNode = null;
					favFormSvc.Refresh();
				}
		}

		[CommandStatusGetter("Janus.Favorites.OpenLink")]
		[CommandStatusGetter("Janus.Favorites.EditLink")]
		public CommandStatus QueryLinkCommandStatus(ICommandContext context)
		{
			return QueryFavoritesItemCommandStatus(context).UnavailableIfNot(
				() => context
					.GetRequiredService<IFavoritesDummyFormService>()
					.SelectedEntries.Single() is FavoritesLink);
		}

		[CommandStatusGetter("Janus.Favorites.RenameFolder")]
		public CommandStatus QueryFolderCommandStatus(ICommandContext context)
		{
			return QueryFavoritesItemCommandStatus(context).UnavailableIfNot(
				() => context
					.GetRequiredService<IFavoritesDummyFormService>()
					.SelectedEntries.Single() is FavoritesFolder);
		}

		private static CommandStatus QueryFavoritesItemCommandStatus(IServiceProvider context)
		{
			return QueryFavoritesCommandStatus(context).UnavailableIfNot(
				() => context
					.GetRequiredService<IFavoritesDummyFormService>()
					.SelectedEntries
					.IsSingle());
		}

		[CommandStatusGetter("Janus.Favorites.MoveItem")]
		[CommandStatusGetter("Janus.Favorites.DeleteItem")]
		public CommandStatus QueryFavoritesItemsCommandStatus(ICommandContext context)
		{
			return QueryFavoritesCommandStatus(context).UnavailableIfNot(
				() => context
					.GetRequiredService<IFavoritesDummyFormService>()
					.SelectedEntries
					.Any());
		}

		[CommandStatusGetter("Janus.Favorites.AddLink")]
		[CommandStatusGetter("Janus.Favorites.CreateFolder")]
		public CommandStatus QueryCreationCommandStatus(ICommandContext context)
		{
			return QueryFavoritesCommandStatus(context).UnavailableIfNot(
				() =>
				{
					var selectedEntries = context
						.GetRequiredService<IFavoritesDummyFormService>()
						.SelectedEntries;
					return !selectedEntries.Any()
						|| (selectedEntries.IsSingle() && selectedEntries.Single() is FavoritesFolder);
				});
		}

		private static CommandStatus QueryFavoritesCommandStatus(IServiceProvider context)
		{
			return Features.Instance.ActiveFeature is FavoritesFeature
							&& context.GetService<IFavoritesDummyFormService>() != null
						? CommandStatus.Normal
						: CommandStatus.Unavailable;
		}

		[CommandStatusSubscriber("Janus.Favorites.AddLink")]
		[CommandStatusSubscriber("Janus.Favorites.CreateFolder")]
		[CommandStatusSubscriber("Janus.Favorites.MoveItem")]
		[CommandStatusSubscriber("Janus.Favorites.DeleteItem")]
		[CommandStatusSubscriber("Janus.Favorites.RenameFolder")]
		[CommandStatusSubscriber("Janus.Favorites.OpenLink")]
		[CommandStatusSubscriber("Janus.Favorites.EditLink")]
		public IDisposable SubscribeFavoritesItemCommandStatusChanged(
			IServiceProvider provider, Action handler)
		{
			var favoritesSvc = provider.GetService<IFavoritesDummyFormService>();
			if(favoritesSvc!=null)
			{
				EventHandler statusUpdater = (sender, e) => handler();
				favoritesSvc.SelectedEntriesChanged += statusUpdater;
				return Disposable.Create(
					() => favoritesSvc.SelectedEntriesChanged -= statusUpdater);
			}
			return Disposable.Empty;
		}
	}
}