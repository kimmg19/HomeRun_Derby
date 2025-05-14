using System.Collections.Generic;
using UnityEngine;

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

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    // Inspector에서 조정 가능한 값들
    [Header("점수 설정")]
    [SerializeField, ReadOnly] int baseScore = 1000;//기본 점수
    [SerializeField, ReadOnly] float distanceMultiplier = 2.0f;//비거리 계수
    [SerializeField, ReadOnly] float homerunDistance = 99f;//홈런 비거리 기준
    [SerializeField, ReadOnly] float longDistanceThreshold = 130f;//대형 홈런 추가점수 기준

    // 점수 변수
    public int CurrentScore { get; set; }
    public int BestScore { get; set; }
    ScoreCalculator calculator;

    Queue<HitRecord> hits = new Queue<HitRecord>();  //타격 기록 저장용 큐.
    Queue<PitchRecord> pitches = new Queue<PitchRecord>();  //투구 기록 저장용 큐.


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
            EventManager.Instance.OnGameReady += ResetScore;
        }
        else Debug.LogError("ScoreManager 이벤트 등록 실패");
    }
    EHitTiming timing = EHitTiming.Miss;
    float distance = 0;
    int score = 0;
    EPitchLocation ePitchLocation; EPitchType ePitchType; float speed;
    // 타격 처리 및 점수 계산
    void ProcessHit(EHitTiming t, float d, bool isCritical)
    {

        if (t == EHitTiming.Miss)
        {
            EventManager.Instance.PublishHitResult(false, 0, t, 0, false, false);
            SetHitRecord(t, 0, 0);

            return;
        }
        bool isHomerun = calculator.IsHomerun(d, homerunDistance);   // 홈런 판정
        bool isBighomerun = calculator.IsBigHomerun(d, longDistanceThreshold);//대형홈런판정
        if (isHomerun || isBighomerun)
        {
            SoundManager.Instance.PlaySFX(SoundManager.ESfx.Homerun, 0.3f);
        }
        score = calculator.CalculateScore(                              // 점수 계산
            t, d, isHomerun,
            baseScore, distanceMultiplier, longDistanceThreshold
        );
        SetHitRecord(t, d, score);
        // 타격 이벤트 발생-UI에 표시
        EventManager.Instance.PublishHitResult(isHomerun, d, t, score, isCritical, isBighomerun);

        // 점수 추가
        AddScore(score);
    }

    public void SetHitRecord(EHitTiming t, float d, int s)
    {
        timing = t; distance = d; score = s;
    }
    public void SetPitchRecord(EPitchLocation p, EPitchType pSO, float s)
    {
        ePitchLocation = p;
        ePitchType = pSO;
        speed = s;
    }
    public void SetRecord()
    {
        print(timing + "" + distance + "" + score);
        print(ePitchLocation + "" + ePitchType + "" + speed);
        hits.Enqueue(new HitRecord(timing, distance, score));
        pitches.Enqueue(new PitchRecord(ePitchLocation, ePitchType, speed));
        ResetRecord();
    }
    void ResetRecord()
    {
        timing = EHitTiming.Miss;
        distance = 0;
        score = 0;
    }

    public Queue<HitRecord> GetHitRecord()
    {
        return hits;
    }
    public Queue<PitchRecord> GetPitchRecord()
    {
        return pitches;
    }
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