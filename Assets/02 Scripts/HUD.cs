using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI introText;
    [SerializeField] TextMeshProUGUI swingChanceText;
    [SerializeField] TextMeshProUGUI bestScoreText;
    [SerializeField] TextMeshProUGUI currentScoreText;
    [SerializeField] GameObject DataBox;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI pitchTypeText;
    [SerializeField] TextMeshProUGUI pitchPosText;
    [SerializeField] GameObject homerunText; // 홈런 알림 UI (선택사항)
    [SerializeField] TextMeshProUGUI distanceText; // 거리 표시용 텍스트 (선택사항)

    int maxCount;
    int bestScore;

    void OnEnable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnSwingCount += ChangeCount;
            EventManager.Instance.OnGameStart += DisableIntro;
            EventManager.Instance.OnBallReleased += GetBallData;
            EventManager.Instance.EnableBallData += EnableBallData;
            EventManager.Instance.OnGameReady += InitializeUI;

            // 점수 관련 이벤트 구독
            EventManager.Instance.OnScoreChanged += UpdateScore;
            EventManager.Instance.OnBestScoreChanged += UpdateBestScore;
            EventManager.Instance.OnHomerunResult += ShowHomerunResult;
        }
        else Debug.LogError("HUD 이벤트 등록 실패");
    }

    void Start()
    {
        maxCount = HomerunDerbyManager.Instance != null ?
            HomerunDerbyManager.Instance.SwingCount : 15;
        DataBox.SetActive(false);

        // 최고 점수 로드
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (bestScoreText != null)
            bestScoreText.text = $"{bestScore}";

        if (currentScoreText != null)
            currentScoreText.text = "0";

        if (homerunText != null)
            homerunText.SetActive(false);
    }

    void InitializeUI()
    {
        // 게임 시작시 UI 초기화
        if (currentScoreText != null)
            currentScoreText.text = "0";

        if (bestScoreText != null)
            bestScoreText.text = $"{bestScore}";

        if (distanceText != null)
            distanceText.text = "";
    }

    void ChangeCount(int count)
    {
        swingChanceText.text = $"{count} / {maxCount}";
    }

    void DisableIntro()
    {
        introText.enabled = false;
    }

    void GetBallData(float speed, EPitchPosition position, EPitchType type)
    {
        speedText.text = $"{speed:F1}KM";
        pitchPosText.text = position.ToString();
        pitchTypeText.text = type.ToString();
    }

    void EnableBallData(bool b)
    {
        DataBox.SetActive(b);
    }

    // 현재 점수 업데이트
    void UpdateScore(int score)
    {
        if (currentScoreText != null)
            currentScoreText.text = $"{score}";
    }

    // 최고 점수 업데이트
    void UpdateBestScore(int score)
    {
        bestScore = score;
        if (bestScoreText != null)
            bestScoreText.text = $"{bestScore}";
    }

    // 홈런 결과 표시
    void ShowHomerunResult(bool isHomerun, float distance)
    {
        // 거리 표시
        if (distanceText != null)
            distanceText.text = $"{distance:F1}m";

        // 홈런 알림 표시
        if (homerunText != null && isHomerun)
        {
            homerunText.SetActive(true);
            Invoke("HideHomerunText", 2.0f);
        }
    }

    void HideHomerunText()
    {
        if (homerunText != null)
            homerunText.SetActive(false);
    }

    void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnSwingCount -= ChangeCount;
            EventManager.Instance.OnGameStart -= DisableIntro;
            EventManager.Instance.OnBallReleased -= GetBallData;
            EventManager.Instance.EnableBallData -= EnableBallData;
            EventManager.Instance.OnGameReady -= InitializeUI;

            // 점수 관련 이벤트 구독 해제
            EventManager.Instance.OnScoreChanged -= UpdateScore;
            EventManager.Instance.OnBestScoreChanged -= UpdateBestScore;
            EventManager.Instance.OnHomerunResult -= ShowHomerunResult;
        }
        else Debug.LogError("HUD 이벤트 해제 실패");
    }
}