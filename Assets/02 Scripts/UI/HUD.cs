using System.Collections;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{    

    [SerializeField] TextMeshProUGUI swingChanceText;   //스윙 기회
    [SerializeField] TextMeshProUGUI bestScoreText;     //총 최고점수
    [SerializeField] TextMeshProUGUI currentScoreText;  //총 현재점수
    [SerializeField] TextMeshProUGUI earnedScoreText;   //현재 타격으로 얻은 점수
    [SerializeField] TextMeshProUGUI finalScoreText;   //현재 타격으로 얻은 점수
    

    [SerializeField] TextMeshProUGUI speedText;         //구속
    [SerializeField] TextMeshProUGUI pitchTypeText;     //구종
    [SerializeField] TextMeshProUGUI pitchPosText;      //투구위치

    [SerializeField] TextMeshProUGUI distanceText;      //비거리
    [SerializeField] TextMeshProUGUI homerunText;       //홈런 여부
    [SerializeField] TextMeshProUGUI timingText;        //타격 타이밍
    [SerializeField] TextMeshProUGUI criticalText;      //크리 여부
    [SerializeField] TextMeshProUGUI bigText;

    [SerializeField] GameObject introBox;               //터치후 스타트
    [SerializeField] GameObject scoreBox;               //점수 데이터 UI 박스
    [SerializeField] GameObject pitchDataBox;           //투구 데이터 UI 박스
    [SerializeField] GameObject hitDataBox;             //타격 데이터 UI 박스
    [SerializeField] GameObject gameOverText;           //점수 표기 전 게임오버 텍스트 먼저
    [SerializeField] GameObject gameOverPanel;          //게임오바 최종화면

    int maxCount;
    int bestScore;

    void OnEnable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnSwingCount += ChangeCount;
            EventManager.Instance.OnSetBallData += GetPitchData;
            EventManager.Instance.OnEnablePitchData += ShowPitchData;
            EventManager.Instance.OnGameReady += InitializeUIAtStart;
            EventManager.Instance.OnScoreChanged += UpdateScore;
            EventManager.Instance.OnHitResult += ShowHitResult;
            EventManager.Instance.OnGameFinished += GameOver;

        }
        else Debug.LogError("HUD 이벤트 등록 실패");
    }
    void Awake()
    {
        CheckNull(introBox, nameof(introBox));
        CheckNull(swingChanceText, nameof(swingChanceText));
        CheckNull(bestScoreText, nameof(bestScoreText));
        CheckNull(currentScoreText, nameof(currentScoreText));
        CheckNull(earnedScoreText, nameof(earnedScoreText));
        CheckNull(finalScoreText, nameof(finalScoreText)); // 추가됨
        CheckNull(speedText, nameof(speedText));
        CheckNull(pitchTypeText, nameof(pitchTypeText));
        CheckNull(pitchPosText, nameof(pitchPosText));
        CheckNull(distanceText, nameof(distanceText));
        CheckNull(homerunText, nameof(homerunText));
        CheckNull(timingText, nameof(timingText));
        CheckNull(criticalText, nameof(criticalText));
        CheckNull(bigText, nameof(bigText));
        CheckNull(scoreBox, nameof(scoreBox));
        CheckNull(pitchDataBox, nameof(pitchDataBox));
        CheckNull(hitDataBox, nameof(hitDataBox));
        CheckNull(gameOverText, nameof(gameOverText)); // 추가됨
        CheckNull(gameOverPanel, nameof(gameOverPanel));
    }
    void CheckNull(Object obj, string name)
    {
        if (obj == null)
            Debug.LogError($"{name} 누락");
    }
    void InitializeUIAtStart()
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
        gameOverPanel.SetActive(false);
        introBox.SetActive(true);
        gameOverText.SetActive(false);
        InitDuringGame();
    }
    public void OnIntroTextClicked()
    {
        EventManager.Instance.PublishGameStart();
        introBox.SetActive(false);
    }
    //게임 중 텍스트 초기화
    void InitDuringGame()
    {
        criticalText.text = "";
        bigText.text = "";
        earnedScoreText.text = "";
    }
    void ChangeCount(int count)
    {
        swingChanceText.text = $"{count} / {maxCount}";
    }
    

    void GetPitchData(float speed, EPitchPosition position, EPitchType type)
    {
        speedText.text = $"{speed:F1}KM";
        pitchPosText.text = position.ToString();
        pitchTypeText.text = type.ToString();
    }

    void ShowPitchData(EPitchPosition p)
    {
        pitchDataBox.SetActive(true);
        StartCoroutine(HideHitDataText(pitchDataBox));
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
    //점수 올라가는 효과
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
    void ShowHitResult(bool isHomerun, float distance, EHitTiming timing, int score, bool isCritical, bool isBig)
    {
        if (isCritical) criticalText.text = "+Critical!!";
        if (isBig) bigText.text = "+Big!!!";
        if (score > 0) earnedScoreText.text = $"+{score}";

        if(distance>0)distanceText.text = $"{distance+50:F1}m";
        timingText.text = timing.ToString();

        // 홈런 알림 표시
        if (timing == EHitTiming.Miss)
        {
            homerunText.text = "OOPS!!";
        }
        else if (isHomerun)
        {
            homerunText.color = Color.red;
            homerunText.text = "HOMERUN!!!!";
        }
        else
        {
            homerunText.color = Color.red;
            homerunText.text = "Hit!";
        }
        hitDataBox.SetActive(true);
        scoreBox.SetActive(true);
        StartCoroutine(HideHitDataText(scoreBox));
        StartCoroutine(HideHitDataText(hitDataBox));
    }

    IEnumerator HideHitDataText(GameObject box)
    {
        yield return new WaitForSeconds(3);
        box.SetActive(false);
        InitDuringGame();
    }
    void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }
    IEnumerator GameOverCoroutine()
    {
        gameOverText.SetActive(true);        
        yield return new WaitForSeconds(2);
        gameOverText.SetActive(false);
        gameOverPanel.SetActive(true);
        StartCoroutine(ScoreEffect(finalScoreText,0,ScoreManager.Instance.CurrentScore));
    }
    void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnGameFinished -= GameOver;
            EventManager.Instance.OnSwingCount -= ChangeCount;
            EventManager.Instance.OnSetBallData -= GetPitchData;
            EventManager.Instance.OnEnablePitchData -= ShowPitchData;
            EventManager.Instance.OnGameReady -= InitializeUIAtStart;
            EventManager.Instance.OnScoreChanged -= UpdateScore;
            EventManager.Instance.OnHitResult -= ShowHitResult;
        }
        else Debug.LogError("HUD 이벤트 해제 실패");
    }
}