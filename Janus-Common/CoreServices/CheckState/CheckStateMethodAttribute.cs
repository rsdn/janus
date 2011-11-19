using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public abstract class CheckStateMethodAttribute : Attribute
	{
		private readonly string _name;

		protected CheckStateMethodAttribute([NotNull] string name)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (!CheckStateNamesValidator.IsValidCheckStateName(name))
				throw new ArgumentException(
					"Имя состояния галочки менеет некорректный формат.", "name");

			_name = name;
		}

		public string Name
		{
			get { return _name; }
		}
	}
}