#region Namespace Sonata.Diagnostics.Logging
//	TODO
#endregion

using Sonata.Core.Attributes;
using System;

namespace Sonata.Diagnostics.Logging
{
	[Obsolete]
	public enum LogType : short
	{
		[StringValue("Technical")]
		Technical = 0,

		[StringValue("Business")]
		Business = 1
	}
}
