using System;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

// ���� ���� Ŭ���� - ���� ������ �̿��� ������ �ൿ�� ����
public class PitcherManager : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField] PitchTypeDataSO[] pitchTypeDataSO;
    [SerializeField, Range(1, 3)] int currentDifficulty = 1;
    [SerializeField] float strikeChance = 0.7f;

    [Header("�̺�Ʈ")]
    [SerializeField] public UnityEvent onWindUpStart;
    [SerializeField] public UnityEvent onPitchComplete;
    UnityEvent<Ball, float, EPitchPosition> onBallReleased;

    // ������Ʈ ����
    [HideInInspector] public Animator animator;
    StrikeZoneManager strikeZoneManager;
    BallSpeedManager ballSpeedManager;
    ObjectPoolManager poolManager;
    HitterManager hitterManager;

    // ���� ������
    Ball ball;
    Vector3 catchPoint;
    float finalSpeed;
    EPitchPosition pitchPosition;
    IPitchData pitchData;

    // ���� ����
    IPitcherState currentState;
    PitcherIdleState idleState;
    PitcherReadyState readyState;
    PitcherWindUpState windUpState;
    PitcherThrowState throwState;

    // �ʱ�ȭ - �ʿ��� ������Ʈ�� ���� ��ü�� ����
    void Awake()
    {
        // ������Ʈ ���� �ʱ�ȭ
        hitterManager = FindObjectOfType<HitterManager>();
        strikeZoneManager = GetComponent<StrikeZoneManager>();
        ballSpeedManager = GetComponent<BallSpeedManager>();
        poolManager = GetComponent<ObjectPoolManager>();
        animator = GetComponent<Animator>();

        // ���� ��ü �ʱ�ȭ
        idleState = new PitcherIdleState();
        readyState = new PitcherReadyState();
        windUpState = new PitcherWindUpState();
        throwState = new PitcherThrowState();
    }

    // ���� ���� - Ȩ������ �Ŵ������� ȣ��
    public void Pitching()
    {
        if (ball != null)
            poolManager.ReturnBall(ball);

        // ���� ���ð� ���� ����
        var randomPitchType = pitchTypeDataSO[UnityEngine.Random.Range(0, pitchTypeDataSO.Length)];
        SetPitching(randomPitchType, currentDifficulty);

        // �غ� ���·� ��ȯ
        ChangeState(readyState);
    }

    // ���� ���� �� ���̵��� ���� ���� ����
    void SetPitching(IPitchData pitchData, int difficulty)
    {
        // ��Ʈ����ũ/�� ����
        pitchPosition = UnityEngine.Random.value < strikeChance ?
            EPitchPosition.Strike : EPitchPosition.Ball;

        // ���� ��ġ ����
        catchPoint = strikeZoneManager.SetPitchingPoint(pitchPosition);

        // ���� �� ���� ����
        this.pitchData = pitchData;
        finalSpeed = ballSpeedManager.SetBallSpeed(pitchData, difficulty);
    }

    // ���� ���� - �ִϸ��̼� �̺�Ʈ�� ���¿��� ȣ��
    public void ExecutePitch()
    {
        // Ÿ�ڿ��� ���� �Ϸ� ��ȣ ����
        onPitchComplete?.Invoke();

        // �� ���� �� �ʱ�ȭ
        ball = poolManager.GetBall();
        hitterManager.CurrentBall = ball;

        // �� ��� �̺�Ʈ �߻�
        onBallReleased?.Invoke(ball, finalSpeed, pitchPosition);

        // ���� ����
        ball.Init(pitchData, (int)finalSpeed, ball.transform.position, catchPoint);
        ball.Pitch();
    }

    // ���ε�� �ִϸ��̼� �Ϸ� �� ȣ��
    public void OnWindUpComplete()
    {
        ChangeState(throwState);
    }

    // �غ� �ִϸ��̼� �Ϸ� �� ȣ��
    public void OnReadyComplete()
    {
        ChangeState(windUpState);
    }
    public void OnPitchComplete()
    {
        ChangeState(idleState);
    }
    // ���� ���� �޼���
    public void ChangeState(IPitcherState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState?.Enter(this);
    }
}