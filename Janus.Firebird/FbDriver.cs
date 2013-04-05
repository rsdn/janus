using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;

using FirebirdSql.Data.FirebirdClient;

using LinqToDB.DataProvider;
using LinqToDB.DataProvider.Firebird;

namespace Rsdn.Janus.Firebird
{
	[JanusDBDriver(
		DriverName,
		"Rsdn.Janus.Firebird.Resources",
		"DriverDisplayName",
		"DriverDescription")]
	internal class FBDriver : IDBDriver
	{
		public const string DriverName = "Fb";

		private static readonly Regex _wrapDbObjectNamesRx = new Regex(
			@"([^\[]*(?<object>\[[^\]]*\])[^\[]*)*",
			RegexOptions.IgnoreCase);

		private readonly FbSqlFormatter _sqlFormatter = new FbSqlFormatter();

		public FBDriver(IServiceProvider provider)
		{
		}

		#region IDBDriver Members
		public bool CheckConnectionString(string constr)
		{
			try
			{
				var csbCheck = new FbConnectionStringBuilder(constr) {Pooling = false};

				using (var con = new FbConnection(csbCheck.ConnectionString))
					con.Open();
			}
			catch
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Получить драйвер схемы.
		/// </summary>
		/// <returns></returns>
		public IDBSchemaDriver CreateSchemaDriver()
		{
			return new FBSchemaDriver();
		}

		public DbConnectionStringBuilder CreateConnectionString()
		{
			return new FbConnectionStringBuilder();
		}

		public DbConnectionStringBuilder CreateConnectionString(string constr)
		{
			return new FbConnectionStringBuilder(constr);
		}

		public IDBConfigControl CreateConfigControl()
		{
			return new FbConfigControl();
		}

		/// <summary>
		/// Создать провайдер для BLToolkit.
		/// </summary>
		public IDataProvider CreateDataProvider()
		{
			return new FirebirdDataProvider();
		}

		/// <summary>
		/// Обработать запрос перед выполнением.
		/// </summary>
		public string PreprocessQueryText(string text)
		{
			return WrapDbObjectNames(text);
		}

		/// <summary>
		/// Ссылка на форматтер SQL.
		/// </summary>
		public ISqlFormatter Formatter
		{
			get { return _sqlFormatter; }
		}
		#endregion

		/// <summary>
		/// Оборачивает имена объектов указаных как [object]
		/// в специфичный для движка identifier delimiter
		/// </summary>
		/// <param name="sql">Инструкция для обработки</param>
		private static string WrapDbObjectNames(string sql)
		{
			var match = _wrapDbObjectNamesRx.Match(sql);

			if (match.Success)
			{
				var replacements =
					new Dictionary<string, string>();

				foreach (Capture capture in match.Groups["object"].Captures)
					if (!replacements.ContainsKey(capture.Value))
						replacements.Add(
							capture.Value,
							capture.Value.Replace(@"[", @"""").Replace(@"]", @""""));

				sql = replacements.Aggregate(sql, (current, kvp) => current.Replace(kvp.Key, kvp.Value));
			}

			return sql;
		}
	}
}