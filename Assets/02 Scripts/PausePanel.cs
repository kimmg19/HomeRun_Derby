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
        SceneManager.LoadScene(0);
    }
    public void OnToMain()
    {
        SceneManager.LoadScene(1);
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
