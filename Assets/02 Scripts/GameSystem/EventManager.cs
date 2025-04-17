using System;
using UnityEngine;

/// <summary>
/// 기본 이벤트 시스템 - 게임 내 이벤트를 중앙에서 관리하는 매니저
/// </summary>
[DefaultExecutionOrder(-10000)]
public class EventManager : MonoBehaviour
{
    // 싱글톤 패턴 구현
    public static EventManager Instance { get; private set; }

    public event Action OnReloadPlayGroud;

    // 게임 상태 관련 이벤트
    public event Action OnGameReady;
    public event Action OnGameFinished;
    public event Action OnGameStart;

    // 투수 관련 이벤트
    public event Action<bool> EnableBallData;
    public event Action<int> OnPitchStart;
    public event Action OnPitchComplete;
    public event Action<float, EPitchPosition, EPitchType> OnBallReleased;

    // 타자 관련 이벤트
    public event Action OnSwingStart;
    public event Action<int> OnSwingCount;
    public event Action<EHitTiming, float> OnBallHit; 
    public event Action OnSwingOccurred;

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
    public void PublishReloadPlayGround()=>OnReloadPlayGroud?.Invoke(); 
    // 이벤트 발행 메서드들
    public void PublishGameReady() => OnGameReady?.Invoke();
    public void PublishGameStart() => OnGameStart?.Invoke();
    public void PublishGameFinished() => OnGameFinished?.Invoke();

    public void PublishEnableBallData(bool b) => EnableBallData?.Invoke(b);
    public void PublishPitch(int currentDifficulty) => OnPitchStart?.Invoke(currentDifficulty);
    public void PublishPitchComplete() => OnPitchComplete?.Invoke();
    public void PublishBallReleased(float speed, EPitchPosition position, EPitchType curPitchType) =>
       OnBallReleased?.Invoke(speed, position, curPitchType);

    public void PublishSwingOccurred() => OnSwingOccurred?.Invoke();
    public void PublishSwingCount(int count) => OnSwingCount?.Invoke(count);//ui업데이트, 스윙카운트 감소
    public void PublishSwing() => OnSwingStart?.Invoke();//타자 스윙 트리거
    public void PublishBallHit(EHitTiming timing, float distance) =>
        OnBallHit?.Invoke(timing, distance);//타격 정보
}