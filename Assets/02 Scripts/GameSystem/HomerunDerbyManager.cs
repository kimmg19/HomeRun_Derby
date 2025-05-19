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
    public Ball CurrentBall { get; set; } // ���� ������ ���� ��
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
        // �̺�Ʈ ����
        if (EventManager.Instance != null)
        {
            // ���� �̺�Ʈ ����
            EventManager.Instance.OnGameReady += GameIsReady;
            EventManager.Instance.OnGameFinished += GameFinished;
            EventManager.Instance.OnGameStart += TouchAndStart;
            EventManager.Instance.OnBallSwing += CallDecreaseSwingCount;
            EventManager.Instance.OnEnablePitchData += IsStrike;
            EventManager.Instance.OnSwing += OnSwing;
        }
        else Debug.LogError("HomerunDerbyManager �̺�Ʈ ��� ����");
    }

    void Start()
    {
        // ���� ���� �̺�Ʈ ����
        EventManager.Instance.PublishGameReady();
    }
    //ī��Ʈ�� ���� ���� or ��Ʈ����ũ �� ��� ����. 
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
        // ����� ���� UI � �˸�
        EventManager.Instance.PublishSwingCount(SwingCount);
        yield return new WaitForSeconds(0.5f);//Ÿ�� ������ set �� �� ���� ��ٸ�.
        ScoreManager.Instance.SetRecord();
    }

    IEnumerator StartPitching()
    {
        while (SwingCount > 0 && GameState == EGameState.Playing)
        {
            EventManager.Instance.PublishPitch(currentDifficulty);
            yield return new WaitForSeconds(pitchClock);
        }

        // ���� ���� �̺�Ʈ ����
        EventManager.Instance.PublishGameFinished();
    }

    // ��ġ ó��
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
        // �̺�Ʈ ���� ����
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnBallSwing -= CallDecreaseSwingCount;
            EventManager.Instance.OnGameReady -= GameIsReady;
            EventManager.Instance.OnGameFinished -= GameFinished;
            EventManager.Instance.OnGameStart -= TouchAndStart;
            EventManager.Instance.OnSwing -= OnSwing;
            EventManager.Instance.OnEnablePitchData -= IsStrike;
        }
        else Debug.LogError("HomerunDerbyManager �̺�Ʈ ���� ����");

        if (pitchCoroutine != null)
        {
            StopCoroutine(pitchCoroutine);
            pitchCoroutine = null;
        }
    }
}