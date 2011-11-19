using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Lucene.Net.Index;

using Rsdn.Janus.Log;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	public partial class BuildIndexForm : Form
	{
		private readonly IServiceProvider _provider;
		private const int _messagesBatchSize = 512;

		public BuildIndexForm(IServiceProvider provider)
		{
			_provider = provider;
			InitializeComponent();
			CustomInitializeComponent();
		}

		private void CustomInitializeComponent()
		{
			Text                  = SR.BuildIndexForm.Caption;
			progressLabel.Text    = @"...";
			actionTextBox.Text    = SR.BuildIndexForm.Action.WelcomeText;
			cleanButton.Text      = SR.BuildIndexForm.CleanText;
			startButton.Text      = SR.BuildIndexForm.StartText;
			stopButton.Text       = SR.BuildIndexForm.StopText;
			closeButton.Text      = SR.BuildIndexForm.CloseText;
			optimizeCheckBox.Text = SR.BuildIndexForm.OptimizeIndexText;
			progressBar.Maximum   = 100;
			progressBar.Minimum   = 0;
			progressBar.Value     = 0;
			stopButton.Enabled    = false;
		}

		private void CleanButtonClick(object sender, EventArgs e)
		{
			var r = MessageBox.Show(
				SR.BuildIndexForm.CleanAskText,
				SR.BuildIndexForm.CleanAskCaptionText,
				MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (r == DialogResult.Yes)
			{
				var s = LocalUser.LuceneIndexPath;

				if (Directory.Exists(s))
					Directory.Delete(s, true);

				_provider.LogInfo(SR.Search.IndexCleared);
			}
		}

		private void StartButtonClick(object sender, EventArgs e)
		{
			stopButton.Enabled       = true;
			cleanButton.Enabled      = false;
			startButton.Enabled      = false;
			closeButton.Enabled      = false;
			optimizeCheckBox.Enabled = false;

			_provider.LogInfo(SR.Search.IndexingStarted);

			var args  = new DoWorkArgs(optimizeCheckBox.Checked);
			actionTextBox.Text = StageToString(args.Stage);

			indexBackgroundWorker.RunWorkerAsync(args);
		}

		private void StopButtonClick(object sender, EventArgs e)
		{
			stopButton.Enabled = false;
			indexBackgroundWorker.CancelAsync();
		}

		// TODO: Это бизнеслогика в чистом виде. Нужно выносить в отдельный класс.
		private void IndexBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
		{
			var args = (DoWorkArgs)e.Argument;

			args.Stage = DoWorkStage.Preparing;
			args.ProcessedMessages = 0;
			var indexedCount = 0;
			int maxMidInDb;
			int minMidInDb;
			using (var db = _provider.CreateDBContext())
			{
				args.TotalMessages = db.Messages().Count();
				minMidInDb = db.Messages().Min(m => m.ID);
				maxMidInDb = db.Messages().Max(m => m.ID);
			}

			var writer = SearchHelper.CreateIndexWriter();
			try
			{
				// Направление выборки - от свежих к ранним
				var lastMid = maxMidInDb;

				var baseTerm = new Term("mid");
				var bw = (BackgroundWorker)sender;

				bw.ReportProgress(0, args);

				args.Stage = DoWorkStage.Indexing;

				while (lastMid >= minMidInDb)
				{
					bw.ReportProgress(0, args);
					
					if (bw.CancellationPending)
					{
						e.Cancel = true;
						break;
					}

					List<MessageSearchInfo> items;
					var localLastMid = lastMid;
					using (var db = _provider.CreateDBContext())
						items =
							db
								.Messages(m => m.ID <= localLastMid)
								.OrderByDescending(m => m.ID)
								.Take(_messagesBatchSize)
								.Select(
									m =>
										new MessageSearchInfo(
											m.ID,
											m.Date,
											m.Subject,
											m.Message,
											m.ForumID,
											m.UserID,
											m.UserNick))
								.ToList();
					var reader = IndexReader.Open(writer.GetDirectory(), false);
					try
					{
						foreach (var item in items)
						{
							var term = baseTerm.CreateTerm(item.MessageID.ToString());
							lastMid = item.MessageID;
							args.ProcessedMessages += 1;
							if (!(reader.DocFreq(term) > 0))
							{
								writer.AddDocument(item.CreateDocument());
								indexedCount += 1;
							}
						}
						lastMid -= 1; // сдвигаемся на единицу меньше последнего выбранного
					}
					finally
					{
						reader.Close();
					}
				}

				bw.ReportProgress(0, args);

				if (args.DoOptimize)
				{
					args.Stage = DoWorkStage.Optimizing;
					bw.ReportProgress(0, args);
					writer.SetUseCompoundFile(true);
					writer.Optimize();
				}
			}
			finally
			{
				writer.Close();
			}

			e.Result = indexedCount;
		}

		private void IndexBackgroundWorkerProgressChanged(object sender,
			ProgressChangedEventArgs e)
		{
			var a = (DoWorkArgs)e.UserState;

			progressBar.Value  = a.ProcessedPercent;
			progressLabel.Text = string.Format(SR.BuildIndexForm.ProgressText,
				a.ProcessedMessages, a.TotalMessages,
				a.ProcessedPercent);

			actionTextBox.Text = string.Format(StageToString(a.Stage), 
				a.ProcessedMessages, 
				a.ProcessedPercent);
		}

		private void IndexBackgroundWorkerRunWorkerCompleted(object sender,
			RunWorkerCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				actionTextBox.Text = SR.BuildIndexForm.Action.ErrorText.FormatStr(e.Error.Message);
				_provider.LogInfo(
					string.Format(SR.Search.IndexingTerminatedByError,
						e.Error.Message));
			}
			else if (e.Cancelled)
			{
				actionTextBox.Text = SR.BuildIndexForm.Action.CancelText;
				_provider.LogInfo(SR.Search.IndexingTerminatedByUser);
			}
			else
			{
				actionTextBox.Text = SR.BuildIndexForm.Action.CompleteText;
				_provider.LogInfo(SR.Search.IndexingFinished);
			}

			startButton.Enabled      = true;
			cleanButton.Enabled      = true;
			closeButton.Enabled      = true;
			optimizeCheckBox.Enabled = true;
		}

		private void BuildIndexForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// Нельзя закрыть форму, если поток работает
			e.Cancel = indexBackgroundWorker.IsBusy;
		}

		private static string StageToString(DoWorkStage s)
		{
			switch (s)
			{
				case DoWorkStage.Preparing:
					return SR.BuildIndexForm.Action.PreparingText;
				case DoWorkStage.Indexing:
					return SR.BuildIndexForm.Action.IndexingText;
				case DoWorkStage.Optimizing:
					return SR.BuildIndexForm.Action.OptimizingText;
				default:
					return string.Empty;
			}
		}

		#region Nested type: DoWorkArgs

		private class DoWorkArgs
		{
			private DoWorkStage _stage = DoWorkStage.Preparing;

			internal DoWorkArgs(bool doOptimize)
			{
				DoOptimize = doOptimize;
			}

			public DoWorkStage Stage
			{
				get { return _stage; }
				set { _stage = value; }
			}

			public bool DoOptimize { get; private set; }
			public int TotalMessages { get; set; }
			public int ProcessedMessages { get; set; }

			public int ProcessedPercent
			{
				get
				{
					if (TotalMessages == 0)
						return 0;
					return 100 * ProcessedMessages / TotalMessages;
				}
			}
		}

		#endregion

		#region Nested type: DoWorkStage

		/// <summary>
		/// Этап работы потока.
		/// </summary>
		private enum DoWorkStage
		{
			Preparing,
			Indexing,
			Optimizing,
		}

		#endregion
	}
}