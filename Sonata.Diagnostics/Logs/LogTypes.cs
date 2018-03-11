#region Namespace Sonata.Diagnostics.Logs
//	TODO: comment
#endregion

using Sonata.Core.Attributes;

namespace Sonata.Diagnostics.Logs
{
	public enum LogTypes : short
	{
		[StringValue("Technical")]
		Technical = 0,

		[StringValue("Business")]
		Business = 1
	}
}
