using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using System;
using System.Collections.Generic;

namespace Functions.Tests
{
  public class ListLogger : ILogger
  {
    public IList<string> Logs;

    // Scopes add context to your logging.  In this case, the test just points to
    // the static instance on the NullScope class to allow the test to function.
    public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

    // A default value of false is provided.
    public bool IsEnabled(LogLevel logLevel) => false;

    public ListLogger()
    {
      this.Logs = new List<string>();
    }

    // This method uses the provided formatter function to format the message
    // and then adds teh resulting text ot the Logs collection.
    public void Log<TState>(LogLevel logLevel,
                            EventId eventId,
                            TState state,
                            Exception exception,
                            Func<TState, Exception, string> formatter)
    {
      string message = formatter(state, exception);
      this.Logs.Add(message);
    }
  }
}