#region Namespace Sonata.Diagnostics.Extensions
//	TODO
#endregion

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sonata.Diagnostics.Logging;
using System;

namespace Sonata.Diagnostics.Extensions
{
	/// <summary>
	/// Represents a component adding functionalities to the <see cref="string"/> type.
	/// </summary>
	public static class StringExtension
	{
		#region Methods

		public static ILog4NetProperties ToLog(this string instance)
		{
			return instance == null
				? null
				: JsonConvert.DeserializeObject<ILog4NetProperties>(instance);
		}

		public static T ToLog<T>(this string instance)
		{
			return instance == null
				? default(T)
				: JsonConvert.DeserializeObject<T>(instance);
		}

		public static LogLevel GetEnumValue(this string instance)
		{
			if (String.IsNullOrWhiteSpace(instance))
				return LogLevel.None;

			switch (instance.ToUpper())
			{
				case "NONE": return LogLevel.None;
				case "TRACE": return LogLevel.Trace;
				case "DEBUG": return LogLevel.Debug;
				case "INFO": return LogLevel.Information;
				case "WARN": return LogLevel.Warning;
				case "ERROR": return LogLevel.Error;
				case "FATAL":
				case "CRITICAL":
					return LogLevel.Critical;
				default:
					throw new ArgumentOutOfRangeException(nameof(instance), instance, null);
			}
		}

		#endregion
	}
}
