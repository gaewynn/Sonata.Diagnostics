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
using Newtonsoft.Json;

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
			XElement log4NetElement = null;

			try
			{
				if (!String.IsNullOrWhiteSpace(log4NetOptions.ConfigurationFileFullName)
					&& String.IsNullOrWhiteSpace(log4NetOptions.JsonConfiguration))
				{
					log4NetElement = ParseLog4NetConfigFile(log4NetOptions.ConfigurationFileFullName);
				}

				if (!String.IsNullOrWhiteSpace(log4NetOptions.JsonConfiguration))
					log4NetElement = ParseLog4NetDocument(JsonConvert.DeserializeXNode(log4NetOptions.JsonConfiguration, "root"));

				if (log4NetElement == null)
					throw new InvalidOperationException("No log4net configuration defined.");
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("No valid configuration provided for log4net.", ex);
			}

			var serializer = new XmlSerializer(typeof(Logging.Schema.log4net));
			log4NetOptions.Log4NetConfiguration = serializer.Deserialize(log4NetElement.CreateReader()) as Logging.Schema.log4net;

			if (log4NetOptions.Configure != null)
				log4NetOptions.Configure(log4NetElement);
			else
				XmlConfigurator.Configure(LogManager.GetRepository(log4NetOptions.RepositoryAssembly ?? Assembly.GetEntryAssembly()), log4NetElement.ToXmlElement());
		}

		private static XElement ParseLog4NetConfigFile(string filename)
		{
			if (!File.Exists(filename))
				throw new FileNotFoundException("The provided log4net configuration file does not exist.", filename);

			var content = File.ReadAllText(filename);
			var document = XDocument.Parse(content);

			return ParseLog4NetDocument(document);
		}

		private static XElement ParseLog4NetDocument(XDocument configuration)
		{
			var element = configuration
				.DescendantNodes()
				.FirstOrDefault(e => e.NodeType == XmlNodeType.Element && ((XElement)e).Name == "log4net");

			return (XElement)element;
		}

		#endregion
	}
}
