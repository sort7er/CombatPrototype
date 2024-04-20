using RunSettings;
using UnityEngine;

public class GameTracking : MonoBehaviour
{
    private RunType currentRunType;

    private float timePlayed;

    private bool finished;

    private void Awake()
    {
        currentRunType = RunManager.currentRunType;
        timePlayed = RunManager.timePlayed;

        if(currentRunType == RunType.AB)
        {
            RunManager.SetActive(false);
        }
        else
        {
            RunManager.SetActive(true);
        }

    }


    private void Update()
    {
        if (!finished)
        {
            timePlayed += Time.deltaTime;
        }
    }

    public void Restart()
    {
        RunManager.SetTimer(timePlayed);
    }
    public void Finished()
    {
        finished = true;
        Debug.Log("Time played: " + timePlayed);
    }


}
