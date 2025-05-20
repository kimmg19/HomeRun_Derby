using System;
using UnityEngine;


[DefaultExecutionOrder(-10000)]
public class EventManager : MonoBehaviour
{
    // 싱글톤 패턴 구현
    static EventManager instance;
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
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #region 이벤트
    // 게임 상태 이벤트
    public event Action OnGameReady;
    public event Action OnGameFinished;
    public event Action OnGameStart;

    // 투수 이벤트
    public event Action<EPitchLocation> OnEnablePitchData;
    public event Action<int> OnPitchStart;
    public event Action OnWindUpStart;
    public event Action<float, EPitchLocation, EPitchType> OnSetBallData;

    // 타자 이벤트
    public event Action OnSwing;
    public event Action OnSwingStart;
    public event Action<int> OnSwingCount;
    public event Action<EHitTiming, float, bool> OnBallHit;
    public event Action OnBallSwing;

    // 점수 및 UI 이벤트
    public event Action<int, int> OnScoreChanged;
    public event Action<bool, float, EHitTiming, int, bool, bool> OnHitResult; // 홈런 결과 (홈런 여부, 거리, 타이밍)

    //이펙트 이벤트
    public event Action<Transform, EHitTiming> OnHitEffect;

    //플레이어 및 상점 이벤트
    public event Action<int, int, int> OnPlayerStatChanged;
    public event Action<int> OnCurrencyChanged;
    public event Action<BatItemSO,int> OnBatUpgraded;
    #endregion

    #region 이벤트 실행
    // 게임 상태
    public void PublishGameReady() => OnGameReady?.Invoke();
    public void PublishGameStart() => OnGameStart?.Invoke();
    public void PublishGameFinished() => OnGameFinished?.Invoke();

    //투구
    public void PublishOnEnablePitchData(EPitchLocation pos) => OnEnablePitchData?.Invoke(pos);
    public void PublishPitch(int currentDifficulty) => OnPitchStart?.Invoke(currentDifficulty);
    public void PublishWindUpStart() => OnWindUpStart?.Invoke();
    public void PublishOnSetBallData(float speed, EPitchLocation position, EPitchType curPitchType) =>
       OnSetBallData?.Invoke(speed, position, curPitchType);

    //타자
    public void PublishOnBallSwing() => OnBallSwing?.Invoke();
    public void PublishSwingCount(int count) => OnSwingCount?.Invoke(count);
    public void PublishOnSwing() => OnSwing?.Invoke();
    public void PublishSwingStart() => OnSwingStart?.Invoke();
    public void PublishBallHit(EHitTiming timing, float distance, bool isCritical) =>
        OnBallHit?.Invoke(timing, distance, isCritical);

    // 점수 및 UI
    public void PublishScoreChanged(int currentScore, int targetScore) =>
        OnScoreChanged?.Invoke(currentScore, targetScore);
    public void PublishHitResult(bool isHomerun, float d, EHitTiming t, int s, bool c, bool b) =>
         OnHitResult?.Invoke(isHomerun, d, t, s, c, b);

    //이펙트
    public void PublishHitEffect(Transform hitPosition, EHitTiming timing) =>
        OnHitEffect?.Invoke(hitPosition, timing);

    //플레이어 및 상점 이벤트
    public void PublishPlayerStatsChanged(int power, int judge, int critical) =>
        OnPlayerStatChanged?.Invoke(power, judge, critical);
    public void PublishCurrencyChanged(int currency) => OnCurrencyChanged?.Invoke(currency);
    public void PublishBatUpgraded(BatItemSO batItemSO,int level) => OnBatUpgraded?.Invoke(batItemSO,level);
    #endregion

    void OnApplicationQuit()
    {
        Time.timeScale = 0;
    }
}