#region Namespace Sonata.Diagnostics.Extensions
//	TODO
#endregion

using Microsoft.Extensions.Logging;
using Sonata.Diagnostics.Logging;

namespace Sonata.Diagnostics.Extensions
{
	/// <summary>
	/// Represents a component adding functionalities to the <see cref="ILoggingBuilder"/> interface.
	/// </summary>
	public static class LoggingBuilderExtension
	{
		#region Methods

		public static ILoggingBuilder AddLog4Net(this ILoggingBuilder factory)
		{
			return AddLog4Net(factory, null);
		}

		public static ILoggingBuilder AddLog4Net(this ILoggingBuilder factory, Log4NetProviderOptions options)
		{
			using (var provider = new Log4NetLoggerProvider(options))
				factory.AddProvider(provider);

			return factory;
		}

		#endregion
	}
}
