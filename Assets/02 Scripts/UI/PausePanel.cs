using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    
    [SerializeField] GameObject pausePanel;
    void Awake()
    {
        pausePanel.SetActive(false);
    }
    public void OnPause()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }
    public void OnResume()
    {
        Time.timeScale = 1.0f;
        pausePanel.SetActive(false);
    }
    public void OnRePlay()
    {
        LoadingSceneController.Instance.LoadScene("PlayGround");
        Time.timeScale = 1.0f;
    }
    public void OnToMain()
    {
        LoadingSceneController.Instance.LoadScene("Lobby");
        Time.timeScale = 1.0f;
    }
    public void OnSetting()
    {
        print("¼³Á¤");
    }
    public void OnGameExit()
    {
        Application.Quit();
    }
}
