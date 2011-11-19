using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using Rsdn.Janus.Core.DataSchemaManagement;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// DataBaseSchemaManagement - DBSM
	/// Класс менеджер для управления схемами данных
	/// </summary>
	public static class DBSchemaManager
	{
		private const string _dbscMustBePath = "DbSchema.xml";
		private const string _dbscMustBeResource = "Rsdn.Janus." + _dbscMustBePath;
		// TODO: Никаких дефолтных драйверов быть не должно. Все брать из профиля.
		private const string _defaultDriver = "Jet";

		private static readonly XmlSerializer _serializer = new XmlSerializer(typeof(DBSchema));

		public static bool CheckDB(IServiceProvider serviceProvider)
		{
			IDBDriver driver;
			var svc = serviceProvider.GetRequiredService<IDBDriverManager>();
			if (Config.Instance.ConnectionString == null || Config.Instance.DbDriver == null)
			{
				driver = svc.GetDriver(_defaultDriver);
				// Настройки по-умолчанию.
				var csb = driver.CreateConnectionString();

				Config.Instance.DbDriver = _defaultDriver;
				Config.Instance.ConnectionString = csb.ConnectionString;
			}
			else
				driver = svc.GetDriver(Config.Instance.DbDriver);

			if (!driver.CheckConnectionString(Config.Instance.ConnectionString) ||
				Config.Instance.BadRestruct)
			{
				using (var seldb = new SelectDB(serviceProvider))
				{
					var result = seldb.ShowDialog();

					if (result == DialogResult.OK && !string.IsNullOrEmpty(seldb.ConnectionString))
					{
						Config.Instance.DbDriver = seldb.DbDriver;
						Config.Instance.ConnectionString = seldb.ConnectionString;
						Config.Instance.BadRestruct = false;
						Config.Save();
					}
					else if (result == DialogResult.Cancel)
						return false; // Abort
				}
			}

			return true; // Continue
		}

		/// <summary>
		/// Проверяет - требуется ли реструктуризация.
		/// </summary>
		public static bool IsNeedRestructuring(IServiceProvider provider)
		{
			var dbscMustBeVersion = LoadVersionRes(_dbscMustBeResource);
			try
			{
				var dbscExistVersion = int.Parse(provider.DBVars()["VersionDB"]);
				return dbscExistVersion != dbscMustBeVersion;
			}
			catch (Exception ex)
			{
				Trace.WriteLine(ex);
				return true;
			}
		}

		/// <summary>
		/// Производит реструкторизацию.
		/// </summary>
		public static void Restruct(IServiceProvider serviceProvider)
		{
			try
			{
				var schemaDriver = CreateSchemaDriver(serviceProvider, Config.Instance.DbDriver);
				var dbscMustBe = Load(_dbscMustBeResource);

				var conStr = Config.Instance.ConnectionString;
				schemaDriver.CompareDbsc(dbscMustBe, conStr);

				var location = Assembly.GetExecutingAssembly().Location;
				Debug.Assert(location != null);
				var uriBase = new Uri(location);
				schemaDriver.PrepareToSqlFile(Path.Combine(Path.GetDirectoryName(uriBase.LocalPath), "restructure.sql"));
				schemaDriver.Prepare(conStr);
			}
			catch (Exception ex)
			{
				throw new DBSchemaException(SchemaManagementResources.RestructureError + ex.Message, ex);
			}
			var dbscMustBeVersion = LoadVersionRes(_dbscMustBeResource);
			serviceProvider.DBVars()["VersionDB"] = dbscMustBeVersion.ToString();
		}

		/// <summary>
		/// Переводит базу в другой формат.
		/// </summary>
		public static void Migrate(IServiceProvider serviceProvider)
		{
			using (var seldb = new SelectDB(serviceProvider))
			{
				var result = seldb.ShowDialog();

				if (result != DialogResult.OK || string.IsNullOrEmpty(seldb.ConnectionString))
					return;

				if (Config.Instance.DbDriver == seldb.DbDriver &&
					seldb.ConnectionString == Config.Instance.ConnectionString)
				{
					MessageBox.Show(SR.Database.CannotMigrateToSelf,
						ApplicationInfo.ApplicationName,
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}

				var currentCulture = Thread.CurrentThread.CurrentCulture;

				try
				{
					// Ugly hack to help Jet to convert DateTime to right formatted string
					//
					Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

					var dbswExistent = CreateSchemaDriver(serviceProvider, Config.Instance.DbDriver);
					var dbswTarget = CreateSchemaDriver(serviceProvider, seldb.DbDriver);

					var dbscMustBe = Load(_dbscMustBeResource);

					var targetConStr = seldb.ConnectionString;
					dbswTarget.CompareDbsc(dbscMustBe, targetConStr);
					dbswTarget.Prepare(targetConStr);

					const int batchSize = 10000;
					const int timeout = 0;

					ProgressWorker.Run(serviceProvider, false,
						progressVisualizer =>
						{
							progressVisualizer.SetProgressText(SR.Database.MigrationStarted);
							foreach (var dbsmTable in dbscMustBe.Tables)
							{
								progressVisualizer.SetProgressText(dbsmTable.Name);

								using (var srcConnection = dbswExistent.CreateConnection(Config.Instance.ConnectionString))
								using (var dstConnection = dbswTarget.CreateConnection(targetConStr))
								using (var srcCommand = srcConnection.CreateCommand())
								using (var dstCommand = dstConnection.CreateCommand())
								{
									srcConnection.Open();
									srcCommand.CommandTimeout = timeout;
									srcCommand.CommandText = dbswExistent.MakeSelect(dbsmTable, true);

									var recordsCount = 0;
									using (var reader = srcCommand.ExecuteReader(CommandBehavior.SequentialAccess))
										if (reader.Read())
										{
											dstConnection.Open();

											dstCommand.CommandTimeout = timeout;
											dstCommand.CommandText = dbswTarget.MakeInsert(dbsmTable);
											dstCommand.Prepare();

											foreach (var column in dbsmTable.Columns)
											{
												var p = dstCommand.CreateParameter();
												dstCommand.Parameters.Add(dbswTarget.ConvertToDbParameter(column, p));
											}

											dbswTarget.BeginTableLoad(dstConnection, dbsmTable);
											// Make sure we have all or none records inserted
											//
											using (var transaction = dstConnection.BeginTransaction())
											{
												dstCommand.Transaction = transaction;

												do
												{
													for (var i = 0; i < dbsmTable.Columns.Length; i++)
														((IDbDataParameter)dstCommand.Parameters[i]).Value = reader[i];

													dstCommand.ExecuteNonQuery();

													if (++recordsCount%batchSize == 0)
														progressVisualizer.SetProgressText(dbsmTable.Name + @" (" + recordsCount + @")");
												} while (reader.Read());

												transaction.Commit();
											}
											dbswTarget.EndTableLoad(dstConnection, dbsmTable);
										}
								}
							}
						});

					if (MessageBox.Show(SR.Database.ConfirmRestartAfterMigrate,
						ApplicationInfo.ApplicationName,
						MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					{
						Config.Instance.DbDriver = seldb.DbDriver;
						Config.Instance.ConnectionString = seldb.ConnectionString;
						Config.Save();

						// Bye-bye all unsaved messages!
						//
						Application.Restart();
					}
				}
				catch (Exception ex)
				{
					throw new DBSchemaException(string.Format(
						SchemaManagementResources.MigrationError,
						Config.Instance.DbDriver, seldb.DbDriver, ex.Message), ex);
				}
				finally
				{
					Thread.CurrentThread.CurrentCulture = currentCulture;
				}
			}
		}

		#region Private methods

		private static int LoadVersionRes(string path)
		{
			using (TextReader tr = new StreamReader(
				Assembly
					.GetExecutingAssembly()
					.GetRequiredResourceStream(path)))
			{
				var doc = new XmlDocument();
				doc.Load(tr);
				var root = doc.DocumentElement;
				Debug.Assert(root != null);
				return Convert.ToInt32(root.Attributes["version"].Value);
			}
		}

		private static IDBSchemaDriver CreateSchemaDriver(
			IServiceProvider serviceProvider,
			string driverName)
		{
			var svc = serviceProvider.GetRequiredService<IDBDriverManager>();
			return svc.GetDriver(driverName).CreateSchemaDriver();
		}

		private static DBSchema Load(string path)
		{
			DBSchema dbsc;
			if (Assembly.GetExecutingAssembly().GetManifestResourceInfo(path) == null)
			{
				using (TextReader tr = new StreamReader(path))
					dbsc = (DBSchema)_serializer.Deserialize(tr);
			}
			else
			{
				using (TextReader tr = new StreamReader(
					Assembly
						.GetExecutingAssembly()
						.GetRequiredResourceStream(path)))
				{
					dbsc = (DBSchema)_serializer.Deserialize(tr);
				}
			}
			dbsc.Normalize();
			return dbsc;
		}

		#endregion
	}
}
