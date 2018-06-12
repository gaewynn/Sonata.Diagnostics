#region Namespace Sonata.Diagnostics.Logs
//	TODO
#endregion

using Sonata.Core.Attributes;

namespace Sonata.Diagnostics.Logs
{
	public enum LogType : short
	{
		[StringValue("Technical")]
		Technical = 0,

		[StringValue("Business")]
		Business = 1
	}
}
