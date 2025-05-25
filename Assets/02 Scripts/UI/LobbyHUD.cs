using System.Collections;
using UnityEngine;

public class LobbyHUD : MonoBehaviour
{
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject questPanel;
    [SerializeField] GameObject settingPanel;

    void Awake()
    {
        shopPanel.SetActive(false);
        questPanel.SetActive(false);
        settingPanel.SetActive(false);
    }

    public void OnGameStart()
    {
        LoadingSceneController.Instance.LoadScene("PlayGround");
    }

    public void ActiveWindow(GameObject panel)
    {
        // shopPanel 활성화할 때 의존성 주입
        if (panel == shopPanel)
        {
            var shopUIManager = panel.GetComponent<ShopUIManager>();
            if (shopUIManager != null)
            {
                shopUIManager.Initialize(PlayerManager.Instance);
            }
        }

        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            panel.SetActive(true);
            cg.alpha = 0f;
            cg.interactable = false;
            cg.blocksRaycasts = false;
            StartCoroutine(Fader(cg, true));
        }
        else
        {
            panel.SetActive(true);
        }
    }

    public void Close(GameObject panel)
    {
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
            StartCoroutine(Fader(cg, false));
        }
        else
        {
            panel.SetActive(false);
        }
    }

    IEnumerator Fader(CanvasGroup cg, bool isFadeIn)
    {
        float timer = 0f;
        while (timer < 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 3f;
            cg.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if (isFadeIn)
        {
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
        else
        {
            cg.gameObject.SetActive(false);
        }
    }
}