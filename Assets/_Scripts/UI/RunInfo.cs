using RunSettings;
using System;

public class RunInfo
{
    public RunType runType;
    public float secondsPlayed;
    public int numDeath;
    public int damageDealt;
    public int damageReceived;

    public RunInfo(RunType runType, float secondsPlayed, int numDeath, int damageDealt, int damageReceived)
    {
        this.runType = runType;
        this.secondsPlayed = secondsPlayed;
        this.numDeath = numDeath;
        this.damageDealt = damageDealt;
        this.damageReceived = damageReceived;
    }

    public string GetTimeInMinutesAndSeconds()
    {
        string minutes = "<mspace=.9em>" + TimeSpan.FromSeconds(secondsPlayed).Minutes.ToString();
        string seconds = "<mspace=.9em>" + TimeSpan.FromSeconds(secondsPlayed).Seconds.ToString();

        return minutes + " : " + seconds;
    }
    public string GetTimeInMinutesAndSecondsNoFormat()
    {
        string minutes = TimeSpan.FromSeconds(secondsPlayed).Minutes.ToString();
        string seconds = TimeSpan.FromSeconds(secondsPlayed).Seconds.ToString();

        return minutes + " : " + seconds;
    }
}
