using UnityEngine;

/// <summary>
/// Unity ����������Ŭ �� �̺�Ʈ ó���� ����ϴ� MonoBehaviour Ŭ����
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    // Inspector���� ���� ������ ����
    [Header("���� ����")]
    [SerializeField] int baseScore = 50;
    [SerializeField] float distanceMultiplier = 2.0f;
    [SerializeField] float homerunDistance = 70f;
    [SerializeField] float longDistanceThreshold = 100f;

    // ���� ����
    public int CurrentScore { get;  set; }
    public int BestScore { get;  set; }

    // ���� �ν��Ͻ�
    ScoreCalculator calculator;

    void Awake()
    {
        // �̱��� ����
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        // ���� �ν��Ͻ� ����
        calculator = new ScoreCalculator();

        // �ְ� ���� �ε�
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

    // Ÿ�� ó�� �� ���� ���
    void ProcessHit(EHitTiming timing, float distance)
    {
        if (timing == EHitTiming.Miss) return;

        // Ȩ�� ����
        bool isHomerun = calculator.IsHomerun(distance, homerunDistance);

        // ���� ���
        int score = calculator.CalculateScore(
            timing, distance, isHomerun,
            baseScore, distanceMultiplier, longDistanceThreshold
        );

        // Ȩ�� �̺�Ʈ �߻�
        EventManager.Instance.PublishHomerunResult(isHomerun, distance);

        // ��� �α�
        Debug.Log($"����: +{score}�� (�Ÿ�: {distance}m, Ÿ�̹�: {timing}, Ȩ��: {isHomerun})");

        // ���� �߰�
        AddScore(score);
    }

    // ���� �߰�
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