using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] GameObject recordPanel;
    void Awake()
    {
        recordPanel.SetActive(false);        
    }
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
        recordPanel.SetActive(true);
    }
    public void HideRecord()
    {
        recordPanel.SetActive(false);
    }
    public void GameExit()
    {
        Application.Quit();
    }
}
