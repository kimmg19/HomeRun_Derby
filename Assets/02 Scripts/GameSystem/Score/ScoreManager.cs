using UnityEngine;

/// <summary>
/// Unity ����������Ŭ �� �̺�Ʈ ó���� ����ϴ� MonoBehaviour Ŭ����
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    // Inspector���� ���� ������ ����
    [Header("���� ����")]
    [SerializeField, ReadOnly] int baseScore = 1000;//�⺻ ����
    [SerializeField, ReadOnly] float distanceMultiplier = 2.0f;//��Ÿ� ���
    [SerializeField, ReadOnly] float homerunDistance = 50f;//Ȩ�� ��Ÿ� ����
    [SerializeField, ReadOnly] float longDistanceThreshold = 70f;//���� Ȩ�� �߰����� ����

    // ���� ����
    public int CurrentScore { get; set; }
    public int BestScore { get; set; }

    // ���� �ν��Ͻ�
    ScoreCalculator calculator;

    void Awake()
    {
        // �̱��� ����
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
        else Debug.LogError("ScoreManager �̺�Ʈ ��� ����");
    }

    // Ÿ�� ó�� �� ���� ���- Ÿ�� ���� �ÿ���
    void ProcessHit(EHitTiming timing, float distance)
    {
        bool isHomerun = calculator.IsHomerun(distance, homerunDistance);   // Ȩ�� ����
        int score = calculator.CalculateScore(                              // ���� ���
            timing, distance, isHomerun,
            baseScore, distanceMultiplier, longDistanceThreshold
        );
        // Ȩ�� �̺�Ʈ �߻�
        EventManager.Instance.PublishHitResult(isHomerun, distance, timing,score);

        // ���� �߰�
        AddScore(score);
    }

    // ���� �߰�
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