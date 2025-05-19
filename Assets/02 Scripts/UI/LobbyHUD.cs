using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LobbyHUD : MonoBehaviour
{
    [SerializeField] GameObject shopPanel;
    
    void Awake()
    {
        shopPanel.SetActive(false);
    }

    // ���ӽ��� ��ư �̺�Ʈ
    public void OnGameStart()
    {
        LoadingSceneController.Instance.LoadScene("PlayGround");
    }

    // panel�� ĵ���� �׷��� �ִٸ� fader ȿ�� ���� ������ �׳� Ŵ
    public void ActiveWindow(GameObject panel)
    {
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            panel.SetActive(true);
            // �ʱ� ����: ������ ������ Ȱ��ȭ�� ����
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
            // ���̵� �ƿ� ���� �� ��ȣ�ۿ� ��� ��Ȱ��ȭ
            cg.interactable = false;
            cg.blocksRaycasts = false;
            StartCoroutine(Fader(cg, false));
        }
        else
        {
            panel.SetActive(false); // �� �κ� ���� (false�� ����)
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

        // �ִϸ��̼� �Ϸ� �� ó��
        if (isFadeIn)
        {
            // ���̵� �� �Ϸ� �� ��ȣ�ۿ� Ȱ��ȭ
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
        else
        {
            // ���̵� �ƿ� �Ϸ� �� �г� ��Ȱ��ȭ
            cg.gameObject.SetActive(false);
        }
    }
}