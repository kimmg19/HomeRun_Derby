using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    public void RePlay()
    {
        SceneManager.LoadScene(0);
    }
    public void ToMain()
    {
        SceneManager.LoadScene(1);
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
