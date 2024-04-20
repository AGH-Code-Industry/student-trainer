using Chapter.Singleton;
using UnityEngine;
public class TimeManager : Singleton<TimeManager>
{
    public Daytime globalTime;
    public string timePreview = "0:12:00";
    void Start()
    {
        globalTime = new Daytime(0, 12, 0);
        InvokeRepeating("TimePass", 0f, 1f);
    }

    void TimePass()
    {
        globalTime.AddMinutes(1);
        timePreview = globalTime.GetFormattedTime();
    }
}