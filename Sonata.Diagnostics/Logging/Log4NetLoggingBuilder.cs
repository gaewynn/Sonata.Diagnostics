using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Sonata.Diagnostics.Logging
{
	internal class Log4NetLoggingBuilder : ILoggingBuilder
	{
		public Log4NetLoggingBuilder(IServiceCollection services)
		{
			

			Services = services;
		}

		public IServiceCollection Services { get; }
	}
}
