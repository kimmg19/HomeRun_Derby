using UnityEngine;

/// <summary>
/// Unity 라이프사이클 및 이벤트 처리를 담당하는 MonoBehaviour 클래스
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    // Inspector에서 조정 가능한 값들
    [Header("점수 설정")]
    [SerializeField, ReadOnly] int baseScore = 1000;//기본 점수
    [SerializeField, ReadOnly] float distanceMultiplier = 2.0f;//비거리 계수
    [SerializeField, ReadOnly] float homerunDistance = 50f;//홈런 비거리 기준
    [SerializeField, ReadOnly] float longDistanceThreshold = 70f;//대형 홈런 추가점수 기준

    // 점수 변수
    public int CurrentScore { get; set; }
    public int BestScore { get; set; }

    // 계산기 인스턴스
    ScoreCalculator calculator;

    void Awake()
    {
        // 싱글톤 패턴
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
       
        calculator = new ScoreCalculator();        
        BestScore = PlayerPrefs.GetInt("BestScore", 0);
    }

    void OnEnable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnBallHit += ProcessHit;
            EventManager.Instance.OnGameFinished += SaveScore;
            EventManager.Instance.OnGameReady += ResetScore;
        }
        else Debug.LogError("ScoreManager 이벤트 등록 실패");
    }

    // 타격 처리 및 점수 계산- 타격 성공 시에만
    void ProcessHit(EHitTiming timing, float distance)
    {
        bool isHomerun = calculator.IsHomerun(distance, homerunDistance);   // 홈런 판정
        int score = calculator.CalculateScore(                              // 점수 계산
            timing, distance, isHomerun,
            baseScore, distanceMultiplier, longDistanceThreshold
        );
        // 홈런 이벤트 발생
        EventManager.Instance.PublishHitResult(isHomerun, distance, timing,score);

        // 점수 추가
        AddScore(score);
    }

    // 점수 추가
    void AddScore(int points)
    {
        int lastScore=CurrentScore;
        CurrentScore += points;

        if (CurrentScore > BestScore)
        {
            BestScore = CurrentScore;
        }
        EventManager.Instance.PublishScoreChanged(lastScore, CurrentScore);

    }

    // 점수 저장
    void SaveScore()
    {
        PlayerPrefs.SetInt("BestScore", BestScore);
        PlayerPrefs.Save();
    }

    // 점수 초기화
    void ResetScore()
    {
        CurrentScore = 0;
    }

    void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnBallHit -= ProcessHit;
            EventManager.Instance.OnGameFinished -= SaveScore;
            EventManager.Instance.OnGameReady -= ResetScore;
        }
    }
}