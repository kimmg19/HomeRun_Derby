using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    
    public void RePlay()
    {
        LoadingSceneController.Instance.LoadScene("PlayGround");
    }
    public void ToMain()
    {
        LoadingSceneController.Instance.LoadScene("Lobby");

    }
    public void ShowRecord()
    {
        print("±‚∑œ√¢");
    }
    public void GameExit()
    {
        Application.Quit();
    }
}
