using System.Collections;
using UnityEngine;
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
    static HomerunDerbyManager instance;
    public static HomerunDerbyManager Instance
    {
        get { return instance; }
    }

    public EGameState GameState { get; set; }
    Coroutine pitchCoroutine;
    public Ball CurrentBall { get; set; } // 현재 투수가 던진 공
    public int SwingCount { get; set; } = 3;
    [ReadOnly] public int currentDifficulty = 1;
    [SerializeField] float pitchClock = 7;
    void Awake()
    {
        if (instance == null || instance != this)
            instance = this;
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
            EventManager.Instance.OnBallSwing += CallDecreaseSwingCount;
            EventManager.Instance.OnEnablePitchData += IsStrike;
            EventManager.Instance.OnSwing += OnSwing;
        }
        else Debug.LogError("HomerunDerbyManager 이벤트 등록 실패");
    }

    void Start()
    {
        // 게임 시작 이벤트 발행
        EventManager.Instance.PublishGameReady();
    }
    //카운트는 볼에 스윙 or 스트라이크 인 경우 감소. 
    void IsStrike(EPitchLocation pos)
    {
        if (pos == EPitchLocation.STRIKE)
            CallDecreaseSwingCount();
    }
    void CallDecreaseSwingCount() => StartCoroutine(DecreaseSwingCount());

    IEnumerator DecreaseSwingCount()
    {
        SwingCount--;
        if (SwingCount > 9 && SwingCount <= 12) currentDifficulty = 2;
        else if (SwingCount > 6 && SwingCount <= 9) currentDifficulty = 3;
        else if (SwingCount > 3 && SwingCount <= 6) currentDifficulty = 4;
        else if (SwingCount <= 3) currentDifficulty = 5;
        // 변경된 값을 UI 등에 알림
        EventManager.Instance.PublishSwingCount(SwingCount);
        yield return new WaitForSeconds(0.5f);//타격 데이터 set 할 때 까지 기다림.
        ScoreManager.Instance.SetRecord();
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
        if (GameState == EGameState.Playing)
        {
            var touch = Touchscreen.current.primaryTouch;
            int fingerId = touch.touchId.ReadValue();
            EventManager.Instance.PublishSwingStart();
        }
    }
    void TouchAndStart()
    {
        //print("Game Start");
        GameState = EGameState.Playing;
        pitchCoroutine = StartCoroutine(StartPitching());
    }

    void GameIsReady()
    {
        GameState = EGameState.Intro;
        SoundManager.Instance.PlayBGM(SoundManager.EBgm.Crowd);
    }

    void GameFinished()
    {
        GameState = EGameState.Finish;
    }

    void OnDisable()
    {
        // 이벤트 구독 해제
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnBallSwing -= CallDecreaseSwingCount;
            EventManager.Instance.OnGameReady -= GameIsReady;
            EventManager.Instance.OnGameFinished -= GameFinished;
            EventManager.Instance.OnGameStart -= TouchAndStart;
            EventManager.Instance.OnSwing -= OnSwing;
            EventManager.Instance.OnEnablePitchData -= IsStrike;
        }
        else Debug.LogError("HomerunDerbyManager 이벤트 해제 실패");

        if (pitchCoroutine != null)
        {
            StopCoroutine(pitchCoroutine);
            pitchCoroutine = null;
        }
    }
}