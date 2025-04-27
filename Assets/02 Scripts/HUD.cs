using System.Collections;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI introText;         //터치후 스타트 텍스트
    [SerializeField] TextMeshProUGUI swingChanceText;   //스윙 기회
    [SerializeField] TextMeshProUGUI bestScoreText;     //총 최고점수
    [SerializeField] TextMeshProUGUI currentScoreText;  //총 현재점수
    [SerializeField] TextMeshProUGUI earnedScoreText;   //현재 타격으로 얻은 점수

    [SerializeField] TextMeshProUGUI speedText;         //구속
    [SerializeField] TextMeshProUGUI pitchTypeText;     //구종
    [SerializeField] TextMeshProUGUI pitchPosText;      //투구위치

    [SerializeField] TextMeshProUGUI distanceText;      //비거리
    [SerializeField] TextMeshProUGUI homerunText;       //홈런 여부
    [SerializeField] TextMeshProUGUI timingText;        //타격 타이밍
    [SerializeField] TextMeshProUGUI criticalText;      //크리 여부

    [SerializeField] GameObject scoreBox;
    [SerializeField] GameObject pitchDataBox;           //투구 데이터 UI
    [SerializeField] GameObject hitDataBox;             //타격 데이터 UI

    int maxCount;
    int bestScore;

    void OnEnable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnSwingCount += ChangeCount;
            EventManager.Instance.OnGameStart += DisableIntro;
            EventManager.Instance.OnBallReleased += GetPitchData;
            EventManager.Instance.EnableBallData += EnableBallData;
            EventManager.Instance.OnGameReady += InitializeUI;
            EventManager.Instance.OnScoreChanged += UpdateScore;
            EventManager.Instance.OnHitResult += ShowHitResult;
        }
        else Debug.LogError("HUD 이벤트 등록 실패");
    }
    void Awake()
    {
        if (introText == null) return;
        if (swingChanceText == null) return;
        if (bestScoreText == null) return;
        if (currentScoreText == null) return;
        if (earnedScoreText == null) return;
        if (speedText == null) return;
        if (pitchTypeText == null) return;
        if (pitchPosText == null) return;
        if (distanceText == null) return;
        if (homerunText == null) return;
        if (pitchDataBox == null) return;
        if (hitDataBox == null) return;
        if(criticalText==null) return;
        if(scoreBox == null) return;
    }


    void InitializeUI()
    {
        // 게임 시작시 UI 초기화
        currentScoreText.text = "0";
        bestScoreText.text = $"{bestScore}";        
        bestScore = PlayerPrefs.GetInt("BestScore", 0); // 최고 점수 로드
        bestScoreText.text = $"{bestScore}";
        maxCount = HomerunDerbyManager.Instance != null ?
            HomerunDerbyManager.Instance.SwingCount : 15;
        pitchDataBox.SetActive(false);
        hitDataBox.SetActive(false);
        scoreBox.SetActive(false);        
    }

    void ChangeCount(int count)
    {
        swingChanceText.text = $"{count} / {maxCount}";
    }

    void DisableIntro()
    {
        introText.enabled = false;
    }

    void GetPitchData(float speed, EPitchPosition position, EPitchType type)
    {
        speedText.text = $"{speed:F1}KM";
        pitchPosText.text = position.ToString();
        pitchTypeText.text = type.ToString();
    }

    void EnableBallData(bool b)
    {
        pitchDataBox.SetActive(b);
    }

    // 현재 점수 업데이트. 현재 점수(타격 직전까지의), 타격 후 점수, 타격으로 얻은 점수 순.
    void UpdateScore(int currentScore, int targetScore)
    {
        if (currentScore >= bestScore)
        {
            StartCoroutine(ScoreEffect(bestScoreText, currentScore, targetScore));
        }
        StartCoroutine(ScoreEffect(currentScoreText, currentScore, targetScore));
    }
    IEnumerator ScoreEffect(TextMeshProUGUI scoreText, float currentScore, float targetScore)
    {
        float duration = 0.8f;
        float offset = (targetScore - currentScore) / duration;
        while (currentScore < targetScore)
        {
            currentScore += offset * Time.deltaTime;
            scoreText.text = ((int)currentScore).ToString();
            yield return null;
        }
        currentScore = targetScore;
        scoreText.text = ((int)currentScore).ToString();
    }

    // 타격 결과 표시
    void ShowHitResult(bool isHomerun, float distance, EHitTiming timing, int score, bool isCritical)
    {
        if (isCritical)  criticalText.text = "+Critical!!"; 
        else criticalText.text = "";
        distanceText.text = $"{distance:F1}m";
        timingText.text = timing.ToString();
        earnedScoreText.text = $"+{score}";
        // 홈런 알림 표시
        if (isHomerun)
        {
            homerunText.color = Color.red;
            homerunText.text = "HOMERUN!!!!";
        }
        else
        {
            homerunText.color = Color.red;
            homerunText.text = "Hit!";
        }
        scoreBox.SetActive(true);
        hitDataBox.SetActive(true);
        Invoke("HideHitDataText", 3.0f);
    }

    void HideHitDataText()
    {
        hitDataBox.SetActive(false);
        scoreBox.SetActive(false);

    }

    void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnSwingCount -= ChangeCount;
            EventManager.Instance.OnGameStart -= DisableIntro;
            EventManager.Instance.OnBallReleased -= GetPitchData;
            EventManager.Instance.EnableBallData -= EnableBallData;
            EventManager.Instance.OnGameReady -= InitializeUI;
            EventManager.Instance.OnScoreChanged -= UpdateScore;
            EventManager.Instance.OnHitResult -= ShowHitResult;
        }
        else Debug.LogError("HUD 이벤트 해제 실패");
    }
}