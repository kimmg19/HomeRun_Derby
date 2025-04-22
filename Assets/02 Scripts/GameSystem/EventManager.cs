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
    public event Action<bool> EnableBallData;
    public event Action<int> OnPitchStart;
    public event Action OnWindUpStart;
    public event Action<float, EPitchPosition, EPitchType> OnBallReleased;

    // 타자 관련 이벤트
    public event Action OnSwingStart;
    public event Action<int> OnSwingCount;
    public event Action<EHitTiming, float> OnBallHit;
    public event Action OnSwingOccurred;

    // 점수 관련 이벤트 추가
    public event Action<int,int> OnScoreChanged;
    public event Action<bool, float, EHitTiming, int> OnHitResult; // 홈런 결과 (홈런 여부, 거리, 타이밍)

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

    // 이벤트 발행 메서드들
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

    // 점수 관련 이벤트 발행 메서드
    public void PublishScoreChanged(int currentScore,int targetScore) => OnScoreChanged?.Invoke(currentScore, targetScore);
    public void PublishHitResult(bool isHomerun, float distance, EHitTiming timing,int score) => OnHitResult?.Invoke(isHomerun, distance, timing,score);
}