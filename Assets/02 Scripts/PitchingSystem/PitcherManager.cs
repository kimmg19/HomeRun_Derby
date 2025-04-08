using System;
using UnityEngine;

public enum PitchState
{
    Ready,
    Throw
}

public class PitcherManager : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField] PitchTypeDataSO[] pitchTypeDataSO;
    [SerializeField, Range(1, 3)] int currentDifficulty = 1;
    [SerializeField] float strikeChance = 0.7f;

    [HideInInspector] public Animator animator;
    StrikeZoneManager strikeZoneManager;
    BallSpeedManager ballSpeedManager;
    ObjectPoolManager poolManager;
    HitterManager hitterManager;

    // ���� ������
    Ball ball;
    Vector3 catchPoint;
    float finalSpeed;
    public EPitchPosition pitchPosition;
    IPitchData pitchData;

    // ���� ����
    IPitcherState current_IState;
    public PitchState eState;
    PitcherIdleState idleState;
    PitcherReadyState readyState;
    PitcherWindUpState windUpState;
    PitcherThrowState throwState;

    void Awake()
    {
        hitterManager = FindObjectOfType<HitterManager>();
        strikeZoneManager = GetComponent<StrikeZoneManager>();
        ballSpeedManager = GetComponent<BallSpeedManager>();
        poolManager = GetComponent<ObjectPoolManager>();
        animator = GetComponent<Animator>();

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
        HomerunDerbyManager.Instance.TriggerPitchComplete();

        // �� ���� �� �ʱ�ȭ
        ball = poolManager.GetBall();
        hitterManager.CurrentBall = ball;

        // �� ��� �̺�Ʈ �߻�
        HomerunDerbyManager.Instance.TriggerBallReleased(ball, finalSpeed, pitchPosition);

        // ���� ����
        ball.Init(pitchData, pitchPosition, (int)finalSpeed, ball.transform.position, catchPoint);
        ball.Pitch();
    }

    // ���ε�� �ִϸ��̼� �Ϸ� �� ȣ��->����
    public void OnWindUpComplete()
    {
        ChangeState(throwState);
    }

    // �غ� �ִϸ��̼� �Ϸ� �� ȣ��->���ε��
    public void OnReadyComplete()
    {
        ChangeState(windUpState);
    }

    //������ �Ϸ�->�غ�
    public void OnPitchComplete()
    {
        ChangeState(idleState);
    }

    // ���� ���� �޼���
    public void ChangeState(IPitcherState newState)
    {
        current_IState?.Exit(this);
        current_IState = newState;
        current_IState?.Enter(this);
    }
}