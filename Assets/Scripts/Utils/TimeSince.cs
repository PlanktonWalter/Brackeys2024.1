public struct TimeSince
{

    public readonly float Time;

    public TimeSince(float time)
    {
        Time = time;
    }

    public static implicit operator float(TimeSince timeSince) => UnityEngine.Time.time - timeSince.Time;

}

public struct TimeUntil
{

    public readonly float Time;

    public TimeUntil(float time)
    {
        Time = time;
    }

    public static implicit operator float(TimeUntil timeSince) => timeSince.Time - UnityEngine.Time.time;

}
