using System;
using UnityEngine;

/// <summary>
/// �⺻ �̺�Ʈ �ý��� - ���� �� �̺�Ʈ�� �߾ӿ��� �����ϴ� �Ŵ���
/// </summary>
[DefaultExecutionOrder(-10000)]
public class EventManager : MonoBehaviour
{
    // �̱��� ���� ����
    public static EventManager Instance { get; set; }

    public event Action OnReloadPlayGroud;

    // ���� ���� ���� �̺�Ʈ
    public event Action OnGameReady;
    public event Action OnGameFinished;
    public event Action OnGameStart;

    // ���� ���� �̺�Ʈ
    public event Action<bool> EnableBallData;
    public event Action<int> OnPitchStart;
    public event Action OnWindUpStart;
    public event Action<float, EPitchPosition, EPitchType> OnBallReleased;

    // Ÿ�� ���� �̺�Ʈ
    public event Action OnSwingStart;
    public event Action<int> OnSwingCount;
    public event Action<EHitTiming, float> OnBallHit;
    public event Action OnSwingOccurred;

    // ���� ���� �̺�Ʈ �߰�
    public event Action<int,int> OnScoreChanged;
    public event Action<bool, float, EHitTiming, int> OnHitResult; // Ȩ�� ��� (Ȩ�� ����, �Ÿ�, Ÿ�̹�)

    void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PublishReloadPlayGround() => OnReloadPlayGroud?.Invoke();

    // �̺�Ʈ ���� �޼����
    public void PublishGameReady() => OnGameReady?.Invoke();
    public void PublishGameStart() => OnGameStart?.Invoke();
    public void PublishGameFinished() => OnGameFinished?.Invoke();

    public void PublishEnableBallData(bool b) => EnableBallData?.Invoke(b);
    public void PublishPitch(int currentDifficulty) => OnPitchStart?.Invoke(currentDifficulty);
    public void PublishWindUpStart() => OnWindUpStart?.Invoke();
    public void PublishBallReleased(float speed, EPitchPosition position, EPitchType curPitchType) =>
       OnBallReleased?.Invoke(speed, position, curPitchType);

    public void PublishSwingOccurred() => OnSwingOccurred?.Invoke();
    public void PublishSwingCount(int count) => OnSwingCount?.Invoke(count);
    public void PublishSwing() => OnSwingStart?.Invoke();
    public void PublishBallHit(EHitTiming timing, float distance) =>
        OnBallHit?.Invoke(timing, distance);

    // ���� ���� �̺�Ʈ ���� �޼���
    public void PublishScoreChanged(int currentScore,int targetScore) => OnScoreChanged?.Invoke(currentScore, targetScore);
    public void PublishHitResult(bool isHomerun, float distance, EHitTiming timing,int score) => OnHitResult?.Invoke(isHomerun, distance, timing,score);
}