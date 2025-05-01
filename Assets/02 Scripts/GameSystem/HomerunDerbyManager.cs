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
    public Ball CurrentBall { get; set; } // ���� ������ ���� ��
    public int SwingCount { get; private set; } = 15;
    [ReadOnly] public int currentDifficulty = 1;
    [SerializeField, ReadOnly] float pitchClock = 8;

    void OnEnable()
    {
        // �̺�Ʈ ����
        if (EventManager.Instance != null)
        {
            // ���� �̺�Ʈ ����
            EventManager.Instance.OnGameReady += GameIsReady;
            EventManager.Instance.OnGameFinished += GameFinished;
            EventManager.Instance.OnGameStart += TouchAndStart;
            EventManager.Instance.OnBallSwing += DecreaseSwingCount;
            EventManager.Instance.OnEnablePitchData += IsStrike;
        }
        else Debug.LogError("HomerunDerbyManager �̺�Ʈ ��� ����");
    }
    void Awake()
    {
        if (Instance == null || Instance != this)
            Instance = this;
    }
    void Start()
    {
        // ���� ���� �̺�Ʈ ����
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
        // ����� ���� UI � �˸�
        EventManager.Instance.PublishSwingCount(SwingCount);
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
        var touch = Touchscreen.current.primaryTouch;
        int fingerId = touch.touchId.ReadValue();

        if (EventSystem.current.IsPointerOverGameObject(fingerId)) { print("����2"); return; }

        if (GameState == EGameState.Playing) EventManager.Instance.PublishSwing();
        else print("����4");
    }

    IEnumerator Swing()
    {

        //OnSwing()�� �ߵ���� ȣ���ε� �̶� IsPointerOverGameObject�� ������Ʈ ���� �� �ֱ⿡ �������� ������ ó��.
        yield return null;
        var touch = Touchscreen.current.primaryTouch;
        int fingerId = touch.touchId.ReadValue();

        if (EventSystem.current.IsPointerOverGameObject(fingerId)) { print("����2"); yield break; }
        print("����3");

        if (GameState == EGameState.Playing) EventManager.Instance.PublishSwing();
        else print("����4");
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
        // �̺�Ʈ ���� ����
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnBallSwing -= DecreaseSwingCount;
            EventManager.Instance.OnGameReady -= GameIsReady;
            EventManager.Instance.OnGameFinished -= GameFinished;
            EventManager.Instance.OnGameStart -= TouchAndStart;

        }
        else Debug.LogError("HomerunDerbyManager �̺�Ʈ ���� ����");

        if (pitchCoroutine != null)
        {
            StopCoroutine(pitchCoroutine);
            pitchCoroutine = null;
        }
    }
}