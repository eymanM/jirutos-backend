using AutoMapper.Configuration.Conventions;

namespace Foundation.Utils;

public class TimeSpanString
{
    const int WEEK_WORK_HOURS = 40;
    public static string TSpanToSpanStr(TimeSpan ts)
    {
        string weeks, days, hours, minutes;

        int weeksTemp = (ts.Days / 7) > 0 ? ts.Days / 7 : 0;
        weeks = weeksTemp > 0 ? Convert.ToString(weeksTemp) + "w" : "";

        int daysTemp = weeksTemp > 0 ? ts.Days / (7 * weeksTemp) : ts.Days;
        days = daysTemp > 0 ? Convert.ToString(daysTemp) + "d" : "";

        hours = ts.Hours > 0 ? Convert.ToString(ts.Hours) + "h" : "";

        minutes = ts.Minutes > 0 ? Convert.ToString(ts.Minutes) + "m" : "";
        string res = "";

        foreach (string item in new string[] { weeks, days, hours, minutes })
            if (!string.IsNullOrEmpty(item)) res += (item + " ");

        return res is "" ? "1m" : res.Trim();
    }

    public static string TSpanToWorkSpanStr(TimeSpan ts)
    {
        string weeks, days, hours, minutes;

        int weeksTemp = (int)(ts.TotalHours / WEEK_WORK_HOURS > 0 ? (int)ts.TotalHours / WEEK_WORK_HOURS : 0);
        weeks = weeksTemp > 0 ? Convert.ToString(weeksTemp) + "w" : "";

        int leftedHours = (int)(ts.TotalHours - (WEEK_WORK_HOURS * weeksTemp));

        int daysTemp = (leftedHours / 8) > 0 ? (leftedHours / 8) : 0;
        days = daysTemp > 0 ? Convert.ToString(daysTemp) + "d" : "";

        leftedHours -= daysTemp * 8;

        hours = leftedHours > 0 ? Convert.ToString(leftedHours) + "h" : "";

        minutes = ts.Minutes > 0 ? Convert.ToString(ts.Minutes) + "m" : "";
        string res = "";

        foreach (string item in new string[] { weeks, days, hours, minutes })
            if (!string.IsNullOrEmpty(item)) res += (item + " ");

        return res is "" ? "1m" : res.Trim();
    }

    public static TimeSpan SpanStrToTSpan(string span)
    {
        int weeks = 0, days = 0, hours = 0, minutes = 0;

        span = span.Trim();
        var weekIndex = span.IndexOf("w");
        if (weekIndex > 0)
        {
            weeks = Convert.ToInt32(span.Substring(0, weekIndex).Trim());
            span = span.Remove(0, weekIndex + 1);
        }

        span = span.Trim();
        var dayIndex = span.IndexOf("d");
        if (dayIndex > 0)
        {
            days = Convert.ToInt32(span.Substring(0, dayIndex).Trim());
            span = span.Remove(0, dayIndex + 1);
        }

        span = span.Trim();
        var hourIndex = span.IndexOf("h");
        if (hourIndex > 0)
        {
            hours = Convert.ToInt32(span.Substring(0, hourIndex).Trim());
            span = span.Remove(0, hourIndex + 1);
        }

        span = span.Trim();
        var minuteIndex = span.IndexOf("m");
        if (minuteIndex > 0) minutes = Convert.ToInt32(span.Substring(0, minuteIndex).Trim());

        return new TimeSpan(weeks * 7 + days, hours, minutes, 0);
    }
}