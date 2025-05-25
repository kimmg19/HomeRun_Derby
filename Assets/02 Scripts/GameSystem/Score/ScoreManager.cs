using System.Collections.Generic;
using UnityEngine;
#region 기록 구조체
public struct HitRecord
{
    EHitTiming timing;
    float distance;
    int score;

    public HitRecord(EHitTiming t, float d, int s)
    {
        this.timing = t;
        this.distance = d;
        this.score = s;
    }
    public float GetDistance() { return distance; }
    public int GetScore() { return score; }
    public EHitTiming GetTiming() { return timing; }
}

public struct PitchRecord
{
    EPitchLocation pitchPosition;
    EPitchType pitchType;
    float speed;
    public PitchRecord(EPitchLocation p, EPitchType pSO, float s)
    {
        pitchPosition = p;
        pitchType = pSO;
        speed = s;
    }
    public EPitchLocation GetPitchLocation() { return pitchPosition; }
    public EPitchType GetEPitchType() { return pitchType; }
    public float GetSpeed() { return speed; }
}
#endregion

public class ScoreManager : MonoBehaviour
{

    // 싱글톤 인스턴스
    public static ScoreManager Instance;

    // 점수 계산 컴포넌트
    ScoreCalculator calculator;

    // 점수 설정 변수들
    [Header("점수 설정")]
    [SerializeField, ReadOnly] int baseScore = 1000;         // 기본 점수
    [SerializeField, ReadOnly] float distanceMultiplier = 2.0f; // 비거리 계수
    [SerializeField, ReadOnly] float homerunDistance = 90f;    // 홈런 비거리 기준
    [SerializeField, ReadOnly] float longDistanceThreshold = 120f; // 대형 홈런 추가점수 기준

    // 게임 세션 통계 데이터
    [Header("세션 통계")]
    [SerializeField, ReadOnly] int sessionHomerunCount = 0;
    [SerializeField, ReadOnly] int sessionPerfectHits = 0;
    [SerializeField, ReadOnly] float sessionTotalDistance = 0f;

    // 점수 프로퍼티
    public int CurrentScore { get; set; }
    public int RewardCurrency { get; set; }
    public int BestScore { get; set; }

    // 타격 및 투구 기록 큐
    Queue<HitRecord> hits = new Queue<HitRecord>();
    Queue<PitchRecord> pitches = new Queue<PitchRecord>();

    // 현재 타격 및 투구 임시 변수
    EHitTiming timing = EHitTiming.Miss;
    float distance = 0;
    int score = 0;
    EPitchLocation ePitchLocation;
    EPitchType ePitchType;
    float speed;

    #region 생명주기 메서드
    void Awake()
    {
        // 싱글톤 패턴
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        calculator = GetComponent<ScoreCalculator>();
        BestScore = PlayerPrefs.GetInt("BestScore", 0);
    }

    void OnEnable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnBallHit += ProcessHit;
            EventManager.Instance.OnGameFinished += SaveScore;
            EventManager.Instance.OnGameFinished += Reward;
            EventManager.Instance.OnGameReady += ResetScore;
            EventManager.Instance.OnGameReady += ResetSessionStats;
        }
        else Debug.LogError("ScoreManager 이벤트 등록 실패");
    }

    void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnBallHit -= ProcessHit;
            EventManager.Instance.OnGameFinished -= SaveScore;
            EventManager.Instance.OnGameFinished -= Reward;
            EventManager.Instance.OnGameReady -= ResetScore;
            EventManager.Instance.OnGameReady -= ResetSessionStats;
        }
    }
    #endregion

    #region 점수 관리
    // 타격 처리 및 점수 계산
    void ProcessHit(BallHitData hitData)
    {

        bool isHomerun = calculator.IsHomerun(hitData.distance, homerunDistance);
        bool isBighomerun = calculator.IsBigHomerun(hitData.distance, longDistanceThreshold);
        float distance = hitData.distance;
        EHitTiming timing = hitData.timing;
        bool isCritical=hitData.isCritical;
        if (timing == EHitTiming.Miss)
        {
            EventManager.Instance.PublishHitResult(false, 0, timing, 0, false, false);
            SetHitRecord(timing, 0, 0);
            return;
        }
        
        if (isHomerun || isBighomerun)
        {
            SoundManager.Instance.PlaySFX(SoundManager.ESfx.Homerun, 0.3f);
            sessionHomerunCount++; // 홈런 개수 증가
        }

        if (timing == EHitTiming.Perfect)
        {
            sessionPerfectHits++; // 퍼펙트 타이밍 개수 증가
        }

        sessionTotalDistance += distance; // 총 비거리 누적

        score = calculator.CalculateScore(
            timing, distance, isHomerun,
            baseScore, distanceMultiplier, longDistanceThreshold
        );

        SetHitRecord(timing, distance, score);
        EventManager.Instance.PublishHitResult(isHomerun, distance, timing, score, isCritical, isBighomerun);
        AddScore(score);
    }

    // 점수 추가
    void AddScore(int points)
    {
        int lastScore = CurrentScore;
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

    // 게임 종료 시 보상 지급
    void Reward()
    {
        RewardCurrency = CurrentScore / 100;
        PlayerManager.Instance.AddCurrency(RewardCurrency);
    }
    #endregion

    #region 기록 관리
    // 타격 기록 설정
    public void SetHitRecord(EHitTiming t, float d, int s)
    {
        timing = t;
        distance = d;
        score = s;
    }

    // 투구 기록 설정
    public void SetPitchRecord(EPitchLocation p, EPitchType pSO, float s)
    {
        ePitchLocation = p;
        ePitchType = pSO;
        speed = s;
    }

    // 기록 저장
    public void SetRecord()
    {
        hits.Enqueue(new HitRecord(timing, distance, score));
        pitches.Enqueue(new PitchRecord(ePitchLocation, ePitchType, speed));
        ResetRecord();
    }

    // 임시 기록 초기화
    void ResetRecord()
    {
        timing = EHitTiming.Miss;
        distance = 0;
        score = 0;
    }

    // 세션 통계 초기화
    void ResetSessionStats()
    {
        sessionHomerunCount = 0;
        sessionPerfectHits = 0;
        sessionTotalDistance = 0f;
    }

    // 타격 기록 반환
    public Queue<HitRecord> GetHitRecord()
    {
        return hits;
    }

    // 투구 기록 반환
    public Queue<PitchRecord> GetPitchRecord()
    {
        return pitches;
    }
    #endregion

    #region 퀘스트 시스템 데이터
    // 퀘스트 시스템을 위한 통계 데이터 제공 메서드
    public int GetSessionHomerunCount()
    {
        return sessionHomerunCount;
    }

    public int GetSessionPerfectHits()
    {
        return sessionPerfectHits;
    }

    public float GetSessionTotalDistance()
    {
        return sessionTotalDistance;
    }
    #endregion
}

