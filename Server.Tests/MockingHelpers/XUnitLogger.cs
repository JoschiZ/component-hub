using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Server.Tests.MockingHelpers;

/// <summary>
/// Logger implementation to log messages to the output helper
/// </summary>
/// <typeparam name="T"></typeparam>
internal sealed class XunitLogger<T> : ILogger<T>, IDisposable
{
    private ITestOutputHelper _output;

    public XunitLogger(ITestOutputHelper output)
    {
        _output = output;
    }
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter)
    {
        _output.WriteLine("XUnitLogger: " + state);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return this;
    }

    public void Dispose()
    {
    }
}