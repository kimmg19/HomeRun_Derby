using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyHUD : MonoBehaviour
{
    
    public void OnGameStart()
    {
        LoadingSceneController.Instance.LoadScene("PlayGround");
    }

    public void ActiveWindow(GameObject panel)
    {
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha =0f;
            panel.SetActive(true);
            StartCoroutine(Fader(cg));
        }
        else
        {
            panel.SetActive(true);
        }
    }
    public void Close(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
    IEnumerator Fader(CanvasGroup cg)
    {
        float timer = 0f;
        while (timer < 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 3f;
            cg.alpha = Mathf.Lerp(0f, 1f, timer);
        }
        
    }
}
