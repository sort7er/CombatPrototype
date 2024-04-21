using RunSettings;
using System;
using TMPro;
using UnityEngine;

public class GameTracking : MonoBehaviour
{
    private RunType currentRunType;
    public TextMeshProUGUI runTypeText;
    public TextMeshProUGUI seconds;
    public TextMeshProUGUI minutes;

    private float timePlayed;

    private bool finished;

    private void Awake()
    {
        currentRunType = RunManager.currentRunType;
        timePlayed = RunManager.timePlayed;

        if(currentRunType == RunType.AB)
        {
            runTypeText.text = "A - B";
            RunManager.SetActive(false);
        }
        else
        {
            runTypeText.text = "B - A";
            RunManager.SetActive(true);
        }

    }


    private void Update()
    {
        if (!finished)
        {
            timePlayed += Time.deltaTime;
            ConvertToMinutesAndSeconds(timePlayed);
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

    public void ConvertToMinutesAndSeconds(float time)
    {
        minutes.text = "<mspace=.9em>" + TimeSpan.FromSeconds(time).Minutes.ToString();
        seconds.text = "<mspace=.9em>" + TimeSpan.FromSeconds(time).Seconds.ToString();
    }


}
