using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;

using CodeJam;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class CheckStateSource : ICheckStateSource
	{
		private readonly Dictionary<string, MethodInfo> _checkStateGetters
			= new Dictionary<string, MethodInfo>(StringComparer.OrdinalIgnoreCase);
		private readonly List<SubscribeMethod> _checkStateSubscribers
			= new List<SubscribeMethod>();

		public CheckStateSource()
		{
			var subscribeNames = new List<string>();

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
					var checkStateGetterAttribute = attribute as CheckStateGetterAttribute;
					if (checkStateGetterAttribute != null)
					{
						var parameters = method.GetParameters();
						if (parameters.Length != 1
							|| parameters[0].ParameterType != typeof(IServiceProvider)
							|| method.ReturnType != typeof(CheckState?))
							throw new ApplicationException($"Метод получения состояния галочки '{method}' имеет неверную сигатуру.");

						var name = checkStateGetterAttribute.Name;

						if (_checkStateGetters.ContainsKey(name))
							throw new ApplicationException($"Метод получения состояния галочки '{method}' определен несколько раз.");

						_checkStateGetters[name] = method;
					}

					var checkStateSubcriberAttribute = attribute as CheckStateSubscriberAttribute;
					if (checkStateSubcriberAttribute != null)
					{
						var parameters = method.GetParameters();
						if (parameters.Length != 2
								|| parameters[0].ParameterType != typeof(IServiceProvider)
								|| parameters[1].ParameterType != typeof(Action)
								|| method.ReturnType != typeof(IDisposable))
							throw new ApplicationException(
								$"Метод подписки на оповещения о смене состояния галочки '{method}' имеет неверную сигатуру.");

						subscribeNames.Add(checkStateSubcriberAttribute.Name);
					}
				}
				if (subscribeNames.Count > 0)
				{
					_checkStateSubscribers.Add(
						new SubscribeMethod(subscribeNames.ToArray(), method.MethodHandle));
					subscribeNames.Clear();
				}
			}

			foreach (var checkStateName in
				_checkStateSubscribers
					.SelectMany(subsriber => subsriber.Names)
					.Distinct())
			{
				if (!_checkStateGetters.ContainsKey(checkStateName))
					throw new ApplicationException(
						$"Метод подписки на оповещения о смене состояния галочки '{checkStateName}' определен, а метод запроса" +
						" состояния - нет.");
			}
		}

		#region ICheckStateSource Members

		public CheckState? GetCheckState(
			[NotNull] IServiceProvider provider,
			[NotNull] string name)
		{
			if (provider == null)
				throw new ArgumentNullException(nameof(provider));
			if (name == null)
				throw new ArgumentNullException(nameof(name));

			MethodInfo checkStateGetter;
			if (_checkStateGetters.TryGetValue(name, out checkStateGetter))
				return (CheckState?)checkStateGetter.Invoke(this, new object[] { provider });
			return null;
		}

		public IDisposable SubscribeCheckStateChanged(
			[NotNull] IServiceProvider serviceProvider,
			[NotNull] CheckStateChangedEventHandler handler)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));
			if (handler == null)
				throw new ArgumentNullException(nameof(handler));

			return
				_checkStateSubscribers
					.Select(
						subscribeMethod =>
							(IDisposable)
								MethodBase
									.GetMethodFromHandle(subscribeMethod.MethodHandle)
									.Invoke(
										this,
										new object[]
										{
											serviceProvider,
											(Action) (() => handler(this, subscribeMethod.Names))
										}))
					.ToArray()
					.Merge();
		}

		#endregion

		#region Helper Structures

		private struct SubscribeMethod
		{
			public SubscribeMethod(string[] names, RuntimeMethodHandle method)
			{
				MethodHandle = method;
				Names = names;
			}

			public RuntimeMethodHandle MethodHandle { get; }

			public string[] Names { get; }
		}

		#endregion
	}
}