using System.Collections;
using UnityEngine;

public enum EGameState
{
    Intro,
    Playing,
    Finish
}

public class HomerunDerbyManager : MonoBehaviour
{
    public static HomerunDerbyManager Instance;

    public EGameState GameState { get; set; }
    Coroutine pitchCoroutine;
    public Ball CurrentBall { get; set; } // 현재 투수가 던진 공
    public int SwingCount { get; private set; } = 15;
    [Range(1, 5)] public int currentDifficulty = 1;
    [SerializeField, ReadOnly] float pitchClock = 8;

    void Awake()
    {
        if (Instance == null || Instance != this)
            Instance = this;
    }

    void OnEnable()
    {
        // 이벤트 구독
        if (EventManager.Instance != null)
        {
            // 스윙 이벤트 구독
            EventManager.Instance.OnGameReady += GameIsReady;
            EventManager.Instance.OnGameFinished += GameFinished;
            EventManager.Instance.OnGameStart += TouchAndStart;
            EventManager.Instance.OnSwingOccurred += DecreaseSwingCount;

        }
        else Debug.LogError("HomerunDerbyManager 이벤트 등록 실패");

    }
    void Start()
    {
        // 게임 시작 이벤트 발행
        EventManager.Instance.PublishGameReady();
    }

    void DecreaseSwingCount()
    {
        SwingCount--;
        if (SwingCount > 9 && SwingCount <= 12) currentDifficulty = 2;
        else if (SwingCount > 6 && SwingCount <= 9) currentDifficulty = 3;
        else if (SwingCount > 3 && SwingCount <= 6) currentDifficulty = 4;
        else if (SwingCount <= 3) currentDifficulty = 5;
        // 변경된 값을 UI 등에 알림
        EventManager.Instance.PublishSwingCount(SwingCount);
    }

    IEnumerator StartPitching()
    {
        while (SwingCount > 0 && GameState == EGameState.Playing)
        {
            EventManager.Instance.PublishPitch(currentDifficulty);
            yield return new WaitForSeconds(pitchClock);
        }

        // 게임 종료 이벤트 발행
        EventManager.Instance.PublishGameFinished();
    }

    // 터치 처리
    public void OnTouch()
    {
        if (GameState == EGameState.Intro)
        {
            EventManager.Instance.PublishGameStart();
            return;
        }
        else if (GameState == EGameState.Playing)
        {
            EventManager.Instance.PublishSwing();
        }
        else return;
    }

    private void TouchAndStart()
    {
        print("Game Start");
        GameState = EGameState.Playing;
        pitchCoroutine = StartCoroutine(StartPitching());
    }

    private void GameIsReady()
    {
        GameState = EGameState.Intro;
        print("Loading Complete");
    }

    private void GameFinished()
    {
        GameState = EGameState.Finish;
        print("Finish");
    }

    void OnDisable()
    {
        // 이벤트 구독 해제
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnSwingOccurred -= DecreaseSwingCount;
            EventManager.Instance.OnGameReady -= GameIsReady;
            EventManager.Instance.OnGameFinished -= GameFinished;
            EventManager.Instance.OnGameStart -= TouchAndStart;

        }
        else Debug.LogError("HomerunDerbyManager 이벤트 해제 실패");

        if (pitchCoroutine != null)
        {
            StopCoroutine(pitchCoroutine);
            pitchCoroutine = null;
        }
    }
}