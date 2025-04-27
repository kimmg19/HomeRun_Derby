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
    [SerializeField] float strikeChance=0f;
    [HideInInspector] public Animator animator;
    StrikeZoneManager strikeZoneManager;
    BallSpeedManager ballSpeedManager;

    // ���� ������
    [SerializeField]Ball ball;
    Vector3 catchPoint;
    float finalSpeed;
    public EPitchPosition PitchPosition { get; set; }
    IPitchData pitchData;
    PitchTypeDataSO curPitchType;
    // ���� ����
    IPitcherState currentIState;
    public PitchState CurrentPitchState { get; set; }
    PitcherIdleState idleState;
    PitcherReadyState readyState;
    PitcherWindUpState windUpState;
    PitcherThrowState throwState;

    void Awake()
    {
        strikeZoneManager = GetComponent<StrikeZoneManager>();
        ballSpeedManager = GetComponent<BallSpeedManager>();
        animator = GetComponent<Animator>();

        idleState = new PitcherIdleState();
        readyState = new PitcherReadyState();
        windUpState = new PitcherWindUpState();
        throwState = new PitcherThrowState();
    }

    // �̺�Ʈ ���/����
    void OnEnable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnPitchStart += Pitching;
        }
        else Debug.LogError("�̺�Ʈ ��� ����");
    }
   
    // ���� ���� - Ȩ������ �Ŵ������� ȣ��
    public void Pitching(int currentDifficulty)
    {        

        // ���� ���ð� ���� ����
        curPitchType = pitchTypeDataSO[UnityEngine.Random.Range(0, pitchTypeDataSO.Length)];
        SetPitching(curPitchType, currentDifficulty);

        // �غ� ���·� ��ȯ
        ChangeState(readyState);
    }

    // ���� ���� �� ���̵��� ���� ���� ����
    void SetPitching(IPitchData pitchData, int difficulty)
    {
        // ��Ʈ����ũ/�� ����
        PitchPosition = UnityEngine.Random.value < strikeChance ?
            EPitchPosition.STRIKE : EPitchPosition.BALL;

        // ���� ��ġ ����
        catchPoint = strikeZoneManager.SetPitchingPoint(PitchPosition);

        // ���� �� ���� ����
        this.pitchData = pitchData;
        finalSpeed = ballSpeedManager.SetBallSpeed(pitchData, difficulty);
    }

    // ���� ���� - �ִϸ��̼� �̺�Ʈ�� ���¿��� ȣ��
    public void ExecutePitch()
    {       

        // �� ���� �� �ʱ�ȭ
        ball = ObjectPoolManager.Instance.GetBall();
        HomerunDerbyManager.Instance.CurrentBall = ball;

        // �� ���� �̺�Ʈ�� ����-ui���� ǥ��.
        EventManager.Instance.PublishOnSetBallData(finalSpeed, PitchPosition, curPitchType.pitchType);

        // ���� ����
        ball.Init(pitchData, PitchPosition, (int)finalSpeed, ball.transform.position, catchPoint);
        ball.Pitch();
    }

    public void OnWindUpStart()
    {
        EventManager.Instance.PublishWindUpStart();
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

    // ������ �Ϸ�->�غ�. �ִϸ��̼� �̺�Ʈ
    public void OnPitchCompleteEvent()
    {
        ChangeState(idleState);
    }

    // ���� ���� �޼ҵ�
    public void ChangeState(IPitcherState newState)
    {
        //currentIState?.Exit(this);
        currentIState = newState;
        currentIState?.Enter(this);
    }

    void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnPitchStart -= Pitching;
        }
        else Debug.LogError("�̺�Ʈ ���� ����");
    }
}