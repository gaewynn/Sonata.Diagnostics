#region Namespace Sonata.Diagnostics.Logging
//	TODO
#endregion

using System;
using System.Threading;

namespace Sonata.Diagnostics.Logging
{
	/// <summary>
	/// Default implemenation of <see cref="IExternalScopeProvider"/>
	/// </summary>
	public class LoggerExternalScopeProvider : IExternalScopeProvider
	{
		#region Nested Classes

		private class Scope : IDisposable
		{
			#region Members

			private readonly LoggerExternalScopeProvider _provider;
			private bool _isDisposed;

			#endregion

			#region Properties

			public Scope Parent { get; }

			public object State { get; }

			#endregion

			#region Constructors

			internal Scope(LoggerExternalScopeProvider provider, object state, Scope parent)
			{
				_provider = provider;
				State = state;
				Parent = parent;
			}

			#endregion

			#region Methods

			#region IDisposable Members

			public void Dispose()
			{
				if (!_isDisposed)
				{
					_provider._currentScope.Value = Parent;
					_isDisposed = true;
				}
			}

			#endregion

			#region Object Members

			public override string ToString()
			{
				return State?.ToString();
			}

			#endregion

			#endregion
		}

		#endregion

		#region Members

		private readonly AsyncLocal<Scope> _currentScope = new AsyncLocal<Scope>();

		#endregion

		#region Methods

		#region IExternalScopeProvider Members

		/// <inheritdoc />
		public void ForEachScope<TState>(Action<object, TState> callback, TState state)
		{
			void Report(Scope current)
			{
				if (current == null)
				{
					return;
				}
				Report(current.Parent);
				callback(current.State, state);
			}
			Report(_currentScope.Value);
		}

		/// <inheritdoc />
		public IDisposable Push(object state)
		{
			var parent = _currentScope.Value;
			var newScope = new Scope(this, state, parent);
			_currentScope.Value = newScope;

			return newScope;
		}

		#endregion

		#endregion




	}
}
