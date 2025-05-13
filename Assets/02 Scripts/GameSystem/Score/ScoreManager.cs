using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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
    EPitchPosition pitchPosition;
    PitchTypeDataSO pitchTypeDataSO;
    float speed;
    public PitchRecord(EPitchPosition p, PitchTypeDataSO pSO, float s)
    {
        pitchPosition = p;
        pitchTypeDataSO = pSO;
        speed = s;
    }
    public EPitchPosition GetPitchPosition() { return pitchPosition; }
    public PitchTypeDataSO GetPitchTypeDataSO() { return pitchTypeDataSO; }
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
    EHitTiming timing=EHitTiming.Miss;
    float distance=0;
    int score=0;
    // Ÿ�� ó�� �� ���� ���
    void ProcessHit(EHitTiming t, float d, bool isCritical)
    {
        
        if (t == EHitTiming.Miss)
        {
            print("�꽺��");
            EventManager.Instance.PublishHitResult(false, 0, t, 0, false, false);
            SetHit(t, 0, 0);            
            
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
        SetHit(t, d, score);
        // Ÿ�� �̺�Ʈ �߻�-UI�� ǥ��
        EventManager.Instance.PublishHitResult(isHomerun, d, t, score, isCritical, isBighomerun);
        
        // ���� �߰�
        AddScore(score);        
    }
    void ResetHit()
    {
        timing=EHitTiming.Miss;
        distance = 0;
        score = 0;
    }
    public void SetHit(EHitTiming t, float d, int s)
    {
        timing = t; distance=d; score=s;        
    }
    public void SetHitRecord()
    {
        print(timing + "" + distance + "" + timing);
        hits.Enqueue(new HitRecord(timing, distance, score));
        ResetHit();
    }
    public void SetPitchRecord(EPitchPosition p, PitchTypeDataSO pSO, float speed)
    {
        pitches.Enqueue(new PitchRecord(p,pSO,speed));
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
    public Queue<HitRecord> GetHitRecord()
    {
        return new Queue<HitRecord>(hits);
    }
    public Queue<PitchRecord> GetPitchRecord()
    {
        return new Queue<PitchRecord>(pitches);
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