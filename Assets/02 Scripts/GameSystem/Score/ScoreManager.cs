using UnityEngine;

/// <summary>
/// Unity 라이프사이클 및 이벤트 처리를 담당하는 MonoBehaviour 클래스
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    // Inspector에서 조정 가능한 값들
    [Header("점수 설정")]
    [SerializeField] int baseScore = 50;
    [SerializeField] float distanceMultiplier = 2.0f;
    [SerializeField] float homerunDistance = 70f;
    [SerializeField] float longDistanceThreshold = 100f;

    // 점수 변수
    public int CurrentScore { get;  set; }
    public int BestScore { get;  set; }

    // 계산기 인스턴스
    ScoreCalculator calculator;

    void Awake()
    {
        // 싱글톤 패턴
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        // 계산기 인스턴스 생성
        calculator = new ScoreCalculator();

        // 최고 점수 로드
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

    // 타격 처리 및 점수 계산
    void ProcessHit(EHitTiming timing, float distance)
    {
        if (timing == EHitTiming.Miss) return;

        // 홈런 판정
        bool isHomerun = calculator.IsHomerun(distance, homerunDistance);

        // 점수 계산
        int score = calculator.CalculateScore(
            timing, distance, isHomerun,
            baseScore, distanceMultiplier, longDistanceThreshold
        );

        // 홈런 이벤트 발생
        EventManager.Instance.PublishHomerunResult(isHomerun, distance);

        // 결과 로그
        Debug.Log($"점수: +{score}점 (거리: {distance}m, 타이밍: {timing}, 홈런: {isHomerun})");

        // 점수 추가
        AddScore(score);
    }

    // 점수 추가
    void AddScore(int points)
    {
        CurrentScore += points;
        EventManager.Instance.PublishScoreChanged(CurrentScore);

        if (CurrentScore > BestScore)
        {
            BestScore = CurrentScore;
            EventManager.Instance.PublishBestScoreChanged(BestScore);
        }
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
        EventManager.Instance.PublishScoreChanged(CurrentScore);
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