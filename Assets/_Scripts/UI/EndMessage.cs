using TMPro;
using UnityEngine;
using System.IO;

public class EndMessage : MonoBehaviour
{
    public TextMeshProUGUI timePlayed, numDeath, dealt, received;

    private string path;

    public void SetEndInfo(RunInfo runinfo)
    {
        timePlayed.text = "<i>" + runinfo.GetTimeInMinutesAndSeconds() + "</i>"; 
        numDeath.text = "<i>" + runinfo.numDeath.ToString() + "</i>";
        dealt.text = "<i>" + runinfo.damageDealt.ToString() + "</i>";
        received.text = "<i>" + runinfo.damageReceived.ToString() + "</i>";

        path = "/Playtests/" + runinfo.runType.ToString() + "/";

        Directory.CreateDirectory(Application.streamingAssetsPath + path);
        CreateTextFile(runinfo);
    }


    public void CreateTextFile(RunInfo runInfo)
    {

        int count = Tools.DirCount(Application.streamingAssetsPath + path);
        string txtDocumentName = Application.streamingAssetsPath + path + "Playtest" + count.ToString() + ".txt";

        if(File.Exists(txtDocumentName))
        {
            File.WriteAllText(txtDocumentName, "Playtest session \n\n");
        }

        string textInDocument = "Time played: " + runInfo.GetTimeInMinutesAndSecondsNoFormat() + " \n " +
            "Number of deaths: " + runInfo.numDeath.ToString() + " \n " +
            "Damage dealt: " + runInfo.damageDealt.ToString() + " \n " +
            "Damage received: " + runInfo.damageReceived.ToString();

        File.AppendAllText(txtDocumentName, textInDocument + " \n ");
    }



}
