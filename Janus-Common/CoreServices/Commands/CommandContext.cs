using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class CommandContext : ICommandContext
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly ReadOnlyDictionary<string, object> _parameters;
		private readonly Action<string> _outputWriter;

		#region Constructors

		public CommandContext(
			[NotNull] IServiceProvider serviceProvider)
			: this(serviceProvider, null, null) { }

		public CommandContext(
			[NotNull] IServiceProvider serviceProvider,
			[CanBeNull] IDictionary<string, object> parameters)
			: this(serviceProvider, parameters, null) { }

		public CommandContext(
			[NotNull] IServiceProvider serviceProvider,
			[CanBeNull] Action<string> outputWriter)
			: this(serviceProvider, null, outputWriter) { }

		public CommandContext(
			[NotNull] IServiceProvider serviceProvider,
			[CanBeNull] IDictionary<string, object> parameters,
			[CanBeNull] Action<string> outputWriter)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));

			_serviceProvider = serviceProvider;
			_parameters = parameters != null ?
				new Dictionary<string, object>(parameters).AsReadOnly()
				: null;
			_outputWriter = outputWriter;
		} 

		#endregion

		#region ICommandContext Members

		public object this[[NotNull] string name]
		{
			get
			{
				if (name == null)
					throw new ArgumentNullException(nameof(name));

				object result;
				if (_parameters == null || !_parameters.TryGetValue(name, out result))
					throw new ArgumentException(
						$"Параметр '{name}' не найден.", nameof(name));
				return result;
			}
		}

		public bool IsParameterExists([NotNull] string name)
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name));

			return _parameters != null && _parameters.ContainsKey(name);
		}

		public void WriteToOutput([NotNull] string text)
		{
			if (text == null)
				throw new ArgumentNullException(nameof(text));

			_outputWriter?.Invoke(text);
		}

		#endregion

		#region IServiceProvider Members

		object IServiceProvider.GetService(Type serviceType)
		{
			return _serviceProvider.GetService(serviceType);
		}

		#endregion
	}
}
