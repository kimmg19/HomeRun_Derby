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
    [SerializeField]float pitchClock = 7;
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
    void CallDecreaseSwingCount()=>StartCoroutine(DecreaseSwingCount());

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
            //if (EventSystem.current.IsPointerOverGameObject(fingerId))
            //{
            //    Debug.LogError("UI 터치");
            //    return;
            //}
            EventManager.Instance.PublishSwingStart();

        }
    }

    IEnumerator Swing()
    {

        //OnSwing()은 발동즉시 호출인데 이때 IsPointerOverGameObject는 업데이트 전일 수 있기에 한프레임 지나서 처리.
        yield return null;
        var touch = Touchscreen.current.primaryTouch;
        int fingerId = touch.touchId.ReadValue();

        if (EventSystem.current.IsPointerOverGameObject(fingerId)) { print("스윙2"); yield break; }
        print("스윙3");

        if (GameState == EGameState.Playing) EventManager.Instance.PublishSwingStart();
        else print("스윙4");
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
        //print("Loading Complete");
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

        }
        else Debug.LogError("HomerunDerbyManager 이벤트 해제 실패");

        if (pitchCoroutine != null)
        {
            StopCoroutine(pitchCoroutine);
            pitchCoroutine = null;
        }
    }
}