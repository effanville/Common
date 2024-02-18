using System;

namespace Effanville.Common.Structure.Extensions
{
    /// <summary>
    /// Miscellaneous custom extension methods for the <see cref="DateTime"/> class.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Outputs a date in the UK format (the good format) from a datetime.
        /// </summary>
        public static string ToUkDateString(this DateTime date)
        {
            return date.Day + "/" + date.Month + "/" + date.Year;
        }

        /// <summary>
        /// Outputs a date in the UK format (the good format) from a datetime
        /// where single digit days and months are padded to be 2 digit.
        /// </summary>
        public static string ToUkDateStringPadded(this DateTime Day)
        {
            return Day.Day.ToString().PadLeft(2, '0') + "/" + Day.Month.ToString().PadLeft(2, '0') + "/" + Day.Year;
        }

        /// <summary>
        /// Returns a string representation of the DateTime in a format suitable to be 
        /// used in filenames.
        /// </summary>
        public static string FileSuitableDateTimeValue(this DateTime dateTime)
        {
            return dateTime.Year.ToString() + dateTime.Month.ToString() + dateTime.Day.ToString() + "-" + dateTime.Hour.ToString() + dateTime.Minute.ToString("D2");
        }

        /// <summary>
        /// Returns a string representation of the DateTime date part only in a 
        /// format suitable to be used in filenames.
        /// </summary>
        public static string FileSuitableUKDateString(this DateTime date)
        {
            return date.Year.ToString() + date.Month.ToString() + date.Day.ToString();
        }
    }
}
