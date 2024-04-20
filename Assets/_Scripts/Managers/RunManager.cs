using UnityEngine;

namespace RunSettings
{
    public enum RunType
    {
        AB,
        BA
    }
    public static class RunManager
    {
        public static RunType currentRunType;
        public static float timePlayed;

        public static bool activeHud;

        public static void StartRun(RunType runType)
        {
            currentRunType = runType;
            timePlayed = 0;
        }
        
        //If run resets, call this first
        public static void SetTimer(float currentTime)
        {
            timePlayed = currentTime;
        }

        public static float GetTimer()
        {
            return timePlayed;
        }

        public static void SetActive(bool state)
        {
            activeHud = state;
        }
    }
}

