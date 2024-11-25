namespace DATN.MVC.Helpers
{
    public class DateTimeHelper
    {
        public static long DateTimeToUnix(DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
        }

        public static DateTime UnixToDateTime(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
        }

        public static string FormatDateTimeVietnam(DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static string FormatDateTimeSqlServer(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        public static string FormatDateOnly(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        public static string FormatTimeOnly(DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss");
        }
    }
}
