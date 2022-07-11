namespace ClickUpService.Utils;

public class SimpleUtils
{
    public static DateTime UnixTimeToDT(long unixtime) =>
        new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddMilliseconds(unixtime).ToLocalTime();
}