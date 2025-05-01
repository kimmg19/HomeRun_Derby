using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum EGameState
{
    Intro,
    Playing,
    Pause,
    Finish
}

public class HomerunDerbyManager : MonoBehaviour
{
    public static HomerunDerbyManager Instance;

    public EGameState GameState { get; set; }
    Coroutine pitchCoroutine;
    public Ball CurrentBall { get; set; } // 현재 투수가 던진 공
    public int SwingCount { get; private set; } = 15;
    [ReadOnly] public int currentDifficulty = 1;
    [SerializeField, ReadOnly] float pitchClock = 8;

    void OnEnable()
    {
        // 이벤트 구독
        if (EventManager.Instance != null)
        {
            // 스윙 이벤트 구독
            EventManager.Instance.OnGameReady += GameIsReady;
            EventManager.Instance.OnGameFinished += GameFinished;
            EventManager.Instance.OnGameStart += TouchAndStart;
            EventManager.Instance.OnBallSwing += DecreaseSwingCount;
            EventManager.Instance.OnEnablePitchData += IsStrike;
        }
        else Debug.LogError("HomerunDerbyManager 이벤트 등록 실패");
    }
    void Awake()
    {
        if (Instance == null || Instance != this)
            Instance = this;
    }
    void Start()
    {
        // 게임 시작 이벤트 발행
        EventManager.Instance.PublishGameReady();
    }
    void IsStrike(EPitchPosition pos)
    {
        if (pos == EPitchPosition.STRIKE)
            DecreaseSwingCount();
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
    public void OnSwing()
    {
        //StartCoroutine(Swing());
        var touch = Touchscreen.current.primaryTouch;
        int fingerId = touch.touchId.ReadValue();

        if (EventSystem.current.IsPointerOverGameObject(fingerId)) { print("스윙2"); return; }

        if (GameState == EGameState.Playing) EventManager.Instance.PublishSwing();
        else print("스윙4");
    }

    IEnumerator Swing()
    {

        //OnSwing()은 발동즉시 호출인데 이때 IsPointerOverGameObject는 업데이트 전일 수 있기에 한프레임 지나서 처리.
        yield return null;
        var touch = Touchscreen.current.primaryTouch;
        int fingerId = touch.touchId.ReadValue();

        if (EventSystem.current.IsPointerOverGameObject(fingerId)) { print("스윙2"); yield break; }
        print("스윙3");

        if (GameState == EGameState.Playing) EventManager.Instance.PublishSwing();
        else print("스윙4");
    }

    void TouchAndStart()
    {
        print("Game Start");
        GameState = EGameState.Playing;
        pitchCoroutine = StartCoroutine(StartPitching());
    }

    void GameIsReady()
    {
        GameState = EGameState.Intro;
        print("Loading Complete");
    }

    void GameFinished()
    {
        GameState = EGameState.Finish;
        print("Finish");
    }

    void OnDisable()
    {
        // 이벤트 구독 해제
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnBallSwing -= DecreaseSwingCount;
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