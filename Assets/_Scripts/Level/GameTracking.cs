using RunSettings;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTracking : MonoBehaviour
{
    private RunType currentRunType;
    public TextMeshProUGUI runTypeText;
    public TextMeshProUGUI seconds;
    public TextMeshProUGUI minutes;

    public GameObject HUD, switchMessage, endMessage, deadScreen;
    public TextMeshProUGUI messageText;
    public CameraController cameraController;
    public PauseScreen pauseScreen;
    public Health playerHealth;

    public float timePlayed { get; private set; }

    private bool finished;

    private void Awake()
    {
        currentRunType = RunManager.currentRunType;
        timePlayed = RunManager.timePlayed;
        playerHealth.OnDeath += PlayerDead;

        if(currentRunType == RunType.AB)
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
        if(currentRunType == RunType.AB)
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
        RunManager.SetTimer(timePlayed);
    }
    public void Finished()
    {
        finished = true;
        ExitMessage();
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
    public void ExitMessage()
    {
        OnlyEndMessage();
        cameraController.DontFollowMouse();
        pauseScreen.CannotPause();
        pauseScreen.SetPausedTimescale();
    }
    public void PlayerDead()
    {
        cameraController.DontFollowMouse();
        pauseScreen.CannotPause();
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

}
