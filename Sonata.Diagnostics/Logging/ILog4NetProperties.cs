#region Namespace Sonata.Diagnostics.Logging
//	TODO
#endregion

using Microsoft.Extensions.Logging;
using System;

namespace Sonata.Diagnostics.Logging
{
	public interface ILog4NetProperties
	{
		#region Properties

		string UserName { get; set; }

		string Code { get; set; }

		string Thread { get; set; }

		LogLevel Level { get; set; }

		string Source { get; set; }

		string Message { get; set; }

		Exception Exception { get; set; }

		#endregion
	}
}
