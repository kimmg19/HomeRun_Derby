using System;
using UnityEngine;

/// <summary>
/// 기본 이벤트 시스템 - 게임 내 이벤트를 중앙에서 관리하는 매니저
/// </summary>
[DefaultExecutionOrder(-10000)]
public class EventManager : MonoBehaviour
{
    // 싱글톤 패턴 구현
    public static EventManager Instance { get; set; }

    public event Action OnReloadPlayGroud;

    // 게임 상태 관련 이벤트
    public event Action OnGameReady;
    public event Action OnGameFinished;
    public event Action OnGameStart;

    // 투수 관련 이벤트
    public event Action<EPitchPosition> OnEnablePitchData;
    public event Action<int> OnPitchStart;
    public event Action OnWindUpStart;
    public event Action<float, EPitchPosition, EPitchType> OnSetBallData;

    // 타자 관련 이벤트
    public event Action OnSwingStart;
    public event Action<int> OnSwingCount;
    public event Action<EHitTiming, float, bool> OnBallHit;
    public event Action OnBallSwing;

    // 점수 관련 이벤트 추가
    public event Action<int, int> OnScoreChanged;
    public event Action<bool, float, EHitTiming, int,bool,bool> OnHitResult; // 홈런 결과 (홈런 여부, 거리, 타이밍)

    void Awake()
    {
        // 싱글톤 인스턴스 설정
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

    // 게임
    public void PublishGameReady() => OnGameReady?.Invoke();
    public void PublishGameStart() => OnGameStart?.Invoke();
    public void PublishGameFinished() => OnGameFinished?.Invoke();
    //투구
    public void PublishOnEnablePitchData(EPitchPosition pos) => OnEnablePitchData?.Invoke(pos);
    public void PublishPitch(int currentDifficulty) => OnPitchStart?.Invoke(currentDifficulty);
    public void PublishWindUpStart() => OnWindUpStart?.Invoke();
    public void PublishOnSetBallData(float speed, EPitchPosition position, EPitchType curPitchType) =>
       OnSetBallData?.Invoke(speed, position, curPitchType);
    //타격
    public void PublishOnBallSwing() => OnBallSwing?.Invoke();
    public void PublishSwingCount(int count) => OnSwingCount?.Invoke(count);
    public void PublishSwing() => OnSwingStart?.Invoke();
    public void PublishBallHit(EHitTiming timing, float distance, bool isCritical) =>
        OnBallHit?.Invoke(timing, distance, isCritical);

    // 점수 및 UI
    public void PublishScoreChanged(int currentScore, int targetScore) =>
        OnScoreChanged?.Invoke(currentScore, targetScore);
    public void PublishHitResult(bool isHomerun, float d, EHitTiming t, int s,bool c,bool b) =>
         OnHitResult?.Invoke(isHomerun, d, t, s, c, b);
}