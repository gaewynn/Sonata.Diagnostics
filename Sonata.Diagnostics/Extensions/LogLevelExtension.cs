#region Namespace Sonata.Diagnostics.Extensions
//	TODO
#endregion

using Microsoft.Extensions.Logging;
using System;

namespace Sonata.Diagnostics.Extensions
{
	/// <summary>
	/// Represents a component adding functionalities to the <see cref="LogLevel"/> enum.
	/// </summary>
	public static class LogLevelExtension
	{
		#region Methods

		public static string GetStringValue(this LogLevel instance)
		{
			switch (instance)
			{
				case LogLevel.Trace: return "TRACE";
				case LogLevel.Debug: return "DEBUG";
				case LogLevel.Information: return "INFO";
				case LogLevel.Warning: return "WARN";
				case LogLevel.Error: return "ERROR";
				case LogLevel.Critical: return "FATAL";
				case LogLevel.None: return "NONE";
				default:
					throw new ArgumentOutOfRangeException(nameof(instance), instance, null);
			}
		}

		#endregion
	}
}
