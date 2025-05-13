using System;
using UnityEngine;

/// <summary>
/// �⺻ �̺�Ʈ �ý��� - ���� �� �̺�Ʈ�� �߾ӿ��� �����ϴ� �Ŵ���
/// </summary>
[DefaultExecutionOrder(-10000)]
public class EventManager : MonoBehaviour
{
    // �̱��� ���� ����
    private static EventManager instance;
    public static EventManager Instance
    {
        get
        {
            if (instance == null && Time.timeScale != 0)
            {
                var obj = FindAnyObjectByType<EventManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = Create();
                }
            }
            return instance;
        }
    }

    static EventManager Create()
    {
        return Instantiate(Resources.Load<EventManager>("EventManager"));
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

    }
    public event Action OnReloadPlayGroud;

    // ���� ���� ���� �̺�Ʈ
    public event Action OnGameReady;
    public event Action OnGameFinished;
    public event Action OnGameStart;

    // ���� ���� �̺�Ʈ
    public event Action<EPitchPosition> OnEnablePitchData;
    public event Action<int> OnPitchStart;
    public event Action OnWindUpStart;
    public event Action<float, EPitchPosition, EPitchType> OnSetBallData;

    // Ÿ�� ���� �̺�Ʈ
    public event Action OnSwing;
    public event Action OnSwingStart;
    public event Action<int> OnSwingCount;
    public event Action<EHitTiming, float, bool> OnBallHit;
    public event Action OnBallSwing;

    // ���� ���� �̺�Ʈ �߰�
    public event Action<int, int> OnScoreChanged;
    public event Action<bool, float, EHitTiming, int, bool, bool> OnHitResult; // Ȩ�� ��� (Ȩ�� ����, �Ÿ�, Ÿ�̹�)

    public event Action<Transform, EHitTiming> OnHitEffect;


    public void PublishReloadPlayGround() => OnReloadPlayGroud?.Invoke();

    // ����
    public void PublishGameReady() => OnGameReady?.Invoke();
    public void PublishGameStart() => OnGameStart?.Invoke();
    public void PublishGameFinished() => OnGameFinished?.Invoke();
    //����
    public void PublishOnEnablePitchData(EPitchPosition pos) => OnEnablePitchData?.Invoke(pos);
    public void PublishPitch(int currentDifficulty) => OnPitchStart?.Invoke(currentDifficulty);
    public void PublishWindUpStart() => OnWindUpStart?.Invoke();
    public void PublishOnSetBallData(float speed, EPitchPosition position, EPitchType curPitchType) =>
       OnSetBallData?.Invoke(speed, position, curPitchType);
    //Ÿ��
    public void PublishOnBallSwing() => OnBallSwing?.Invoke();
    public void PublishSwingCount(int count) => OnSwingCount?.Invoke(count);
    public void PublishOnSwing()=>OnSwing?.Invoke();
    public void PublishSwingStart() => OnSwingStart?.Invoke();
    public void PublishBallHit(EHitTiming timing, float distance, bool isCritical) =>
        OnBallHit?.Invoke(timing, distance, isCritical);

    // ���� �� UI
    public void PublishScoreChanged(int currentScore, int targetScore) =>
        OnScoreChanged?.Invoke(currentScore, targetScore);
    public void PublishHitResult(bool isHomerun, float d, EHitTiming t, int s, bool c, bool b) =>
         OnHitResult?.Invoke(isHomerun, d, t, s, c, b);

    public void PublishHitEffect(Transform hitPosition, EHitTiming timing) => OnHitEffect?.Invoke(hitPosition, timing);
    void OnApplicationQuit()
    {
        Time.timeScale = 0;
    }
}