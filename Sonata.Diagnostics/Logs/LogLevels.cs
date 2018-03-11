#region Namespace Sonata.Diagnostics.Logs
//	TODO: comment
#endregion

using Sonata.Core.Attributes;

namespace Sonata.Diagnostics.Logs
{
	/// <summary>
	/// The LogLevel allows you to control the verbosity of the logging output from your application.
	/// </summary>
	public enum LogLevels : short
	{
		/// <summary>
		/// Used for the most detailed log messages, typically only valuable to a developer debugging an issue.
		/// These messages may contain sensitive application data and so should not be enabled in a production environment. 
		/// </summary>
		/// <example>Credentials: {"User":"someuser", "Password":"P@ssword"}</example>
		[StringValue("DEBUG")]
		Debug = 0,

		/// <summary>
		/// These messages are used to track the general flow of the application.
		/// These logs should have some long term value, as opposed to Verbose level messages, which do not.
		/// </summary>
		/// <example>Request received for path /foo</example>
		[StringValue("INFO")]
		Info = 1,

		/// <summary>
		/// The Warning level should be used for abnormal or unexpected events in the application flow.
		/// These may include errors or other conditions that do not cause the application to stop, but which may need to be investigated in the future.
		/// Handled exceptions are a common place to use the Warning log level.
		/// </summary>
		/// <example>Login failed for IP 127.0.0.1 or FileNotFoundException for file foo.txt</example>
		[StringValue("WARNING")]
		Warning = 2,

		/// <summary>
		/// An error should be logged when the current flow of the application must stop due to some failure, such as an exception that cannot be handled or recovered from.
		/// These messages should indicate a failure in the current activity or operation (such as the current HTTP request), not an application-wide failure
		/// </summary>
		/// <example>Cannot insert record due to duplicate key violation</example>
		[StringValue("ERROR")]
		Error = 3,

		/// <summary>
		/// A fatal log level should be reserved for unrecoverable application or system crashes, or catastrophic failure that requires immediate attention.
		/// </summary>
		/// <example>data loss scenarios, stack overflows, out of disk space</example>
		[StringValue("FATAL")]
		Fatal = 4
	}
}
