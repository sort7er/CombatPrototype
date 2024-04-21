using System;
using TMPro;
using UnityEngine;

public class EndMessage : MonoBehaviour
{
    public TextMeshProUGUI timePlayed, numDeath, dealt, received;
    public GameTracking gameTracking;

    private float endTime;

    public void SetEndInfo()
    {
        endTime = gameTracking.timePlayed;
        string minutes = "<mspace=.9em>" + TimeSpan.FromSeconds(endTime).Minutes.ToString();
        string seconds = "<mspace=.9em>" + TimeSpan.FromSeconds(endTime).Seconds.ToString();

        string finalTime = minutes + " : " + seconds;

        timePlayed.text = "<b>Time played:</b>" + "<i>" + finalTime + "</i>";
    }



}
