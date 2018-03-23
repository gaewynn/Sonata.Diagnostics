#region Namespace Sonata.Diagnostics.Extensions
//	TODO
#endregion

using log4net;
using log4net.Config;
using Microsoft.Extensions.Logging;
using Sonata.Core.Extensions;
using Sonata.Diagnostics.Logging;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Sonata.Diagnostics.Extensions
{
	/// <summary>
	/// Represents a component adding functionalities to the <see cref="ILoggerFactory"/> interface.
	/// </summary>
	public static class LoggerFactoryExtension
	{
		#region Methods

		public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, Action<Log4NetProviderOptions> log4NetOptions)
		{
			if (factory == null)
				throw new ArgumentNullException(nameof(factory));
			if (log4NetOptions == null)
				throw new ArgumentNullException(nameof(log4NetOptions));

			var options = new Log4NetProviderOptions();
			log4NetOptions(options);

			ConfigureLog4Net(options);

			using (var provider = new Log4NetLoggerProvider(options))
			{
				provider.Options = options;
				factory.AddProvider(provider);
			}

			return factory;
		}

		private static void ConfigureLog4Net(Log4NetProviderOptions log4NetOptions)
		{
			var log4NetElement = ParseLog4NetConfigFile(log4NetOptions.ConfigurationFileFullName);
			var serializer = new XmlSerializer(typeof(Logging.Schema.log4net));

			log4NetOptions.Log4NetConfiguration = serializer.Deserialize(log4NetElement.CreateReader()) as Logging.Schema.log4net;

			XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetEntryAssembly()), log4NetElement.ToXmlElement());
		}

		private static XElement ParseLog4NetConfigFile(string filename)
		{
			var content = File.ReadAllText(filename);
			var document = XDocument.Parse(content);

			var element = document
				.DescendantNodes()
				.FirstOrDefault(e => e.NodeType == XmlNodeType.Element && ((XElement)e).Name == "log4net");

			return (XElement)element;
		}

		#endregion
	}
}
