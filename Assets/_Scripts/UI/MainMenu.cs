using UnityEngine;
using UnityEngine.SceneManagement;
using RunSettings;

public class MainMenu : MonoBehaviour
{
    public void PlayAB()
    {
        RunManager.StartRun(RunType.AB);
        GameScene();
    }
    public void PlayBA()
    {
        RunManager.StartRun(RunType.BA);
        GameScene();
    }
    private void GameScene()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
