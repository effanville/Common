using Microsoft.Extensions.Logging;

namespace Effanville.Common.Structure.Reporting.LogAspect;

/// <summary>
/// Interface to enable logging with the class logger 
/// </summary>
public interface ILogInterceptable
{
    /// <summary>
    /// The logger to use in intercepting
    /// </summary>
    ILogger Logger { get; }
}