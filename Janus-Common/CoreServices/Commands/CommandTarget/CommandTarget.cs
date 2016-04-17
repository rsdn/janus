using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using CodeJam;
using CodeJam.Extensibility;
using CodeJam.Services;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public abstract class CommandTarget : ICommandTarget
	{
		private readonly Dictionary<string, CommandMethod> _executeMethods =
			new Dictionary<string, CommandMethod>(StringComparer.OrdinalIgnoreCase);
		private readonly Dictionary<string, CommandMethod> _statusMethods =
			new Dictionary<string, CommandMethod>(StringComparer.OrdinalIgnoreCase);
		private readonly List<SubscribeMethod> _subscribeMethods =
			new List<SubscribeMethod>();

		protected CommandTarget([NotNull] IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));

			var commandService = serviceProvider.GetRequiredService<ICommandService>();

			var subscribeCommands = new List<string>();
			foreach (var method in
				GetType().GetMethods(
					BindingFlags.Instance
					| BindingFlags.Static
					| BindingFlags.Public
					| BindingFlags.NonPublic))
			{
				var attributes = Attribute.GetCustomAttributes(method);

				if (attributes.Length == 0)
					continue;

				foreach (var attribute in attributes)
				{
					var executorAttribute = attribute as CommandExecutorAttribute;
					if (executorAttribute != null)
					{
						var commandName = executorAttribute.CommandName;

						if (_executeMethods.ContainsKey(commandName))
							throw new ApplicationException($"Метод выполнения команды '{commandName}' определен несколько раз.");

						_executeMethods[commandName] =
							CreateExecuteMethod(commandService.GetCommandInfo(commandName), method);

						continue;
					}

					var statusAttribute = attribute as CommandStatusGetterAttribute;
					if (statusAttribute != null)
					{
						var commandName = statusAttribute.CommandName;

						if (_statusMethods.ContainsKey(commandName))
							throw new ApplicationException($"Метод запроса статуса команды '{commandName}' определен несколько раз.");

						_statusMethods[commandName] =
							CreateStatusMethod(commandService.GetCommandInfo(commandName), method);

						continue;
					}

					var subscribeAttr = attribute as CommandStatusSubscriberAttribute;
					if (subscribeAttr != null)
						subscribeCommands.Add(subscribeAttr.CommandName);
				}

				if (subscribeCommands.Count > 0)
				{
					_subscribeMethods.Add(CreateSubscribeMethod(method, subscribeCommands.ToArray()));
					subscribeCommands.Clear();
				}
			}

			foreach (var statusGetter in _statusMethods.Values)
				if (!_executeMethods.ContainsKey(statusGetter.CommandInfo.Name))
					throw new ApplicationException(
						$"Метод запроса статуса команды '{statusGetter.CommandInfo.Name}' определен, а метод выполнения - нет.");

			foreach (var subscribeMethod in _subscribeMethods)
				foreach (var commandName in subscribeMethod.CommandNames)
				{
					if (!_executeMethods.ContainsKey(commandName))
						throw new ApplicationException(
							"Метод подписки на оповещения о смене статуса статуса "
							+ $"'{MethodBase.GetMethodFromHandle(subscribeMethod.MethodHandle).Name}' команды '{commandName}' "
							+ "определен, а метод выполнения - нет.");
					if (!_statusMethods.ContainsKey(commandName))
						throw new ApplicationException(
							"Метод подписки на оповещения о смене статуса статуса "
							+ $"'{MethodBase.GetMethodFromHandle(subscribeMethod.MethodHandle).Name}' команды '{commandName}' "
							+ "определен, а метод запроса статуса - нет.");
				}
		}

		#region ICommandTarget Members

		public void Execute(
			[NotNull] string commandName,
			[NotNull] ICommandContext context)
		{
			if (commandName == null)
				throw new ArgumentNullException(nameof(commandName));
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			if (QueryStatus(commandName, context) != CommandStatus.Normal)
				throw new InvalidOperationException(
					$"Команда \"{commandName}\" не может быть выполнена, так как этого не позволяет её статус.");

			InvokeCommandMethod(_executeMethods[commandName], context);
		}

		public CommandStatus QueryStatus(
			[NotNull] string commandName,
			[NotNull] ICommandContext context)
		{
			if (commandName == null)
				throw new ArgumentNullException(nameof(commandName));
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			CommandMethod method;
			if (_statusMethods.TryGetValue(commandName, out method))
				return (CommandStatus)InvokeCommandMethod(method, context);

			return _executeMethods.ContainsKey(commandName)
				? CommandStatus.Normal
				: CommandStatus.Unavailable;
		}

		public IDisposable SubscribeStatusChanged(
			[NotNull] IServiceProvider serviceProvider,
			[NotNull] EventHandler<ICommandTarget, string[]> handler)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));
			if (handler == null)
				throw new ArgumentNullException(nameof(handler));


			return
				_subscribeMethods
					.Select(
						subscribeMethod =>
							(IDisposable)MethodBase.GetMethodFromHandle(subscribeMethod.MethodHandle).Invoke(
								this,
								new object[]
								{
									serviceProvider,
									(Action)(() => handler(this, subscribeMethod.CommandNames))
								}))
					.ToArray()
					.Merge();
		}

		#endregion

		private static CommandMethod CreateExecuteMethod(ICommandInfo commandInfo, MethodInfo method)
		{
			try
			{
				if (method.ReturnType != typeof(void))
					throw new ApplicationException(
						$"Метод выполнения команды '{commandInfo.Name}' должен иметь тип возврата void.");

				CheckParameters(commandInfo, method.GetParameters());
			}
			catch (ApplicationException exception)
			{
				throw new ApplicationException(
					$"Метод выполнения команды '{commandInfo.Name}' имеет неверную сигнатуру.",
					exception);
			}

			return new CommandMethod(commandInfo, method.MethodHandle);
		}

		private static CommandMethod CreateStatusMethod(ICommandInfo commandInfo, MethodInfo method)
		{
			try
			{
				if (method.ReturnType != typeof(CommandStatus))
					throw new ApplicationException(
						$"Метод запроса статуса команды '{commandInfo.Name}' должен иметь тип возврата CommandStatus.");

				CheckParameters(commandInfo, method.GetParameters());
			}
			catch (ApplicationException exception)
			{
				throw new ApplicationException(
					$"Метод запроса статуса команды '{commandInfo.Name}' имеет неверную сигнатуру.",
					exception);
			}

			return new CommandMethod(commandInfo, method.MethodHandle);
		}

		private static void CheckParameters(ICommandInfo commandInfo, ParameterInfo[] parameters)
		{
			if (parameters.Length < 1
					|| parameters[0].ParameterType != typeof(ICommandContext))
				throw new ApplicationException(
					"Отсутствует первый параметр, имеющий тип ICommandContext.");

			foreach (var methodParameter in parameters.Skip(1))
			{
				if (!commandInfo.IsParameterExists(methodParameter.Name))
					throw new ApplicationException($"Параметр '{methodParameter.Name}' не предусмотрен командой.");
				if (commandInfo.GetParameter(methodParameter.Name).IsOptional
						&& methodParameter.ParameterType.IsValueType
						&& methodParameter.ParameterType == typeof(Nullable<>))
					throw new ApplicationException(
						$"Необязательный параметр '{methodParameter.Name}' должен иметь ссылочный тип.");
			}
		}

		private static SubscribeMethod CreateSubscribeMethod(
			MethodInfo method, string[] commandNames)
		{
			var parameters = method.GetParameters();
			if (parameters.Length != 2
					|| !typeof(IServiceProvider).IsAssignableFrom(parameters[0].ParameterType)
					|| !typeof(Action).IsAssignableFrom(parameters[1].ParameterType)
					|| method.ReturnType != typeof(IDisposable))
				throw new ApplicationException(
					$"Метод подписки на оповещения о смене статуса команд '{method}' имеет неверную сигатуру.");

			return new SubscribeMethod(commandNames, method.MethodHandle);
		}

		private object InvokeCommandMethod(CommandMethod method, ICommandContext context)
		{
			var methodInfo = MethodBase.GetMethodFromHandle(method.MethodHandle);
			var parameterInfo = methodInfo.GetParameters();
			var args = new object[parameterInfo.Length];

			args[0] = context;

			for (var i = 1; i < parameterInfo.Length; i++)
			{
				var methodParamInfo = parameterInfo[i];
				var parameterName = methodParamInfo.Name;

				if (context.IsParameterExists(parameterName))
				{
					var parameter = context[parameterName];
					if (parameter != null)
					{
						var methodParameterType = methodParamInfo.ParameterType;
						try
						{
							args[i] = Convert(parameter, methodParameterType);
						}
						catch (ApplicationException exception)
						{
							throw new Exception(
								$"Параметр '{parameterName}' команды '{method.CommandInfo.Name}' не может быть конвертирован в "
								+ $"'{methodParameterType}', для передачи обработчику '{methodInfo.Name}'.",
								exception);
						}
					}
				}
				else if (!method.CommandInfo.GetParameter(parameterName).IsOptional)
					throw new ArgumentException(
						$"Параметр '{parameterName}' команды '{method.CommandInfo.Name}' не найден в контексте.",
						nameof(context));
			}

			return methodInfo.Invoke(this, args);
		}

		private static object Convert(object value, Type destinationType)
		{
			var sourceType = value.GetType();

			if (destinationType.IsAssignableFrom(sourceType))
				return value;

			if (destinationType.IsArray)
			{
				if (destinationType.GetArrayRank() != 1)
					throw new NotSupportedException(("Многомерные массивы не поддерживаются."));

				var elementType = destinationType.GetElementType();

				if (elementType.IsArray)
					throw new NotSupportedException("Вложенные массивы не поддерживаются.");

				var arrStr = value as string;

				if (arrStr == null)
					return null;

				var stringElements = arrStr.Split(',');
				for (var i = 0; i < stringElements.Length; i++)
					stringElements[i] = stringElements[i].Trim();

				if (elementType.IsAssignableFrom(typeof(string)))
					return stringElements;

				var result = new object[stringElements.Length];
				for (var i = 0; i < result.Length; i++)
					try
					{
						result[i] = Convert(stringElements[i], elementType);
					}
					catch (Exception exception)
					{
						throw new ApplicationException(
							$"Не удалось конвертировать элемент массива с индексом '{i}'.",
							exception);
					}
				return result;
			}

			var sourceConverter = TypeDescriptor.GetConverter(sourceType);
			if (sourceConverter.CanConvertTo(destinationType))
				return sourceConverter.ConvertTo(value, destinationType);

			var destinationConverter = TypeDescriptor.GetConverter(destinationType);
			if (destinationConverter.CanConvertFrom(sourceType))
				return destinationConverter.ConvertFrom(value);

			throw new ApplicationException($"Значение типа '{sourceType}' не может быть конвертировано в '{destinationType}'");
		}

		#region Helper Structures

		private struct CommandMethod
		{
			private readonly ICommandInfo _commandInfo;
			private readonly RuntimeMethodHandle _method;

			public CommandMethod(ICommandInfo commandInfo, RuntimeMethodHandle method)
			{
				_commandInfo = commandInfo;
				_method = method;
			}

			public ICommandInfo CommandInfo
			{
				get { return _commandInfo; }
			}

			public RuntimeMethodHandle MethodHandle
			{
				get { return _method; }
			}
		}

		private struct SubscribeMethod
		{
			private readonly RuntimeMethodHandle _method;
			private readonly string[] _commandNames;

			public SubscribeMethod(string[] commandNames, RuntimeMethodHandle method)
			{
				_method = method;
				_commandNames = commandNames;
			}

			public RuntimeMethodHandle MethodHandle
			{
				get { return _method; }
			}

			public string[] CommandNames
			{
				get { return _commandNames; }
			}
		}

		#endregion
	}
}