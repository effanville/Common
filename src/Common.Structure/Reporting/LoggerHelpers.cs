using Microsoft.Extensions.Logging;

namespace Effanville.Common.Structure.Reporting;

public static class LoggerHelpers
{
    public static IReportLogger GetLogger<T>(this ILogger<T> logger) where T : class
    {
        if (logger is InjectableReportLogger injLogger)
        {
            return injLogger.GetInternalLogger();
        }

        return null;
    }

    public static IReportLogger GetLogger(this ILogger logger)
    {
        if (logger is InjectableReportLogger injLogger)
        {
            return injLogger.GetInternalLogger();
        }

        return null;
    }
}