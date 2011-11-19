using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class DefaultCommandService : IDefaultCommandService
	{
		private static readonly ReadOnlyDictionary<string, object> _emptyParameters =
			new Dictionary<string, object>().AsReadOnly();

		private readonly Subject<EventArgs> _defaultCommandChanged = new Subject<EventArgs>();

		public DefaultCommandService(string commandName)
			: this(commandName, null) { }

		public DefaultCommandService(
			string commandName,
			IDictionary<string, object> commandParameters)
		{
			SetCommandAndParametersCore(commandName, commandParameters);
		}

		#region Implementation of IDefaultCommandService

		[CanBeNull]
		public string CommandName { get; private set; }

		[CanBeNull]
		public IDictionary<string, object> Parameters { get; private set; }

		public void SetDefaultCommand(
			string commandName,
			IDictionary<string, object> parameters)
		{
			SetCommandAndParametersCore(commandName, parameters);
			_defaultCommandChanged.OnNext(EventArgs.Empty);
		}

		public IObservable<EventArgs> DefaultCommandChanged
		{
			get { return _defaultCommandChanged; }
		}

		#endregion

		private void SetCommandAndParametersCore(
			string commandName,
			IDictionary<string, object> parameters)
		{
			if (commandName == null && parameters != null)
				throw new ArgumentException(
					@"Параметры команды должны быть ранвы null, когда имя команды равно null.",
					"parameters");

			CommandName = commandName;
			Parameters = parameters != null
				? new Dictionary<string, object>(parameters).AsReadOnly()
				: _emptyParameters;
		}
	}
}