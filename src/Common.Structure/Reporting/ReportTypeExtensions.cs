namespace Effanville.Common.Structure.Reporting;

public static class ReportTypeExtensions
{
    public static string ToLogString(this ReportType me)
    {
        switch(me)
        {
            case ReportType.Error:
                return "Error";
            case ReportType.Warning:
                return "Warn ";
            case ReportType.Information:
                return "Info ";
            default:
                return null;
        }
    }
}