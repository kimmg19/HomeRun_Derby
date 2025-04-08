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
    [SerializeField] GameObject readyUI;
    [SerializeField] PitcherManager pitcherManager;
    [SerializeField] HitterManager hitterManager;
    [SerializeField] float swingChance = 15;
    [SerializeField] float pitchClock;

    //이벤트
    public event Action OnSwing;
    public event Action<EHitTiming, float> OnBallHit;
    public event Action OnMiss;
    public event Action GameReady;
    public event Action GameFinished;
    public event Action OnWindUpStart;
    public event Action OnPitchComplete;
    public event Action<Ball, float, EPitchPosition> OnBallReleased;

    //이벤트 트리거
    public void TriggerSwing() => OnSwing?.Invoke();
    public void TriggerBallHit(EHitTiming timing, float distance) => OnBallHit?.Invoke(timing, distance);
    public void TriggerMiss() => OnMiss?.Invoke();
    public void TriggerWindUpStart() => OnWindUpStart?.Invoke();
    public void TriggerPitchComplete() => OnPitchComplete?.Invoke();
    public void TriggerBallReleased(Ball ball, float speed, EPitchPosition position) =>
        OnBallReleased?.Invoke(ball, speed, position);    

    void Awake()
    {
        if (Instance == null || Instance != this) Instance = this;
        OnSwing += SwingCount;
        SetUpEventListener();
    }

    void Start()
    {
        GameReady?.Invoke();
    }
    
    IEnumerator StartPitching()
    {
        while (swingChance > 0 && state == GameState.Playing)
        {
            pitcherManager.Pitching();
            yield return new WaitForSeconds(pitchClock);
        }
        GameFinished?.Invoke();
    }

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
            if (pitcherManager.eState != PitchState.Throw) return;
            print("Swing");
            hitterManager.Swing();
        }
    }

    void SetUpEventListener()
    {
        GameReady += GameStarted;
        GameFinished += GameFinished_Handler;
        if (hitterManager) OnPitchComplete += hitterManager.OnReady;
    }

    void GameStarted()
    {
        state = GameState.Intro;
        print("Loading Complete");
    }

    void GameFinished_Handler()
    {
        state = GameState.Finish;
        print("Finish");
    }

    void ClearAllEventListeners()
    {
        GameReady = null;
        GameFinished = null;
        OnWindUpStart = null;
        OnPitchComplete = null;
        OnBallReleased = null;
        OnBallHit = null;
        OnMiss = null;
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