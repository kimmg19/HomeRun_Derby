using System;
using UnityEngine;

/// <summary>
/// �⺻ �̺�Ʈ �ý��� - ���� �� �̺�Ʈ�� �߾ӿ��� �����ϴ� �Ŵ���
/// </summary>
[DefaultExecutionOrder(-10000)]
public class EventManager : MonoBehaviour
{
    // �̱��� ���� ����
    public static EventManager Instance { get; private set; }

    public event Action OnReloadPlayGroud;

    // ���� ���� ���� �̺�Ʈ
    public event Action OnGameReady;
    public event Action OnGameFinished;
    public event Action OnGameStart;

    // ���� ���� �̺�Ʈ
    public event Action<bool> EnableBallData;
    public event Action<int> OnPitchStart;
    public event Action OnPitchComplete;
    public event Action<float, EPitchPosition, EPitchType> OnBallReleased;

    // Ÿ�� ���� �̺�Ʈ
    public event Action OnSwingStart;
    public event Action<int> OnSwingCount;
    public event Action<EHitTiming, float> OnBallHit; 
    public event Action OnSwingOccurred;

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
    public void PublishReloadPlayGround()=>OnReloadPlayGroud?.Invoke(); 
    // �̺�Ʈ ���� �޼����
    public void PublishGameReady() => OnGameReady?.Invoke();
    public void PublishGameStart() => OnGameStart?.Invoke();
    public void PublishGameFinished() => OnGameFinished?.Invoke();

    public void PublishEnableBallData(bool b) => EnableBallData?.Invoke(b);
    public void PublishPitch(int currentDifficulty) => OnPitchStart?.Invoke(currentDifficulty);
    public void PublishPitchComplete() => OnPitchComplete?.Invoke();
    public void PublishBallReleased(float speed, EPitchPosition position, EPitchType curPitchType) =>
       OnBallReleased?.Invoke(speed, position, curPitchType);

    public void PublishSwingOccurred() => OnSwingOccurred?.Invoke();
    public void PublishSwingCount(int count) => OnSwingCount?.Invoke(count);//ui������Ʈ, ����ī��Ʈ ����
    public void PublishSwing() => OnSwingStart?.Invoke();//Ÿ�� ���� Ʈ����
    public void PublishBallHit(EHitTiming timing, float distance) =>
        OnBallHit?.Invoke(timing, distance);//Ÿ�� ����
}