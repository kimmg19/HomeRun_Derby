using System;
using System.Collections;
using UnityEngine;


public enum GameState
{
    Intro,
    Playing,
    Finish
}

public class HomerunDerbyManager : MonoBehaviour
{
    public static HomerunDerbyManager Instance;

    GameState state;
    Coroutine pitchCouroutine;
    public Ball CurrentBall { get; set; } //현재 투수가 던진 공
    [SerializeField] GameObject readyUI;
    [SerializeField] PitcherManager pitcherManager;
    [SerializeField] HitterManager hitterManager;
    [SerializeField] float swingChance = 15;
    [SerializeField] float pitchClock;

    //이벤트
    event Action OnSwing;
    event Action<EHitTiming, float> OnBallHit;
    event Action OnGameReady;
    event Action OnGameFinished;
    event Action OnWindUpStart;
    event Action OnPitchComplete;
    event Action<Ball, float, EPitchPosition> OnBallReleased;

    //이벤트 트리거
    public void TriggerSwing() => OnSwing?.Invoke();
    public void TriggerBallHit(EHitTiming timing, float distance) => OnBallHit?.Invoke(timing, distance);
    public void TriggerWindUpStart() => OnWindUpStart?.Invoke();
    public void TriggerPitchComplete() => OnPitchComplete?.Invoke();
    public void TriggerBallReleased(Ball ball, float speed, EPitchPosition position) =>
        OnBallReleased?.Invoke(ball, speed, position);
    public void TriggerGameReady()=> OnGameReady?.Invoke();
    public void TriggerGameFinished()=>OnGameFinished?.Invoke();

    void Awake()
    {
        if (Instance == null || Instance != this) Instance = this;
        SetUpEventListener();
    }
    void SetUpEventListener()
    {
        OnGameReady += GameStarted;
        OnGameFinished += GameFinished;
        OnSwing += SwingCount;
        //투수 공 던지는 순간 타자 다리 들기
        if (hitterManager) OnPitchComplete += hitterManager.OnReady;
    }
    void Start()
    {
        TriggerGameReady();
    }

    IEnumerator StartPitching()
    {
        while (swingChance > 0 && state == GameState.Playing)
        {
            pitcherManager.Pitching();
            yield return new WaitForSeconds(pitchClock);
        }
        TriggerGameFinished();
    }
    //이상함
    public void OnTouch()
    {
        if (state == GameState.Intro)
        {
            print("Game Start");
            readyUI.SetActive(false);
            state = GameState.Playing;
            pitchCouroutine = StartCoroutine(StartPitching());
            return;
        }
        else
        {
            if (pitcherManager.EState != PitchState.Throw) return;
            print("Swing");
            hitterManager.Swing();
        }
    }

    void GameStarted()
    {
        state = GameState.Intro;
        print("Loading Complete");
    }

    void GameFinished()
    {
        state = GameState.Finish;
        print("Finish");
    }

    void ClearAllEventListeners()
    {
        OnGameReady = null;
        OnGameFinished = null;
        OnWindUpStart = null;
        OnPitchComplete = null;
        OnBallReleased = null;
        OnBallHit = null;
        OnSwing = null;
    }

    //이벤트 함수
    void SwingCount()
    {
        swingChance--;
    }

    void OnDisable()
    {
        ClearAllEventListeners();
        if (pitchCouroutine != null) StopCoroutine(pitchCouroutine);
    }
}