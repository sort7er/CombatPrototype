using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public InputReader inputReader;
    public GameTracking gameTracking;
    public CameraController cameraController;
    public bool pause { get; private set; }

    public bool canPause { get; private set; }

    private void Awake()
    {
        CanPause();
        inputReader.OnPause += CheckPause;
        Unpause();
    }
    private void OnDestroy()
    {
        inputReader.OnPause -= CheckPause;
    }
    private void CheckPause()
    {
        if (canPause)
        {
            if (pause)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Pause()
    {
        cameraController.DontFollowMouse();
        gameObject.SetActive(true);
        pause= true;
        SetPausedTimescale();
    }
    public void Unpause()
    {
        cameraController.FollowMouse();
        gameTracking.ShowHUD();
        gameObject.SetActive(false);
        pause = false;
        SetNormalTimescale();
    }

    public void Resume()
    {
        CheckPause();
    }
    public void Restart()
    {
        gameTracking.Restart();
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
    public void CanPause()
    {
        canPause= true;
    }
    public void CannotPause()
    {
        canPause= false;
    }
    public void SetNormalTimescale()
    {
        Time.timeScale = 1;
    }
    public void SetPausedTimescale()
    {
        Time.timeScale = 0;
    }
}
