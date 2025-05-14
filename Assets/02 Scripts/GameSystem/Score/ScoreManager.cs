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

    // Inspector���� ���� ������ ����
    [Header("���� ����")]
    [SerializeField, ReadOnly] int baseScore = 1000;//�⺻ ����
    [SerializeField, ReadOnly] float distanceMultiplier = 2.0f;//��Ÿ� ���
    [SerializeField, ReadOnly] float homerunDistance = 99f;//Ȩ�� ��Ÿ� ����
    [SerializeField, ReadOnly] float longDistanceThreshold = 130f;//���� Ȩ�� �߰����� ����

    // ���� ����
    public int CurrentScore { get; set; }
    public int BestScore { get; set; }
    ScoreCalculator calculator;

    Queue<HitRecord> hits = new Queue<HitRecord>();  //Ÿ�� ��� ����� ť.
    Queue<PitchRecord> pitches = new Queue<PitchRecord>();  //���� ��� ����� ť.


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
            EventManager.Instance.OnGameReady += ResetScore;
        }
        else Debug.LogError("ScoreManager �̺�Ʈ ��� ����");
    }
    EHitTiming timing = EHitTiming.Miss;
    float distance = 0;
    int score = 0;
    EPitchLocation ePitchLocation; EPitchType ePitchType; float speed;
    // Ÿ�� ó�� �� ���� ���
    void ProcessHit(EHitTiming t, float d, bool isCritical)
    {

        if (t == EHitTiming.Miss)
        {
            EventManager.Instance.PublishHitResult(false, 0, t, 0, false, false);
            SetHitRecord(t, 0, 0);

            return;
        }
        bool isHomerun = calculator.IsHomerun(d, homerunDistance);   // Ȩ�� ����
        bool isBighomerun = calculator.IsBigHomerun(d, longDistanceThreshold);//����Ȩ������
        if (isHomerun || isBighomerun)
        {
            SoundManager.Instance.PlaySFX(SoundManager.ESfx.Homerun, 0.3f);
        }
        score = calculator.CalculateScore(                              // ���� ���
            t, d, isHomerun,
            baseScore, distanceMultiplier, longDistanceThreshold
        );
        SetHitRecord(t, d, score);
        // Ÿ�� �̺�Ʈ �߻�-UI�� ǥ��
        EventManager.Instance.PublishHitResult(isHomerun, d, t, score, isCritical, isBighomerun);

        // ���� �߰�
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