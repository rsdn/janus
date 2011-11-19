using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

using BLToolkit.Data.Linq;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Работа с переменными, хранящимися в БД.
	/// </summary>
	[Service(typeof (IDBVarsManager))]
	public class DBVars : IDBVarsManager
	{
		private readonly IServiceProvider _provider;
		private readonly Dictionary<string, object> _defaultValues = new Dictionary<string, object>();
		private readonly Dictionary<string, object> _varsCache = new Dictionary<string, object>();
		private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

		public DBVars(IServiceProvider provider)
		{
			_provider = provider;
		}

		private DBVars()
		{
			_defaultValues.Add(@"LastForumRowVersion",  @"00000000000801CF");
			_defaultValues.Add(@"LastRatingRowVersion", @"00000000000801B1");
			_defaultValues.Add(@"LastUserRowVersion",   @"00000000000801B2");
			_defaultValues.Add(@"VersionDB", @"5");
		}

		private static bool IsVarExists(IDataContext db, string name)
		{
			return db.Vars().Any(v => v.Name == name);
		}

		private object CreateValue(string name, IDataContext db, object value)
		{
			if (value == null)
				value = _defaultValues.ContainsKey(name) ? _defaultValues[name] : '0';
			db
				.Vars()
					.Value(_ => _.Name,  name)
					.Value(_ => _.Value, value)
				.Insert();
			return value;
		}

		private object GetVar(string name)
		{
			using (var db = _provider.CreateDBContext())
			using (_rwLock.GetUpgradeableReaderLock())
			{
				if (_varsCache.ContainsKey(name))
					return _varsCache[name];

				object value;
				if (!IsVarExists(db, name))
					value = CreateValue(name, db, null);
				else
					value =
						db
							.Vars(v => v.Name == name)
							.Select(v => v.Value)
							.Single();
				using (_rwLock.GetWriterLock())
					_varsCache.Add(name, value);

				return value;
			}
		}

		private void SetVar(string name, object value)
		{
			using (var db = _provider.CreateDBContext())
			using (_rwLock.GetUpgradeableReaderLock())
			{
				if (_varsCache.ContainsKey(name))
					_varsCache[name] = value;
				else
					using (_rwLock.GetWriterLock())
						_varsCache.Add(name, value);

				if (!IsVarExists(db, name))
					CreateValue(name, db, value);
				else
					db
						.Vars(v => v.Name == name)
						.Set(_ => _.Value, () => value)
						.Update();
			}
		}

		public string this[[Localizable(false)]string name]
		{
			get { return GetVar(name).ToString(); }
			set { SetVar(name, value); }
		}
	}
}