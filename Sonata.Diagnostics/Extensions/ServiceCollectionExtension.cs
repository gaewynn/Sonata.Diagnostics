using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sonata.Diagnostics.Logging;

namespace Sonata.Diagnostics.Extensions
{
	public static class ServiceCollectionExtension
	{
		public static IServiceCollection AddLog4Net(this IServiceCollection instance, ILoggerFactory loggerFactory)
		{
			if (instance == null)
				return null;
			if (loggerFactory == null)
				throw new ArgumentNullException(nameof(loggerFactory));

			loggerFactory.AddLog4Net();

			return instance;//.AddSingleton(typeof(ILoggingBuilder), new Log4NetLoggingBuilder(instance));
		}
	}
}
