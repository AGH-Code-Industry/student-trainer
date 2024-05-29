using UnityEngine;
using System;


[System.Serializable]
public class Daytime
{
    [HideInInspector]
    public int Days;
    public int Hours;
    public int Minutes;

    // Constructor
    public Daytime(int days, int hours, int minutes)
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

    public void AddDaytime(Daytime time)
    {
        AddHours(time.Hours);
        AddMinutes(time.Minutes);
    }

    public void AddRandomDelay(Daytime offset)
    {
        AddMinutes(UnityEngine.Random.Range(0, offset.Hours * 60 + offset.Minutes + 1));
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


    public int CompareTo(Daytime other)
    {
        // Calculate total elapsed time in minutes
        int totalMinutesThis = Days * 24 * 60 + Hours * 60 + Minutes;
        int totalMinutesOther = other.Days * 24 * 60 + other.Hours * 60 + other.Minutes;

        // Compare the total elapsed time
        return totalMinutesThis.CompareTo(totalMinutesOther);
    }
    public string GetFormattedTime()
    {
        return FormatTime(Days, Hours, Minutes);
    }
    public bool Equals(Daytime other)
    {
        return other.GetFormattedTime() == GetFormattedTime();
    }
    // Private method for formatting time
    private static string FormatTime(int days, int hours, int minutes)
    {
        return $"{days:D1}:{hours:D2}:{minutes:D2}";
    }

    // Implicit conversion operator
    public static implicit operator string(Daytime time)
    {
        return FormatTime(time.Days, time.Hours, time.Minutes);
    }
}
