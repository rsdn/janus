using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обеспечивает базовую функциональность ядра системы.
	/// </summary>
	internal class JanusHost : IServiceProvider, IDisposable, ISupportInitialize
	{
		private const string _extensionDescriptorFileName = "Janus.Extension";

		private const string _extensionDescriptorSchemaResource =
			"Rsdn.Janus.Core.Extensibility.JanusExtensionDescriptor.xsd";

		private const string _extensionDescriptorSchemaUri =
			"http://rsdn.ru/projects/Janus/JanusExtensionDescriptor.xsd";

		private readonly ServiceManager _serviceManager;
		private readonly IActivePartManager _activePartManager;

		internal JanusHost(IServiceProvider serviceProvider)
		{
			_serviceManager =
				serviceProvider == null
					? new ServiceManager(true)
					: new ServiceManager(true, serviceProvider);

			// TODO: Hack! Must be eliminated.
			ApplicationManager.Instance.ServiceProvider = this;

			_serviceManager.PublishDisposable<IEventBroker>(new EventBroker());
			InitExtensibility();
			_activePartManager = ActivePartsHelper.CreateAndPublishManager(_serviceManager);

		}

		private IEnumerable<string> GetExtensionAssemblies(string extDir)
		{
			var stream = GetType().Assembly
				.GetRequiredResourceStream(_extensionDescriptorSchemaResource);
			var schema = XmlSchema.Read(
				stream,
				null);
			var readerSettings = new XmlReaderSettings();
			readerSettings.Schemas.Add(schema);
			foreach (var dir in Directory.GetDirectories(extDir))
			{
				var descFile = Path.Combine(dir, _extensionDescriptorFileName);
				if (!File.Exists(descFile))
					continue;
				var reader = XmlReader.Create(descFile, readerSettings);
				var xDoc = new XmlDocument();
				var nsMgr = new XmlNamespaceManager(xDoc.NameTable);
				nsMgr.AddNamespace("d", _extensionDescriptorSchemaUri);
				xDoc.Load(reader);
				var nodes = xDoc.SelectNodes(
					"/d:janus-extension-assemblies/d:extension-assembly",
					nsMgr);
				if (nodes != null)
					foreach (XmlElement element in nodes)
						yield return Path.Combine(dir, element.InnerText);
			}
		}

		private void InitExtensibility()
		{
			var asmHelper = new AssemblyScanHelper();

			// Добавляем собственную сборку
			asmHelper.AddAssembly(GetType().Assembly);
			// Добавляем Janus-Common
			asmHelper.AddAssembly(typeof(ExtensionInfoProviderBase).Assembly);
			// Добавляем сборки всех расширений
			foreach (var asmPath in GetExtensionAssemblies(EnvironmentHelper.GetJanusRootDir()))
			{
				asmHelper.AddAssembly(Assembly.LoadFrom(asmPath));
				Trace.WriteLine("Use extension assembly '{0}'".FormatStr(asmPath));
			}

			var extensionManager = new ExtensionManager(_serviceManager);
			StrategyFactoryStrategy.RegisterAndScan(
				_serviceManager,
				extensionManager,
				asmHelper.GetTypes());
		}

		#region IServiceProvider Members

		public object GetService(Type serviceType)
		{
			return _serviceManager.GetService(serviceType);
		}

		#endregion

		#region Implementation of IDisposable
		public void Dispose()
		{
			_serviceManager.Dispose();
		}
		#endregion

		#region Implementation of ISupportInitialize
		public void BeginInit()
		{}

		public void EndInit()
		{
			_activePartManager.Activate();
		}
		#endregion
	}
}
