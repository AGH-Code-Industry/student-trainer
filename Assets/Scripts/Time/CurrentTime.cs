using UnityEngine;
using System;

public class CurrentTime
{
    public int Days { get; private set; }
    public int Hours { get; private set; }
    public int Minutes { get; private set; }

    // Constructor
    public CurrentTime(int days, int hours, int minutes)
    {
        Hours = hours;
        Minutes = minutes;
        Days = days;
    }

    public void AddDays(int days)
    {
        Days += days;
    }

    public void AddHours(int hours)
    {
        Hours += hours;
        Hours %= 24;
    }

    public void AddMinutes(int minutes)
    {
        Minutes += minutes;
        Hours += Minutes / 60;
        Minutes %= 60;
        Hours %= 24;
        if (Hours == 0 && Minutes == 0)
            AddDays(1);
    }

    public string GetFormattedTime()
    {
        return FormatTime(Days, Hours, Minutes);
    }

    // Private method for formatting time
    private static string FormatTime(int days, int hours, int minutes)
    {
        return $"{days:D1}:{hours:D2}:{minutes:D2}";
    }

    // Implicit conversion operator
    public static implicit operator string(CurrentTime time)
    {
        return FormatTime(time.Days, time.Hours, time.Minutes);
    }
}
