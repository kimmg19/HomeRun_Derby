using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    static LoadingSceneController instance;

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Image progressBar;
    [SerializeField] string loadSceneName;
    [SerializeField] ToolTipSO tooltipSO;
    [SerializeField] TextMeshProUGUI tooltipText;
    public static LoadingSceneController Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindAnyObjectByType<LoadingSceneController>();
                if (obj != null) instance = obj;
                else instance = Create();
            }
            return instance;
        }
    }

    static LoadingSceneController Create()
    {
        return Instantiate(Resources.Load<LoadingSceneController>("LoadingCanvas"));
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);
        tooltipText.text = tooltipSO.GetToolTip();
        SceneManager.sceneLoaded += OnSceneLoaded;
        loadSceneName = sceneName;
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        progressBar.fillAmount = 0f;
        yield return StartCoroutine(Fade(true));
        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        op.allowSceneActivation = false;
        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;
            if (op.progress < 0.9f) progressBar.fillAmount = op.progress;
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if (progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    void OnSceneLoaded(Scene currentScene, LoadSceneMode none)
    {
        if (currentScene.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        SoundManager.Instance.StopBgm();
    }
    IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;
        while (timer < 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 3f;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }
        if (!isFadeIn) gameObject.SetActive(false);
    }
}

