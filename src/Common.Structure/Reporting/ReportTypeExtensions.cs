namespace Effanville.Common.Structure.Reporting;

/// <summary>
/// Contains extension methods for a <see cref="ReportType"/>
/// </summary>
public static class ReportTypeExtensions
{
    /// <summary>
    /// Provide a short string representation for the <see cref="ReportType"/> enum.
    /// </summary>
    public static string ToLogString(this ReportType me)
    {
        return me switch
        {
            ReportType.Error => "ERR",
            ReportType.Warning => "WRN",
            ReportType.Information => "INF",
            ReportType.Debug => "DBG",
            _ => null,
        };
    }
}