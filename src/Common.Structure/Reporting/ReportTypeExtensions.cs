namespace Effanville.Common.Structure.Reporting;

public static class ReportTypeExtensions
{
    public static string ToLogString(this ReportType me)
    {
        switch(me)
        {
            case ReportType.Error:
                return "ERR";
            case ReportType.Warning:
                return "WRN";
            case ReportType.Information:
                return "INF";
            default:
                return null;
        }
    }
}