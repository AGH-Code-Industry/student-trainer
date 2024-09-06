using UnityEngine;

[System.Serializable]
public struct GameTimeData
{
    [Range(0, 23)] public byte hour;
    [Range(0, 59)] public byte minute;

    public readonly uint ToMinutes() => (uint)(hour * 60 + minute);
}