using RunSettings;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTracking : MonoBehaviour
{
    public static GameTracking instance;

    private RunInfo currentRunInfo;
    public TextMeshProUGUI runTypeText;
    public TextMeshProUGUI seconds;
    public TextMeshProUGUI minutes;

    public GameObject HUD, switchMessage, endMessage, deadScreen;
    public TextMeshProUGUI messageText;
    public CameraController cameraController;
    public PauseScreen pauseScreen;
    public Health playerHealth;
    public EndMessage endClass;

    public float timePlayed { get; private set; }
    public int numDeath { get; private set; }
    public int damageDealt { get; private set; }
    public int damageReceived { get; private set; }

    private bool finished;

    private void Awake()
    {
        instance = this;

        currentRunInfo = RunManager.currentRunInfo;
        timePlayed = currentRunInfo.secondsPlayed;
        numDeath = currentRunInfo.numDeath;
        damageDealt = currentRunInfo.damageDealt;
        damageReceived = currentRunInfo.damageReceived;

        playerHealth.OnDeath += PlayerDead;

        if(currentRunInfo.runType == RunType.AB)
        {
            runTypeText.text = "A - B : Permanent";
            RunManager.SetActiveHUD(false);
        }
        else
        {
            runTypeText.text = "B - A : Active";
            RunManager.SetActiveHUD(true);
        }
        ShowHUD();
    }
    private void OnDestroy()
    {
        playerHealth.OnDeath -= PlayerDead;
    }

    private void Update()
    {
        if (!finished)
        {
            timePlayed += Time.deltaTime;
            ConvertToMinutesAndSeconds(timePlayed);
        }
    }
    public void SwitchHUD()
    {
        if(currentRunInfo.runType == RunType.AB)
        {
            runTypeText.text = "A - B : Active";
            ShowMessage("Now the hud will switch to the active variant");
            RunManager.SetActiveHUD(true);
        }
        else
        {
            runTypeText.text = "B - A : Permanent";
            ShowMessage("Now the hud will switch to the permanent variant");
            RunManager.SetActiveHUD(false);
        }
    }
    public void Restart()
    {
        SetCurrentInfo(timePlayed, numDeath, damageDealt, damageReceived);
        RunManager.SetData(currentRunInfo);
    }
    public void Finished()
    {
        finished = true;

        SetCurrentInfo(timePlayed, numDeath, damageDealt, damageReceived);

        OnlyEndMessage();
        endClass.SetEndInfo(currentRunInfo);
        cameraController.DontFollowMouse();
        pauseScreen.CannotPause();
        pauseScreen.SetPausedTimescale();
    }

    public void ConvertToMinutesAndSeconds(float time)
    {
        minutes.text = "<mspace=.9em>" + TimeSpan.FromSeconds(time).Minutes.ToString();
        seconds.text = "<mspace=.9em>" + TimeSpan.FromSeconds(time).Seconds.ToString();
    }

    public void ShowHUD()
    {
        OnlyHUD();
        cameraController.FollowMouse();
        pauseScreen.CanPause();
        pauseScreen.SetNormalTimescale();
    }
    public void ShowMessage(string message)
    {
        OnlySwitchMessage();
        cameraController.DontFollowMouse();
        messageText.text = message;
        pauseScreen.CannotPause();
        pauseScreen.SetPausedTimescale();
    }
    public void PlayerDead()
    {
        cameraController.DontFollowMouse();
        pauseScreen.CannotPause();
        numDeath++;
        OnlyDeadScreen();
        pauseScreen.SetPausedTimescale();
    }
    public void ExitScene()
    {
        SceneManager.LoadScene(0);
    }

    private void OnlyHUD()
    {
        HUD.SetActive(true);
        switchMessage.SetActive(false);
        endMessage.SetActive(false);
        deadScreen.SetActive(false);
    }
    private void OnlySwitchMessage()
    {
        HUD.SetActive(false);
        switchMessage.SetActive(true);
        endMessage.SetActive(false);
        deadScreen.SetActive(false);
    }
    private void OnlyEndMessage()
    {
        HUD.SetActive(false);
        switchMessage.SetActive(false);
        endMessage.SetActive(true);
        deadScreen.SetActive(false);
    }
    private void OnlyDeadScreen()
    {
        HUD.SetActive(false);
        switchMessage.SetActive(false);
        endMessage.SetActive(false);
        deadScreen.SetActive(true);
    }
    public void SetCurrentInfo(float time, int numDeath, int dealt, int received)
    {
        currentRunInfo.secondsPlayed = time;
        currentRunInfo.numDeath = numDeath;
        currentRunInfo.damageDealt = dealt;
        currentRunInfo.damageReceived = received;
    }
    public void AddDamageDealt(int newDamage)
    {
        damageDealt += newDamage;
    }
    public void AddDamageReceived(int newDamage)
    {
        damageReceived += newDamage;
    }
}
