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
        public static bool activeHud;
        public static RunInfo currentRunInfo;

        public static void StartRun(RunType runType)
        {
            currentRunInfo = new RunInfo(runType, 0, 0, 0, 0);
        }
        
        //If run resets, call this first
        public static void SetData(RunInfo runInfo)
        {
            currentRunInfo = runInfo;
        }

        public static RunInfo GetTimer()
        {
            return currentRunInfo;
        }

        public static void SetActiveHUD(bool state)
        {
            activeHud = state;
        }
    }
}

