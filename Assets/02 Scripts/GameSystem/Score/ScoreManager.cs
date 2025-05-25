using System.Collections.Generic;
using UnityEngine;
#region ��� ����ü
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

    // �̱��� �ν��Ͻ�
    public static ScoreManager Instance;

    // ���� ��� ������Ʈ
    ScoreCalculator calculator;

    // ���� ���� ������
    [Header("���� ����")]
    [SerializeField, ReadOnly] int baseScore = 1000;         // �⺻ ����
    [SerializeField, ReadOnly] float distanceMultiplier = 2.0f; // ��Ÿ� ���
    [SerializeField, ReadOnly] float homerunDistance = 90f;    // Ȩ�� ��Ÿ� ����
    [SerializeField, ReadOnly] float longDistanceThreshold = 120f; // ���� Ȩ�� �߰����� ����

    // ���� ���� ��� ������
    [Header("���� ���")]
    [SerializeField, ReadOnly] int sessionHomerunCount = 0;
    [SerializeField, ReadOnly] int sessionPerfectHits = 0;
    [SerializeField, ReadOnly] float sessionTotalDistance = 0f;

    // ���� ������Ƽ
    public int CurrentScore { get; set; }
    public int RewardCurrency { get; set; }
    public int BestScore { get; set; }

    // Ÿ�� �� ���� ��� ť
    Queue<HitRecord> hits = new Queue<HitRecord>();
    Queue<PitchRecord> pitches = new Queue<PitchRecord>();

    // ���� Ÿ�� �� ���� �ӽ� ����
    EHitTiming timing = EHitTiming.Miss;
    float distance = 0;
    int score = 0;
    EPitchLocation ePitchLocation;
    EPitchType ePitchType;
    float speed;

    #region �����ֱ� �޼���
    void Awake()
    {
        // �̱��� ����
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
        else Debug.LogError("ScoreManager �̺�Ʈ ��� ����");
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

    #region ���� ����
    // Ÿ�� ó�� �� ���� ���
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
            sessionHomerunCount++; // Ȩ�� ���� ����
        }

        if (timing == EHitTiming.Perfect)
        {
            sessionPerfectHits++; // ����Ʈ Ÿ�̹� ���� ����
        }

        sessionTotalDistance += distance; // �� ��Ÿ� ����

        score = calculator.CalculateScore(
            timing, distance, isHomerun,
            baseScore, distanceMultiplier, longDistanceThreshold
        );

        SetHitRecord(timing, distance, score);
        EventManager.Instance.PublishHitResult(isHomerun, distance, timing, score, isCritical, isBighomerun);
        AddScore(score);
    }

    // ���� �߰�
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

    // ���� ����
    void SaveScore()
    {
        PlayerPrefs.SetInt("BestScore", BestScore);
        PlayerPrefs.Save();
    }

    // ���� �ʱ�ȭ
    void ResetScore()
    {
        CurrentScore = 0;
    }

    // ���� ���� �� ���� ����
    void Reward()
    {
        RewardCurrency = CurrentScore / 100;
        PlayerManager.Instance.AddCurrency(RewardCurrency);
    }
    #endregion

    #region ��� ����
    // Ÿ�� ��� ����
    public void SetHitRecord(EHitTiming t, float d, int s)
    {
        timing = t;
        distance = d;
        score = s;
    }

    // ���� ��� ����
    public void SetPitchRecord(EPitchLocation p, EPitchType pSO, float s)
    {
        ePitchLocation = p;
        ePitchType = pSO;
        speed = s;
    }

    // ��� ����
    public void SetRecord()
    {
        hits.Enqueue(new HitRecord(timing, distance, score));
        pitches.Enqueue(new PitchRecord(ePitchLocation, ePitchType, speed));
        ResetRecord();
    }

    // �ӽ� ��� �ʱ�ȭ
    void ResetRecord()
    {
        timing = EHitTiming.Miss;
        distance = 0;
        score = 0;
    }

    // ���� ��� �ʱ�ȭ
    void ResetSessionStats()
    {
        sessionHomerunCount = 0;
        sessionPerfectHits = 0;
        sessionTotalDistance = 0f;
    }

    // Ÿ�� ��� ��ȯ
    public Queue<HitRecord> GetHitRecord()
    {
        return hits;
    }

    // ���� ��� ��ȯ
    public Queue<PitchRecord> GetPitchRecord()
    {
        return pitches;
    }
    #endregion

    #region ����Ʈ �ý��� ������
    // ����Ʈ �ý����� ���� ��� ������ ���� �޼���
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

