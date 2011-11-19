using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Rsdn.Janus
{
	internal sealed class SqlClause
	{
		private StringBuilder _clause = new StringBuilder();
		private StringBuilder _where = new StringBuilder();
		private StringBuilder _order = new StringBuilder();

		public string Select
		{
			get { return _clause.ToString(); }
			set { _clause = new StringBuilder(value); }
		}
		public void AppendWhereFormat(IFormatProvider provider, string format, params object[] args)
		{
			if (_where.Length > 0)
				_where.Append(" AND ");
			_where.AppendFormat(provider, format, args);
		}

		public void AppendWhere(string value)
		{
			if (_where.Length > 0)
				_where.Append(" AND ");
			_where.Append(value);
		}

		public void AppendOrder(string value)
		{
			if (_order.Length > 0)
				_order.Append(", ");
			_order.Append(value);
		}

		public override string ToString()
		{
			return ToString(CultureInfo.InvariantCulture);
		}

		public string ToString(IFormatProvider provider)
		{
			StringBuilder res = new StringBuilder(_clause.ToString());
			if (_where.Length > 0)
				res.AppendFormat(provider, " WHERE {0} ", _where);
			if (_order.Length > 0)
				res.AppendFormat(provider, " ORDER BY {0} ", _order);
			return res.ToString();
		}
	}
}
