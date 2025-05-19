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

    // 게임시작 버튼 이벤트
    public void OnGameStart()
    {
        LoadingSceneController.Instance.LoadScene("PlayGround");
    }

    // panel에 캔버스 그룹이 있다면 fader 효과 적용 없으면 그냥 킴
    public void ActiveWindow(GameObject panel)
    {
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            panel.SetActive(true);
            // 초기 설정: 보이지 않지만 활성화된 상태
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
            // 페이드 아웃 시작 시 상호작용 즉시 비활성화
            cg.interactable = false;
            cg.blocksRaycasts = false;
            StartCoroutine(Fader(cg, false));
        }
        else
        {
            panel.SetActive(false); // 이 부분 수정 (false로 변경)
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

        // 애니메이션 완료 후 처리
        if (isFadeIn)
        {
            // 페이드 인 완료 시 상호작용 활성화
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
        else
        {
            // 페이드 아웃 완료 시 패널 비활성화
            cg.gameObject.SetActive(false);
        }
    }
}