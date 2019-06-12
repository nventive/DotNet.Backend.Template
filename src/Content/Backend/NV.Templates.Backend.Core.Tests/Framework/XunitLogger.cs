using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace NV.Templates.Backend.Core.Tests.Framework
{
    /// <summary>
    /// <see cref="ILogger{TCategoryName}"/> implementation that outputs to xunit <see cref="ITestOutputHelper"/>.
    /// </summary>
    /// <typeparam name="T">The service type</typeparam>
    public sealed class XunitLogger<T> : ILogger<T>
    {
        private ITestOutputHelper _output;

        public XunitLogger(ITestOutputHelper output)
        {
            _output = output;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _output.WriteLine(state.ToString());
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new NoopDisposable();
        }

        private class NoopDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}
