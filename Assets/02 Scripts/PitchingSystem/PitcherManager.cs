using System;
using UnityEngine;

public enum PitchState
{
    Ready,
    Throw
}

public class PitcherManager : MonoBehaviour
{
    [Header("투구 설정")]
    [SerializeField] PitchTypeDataSO[] pitchTypeDataSO;
    [SerializeField] float strikeChance=0f;
    [HideInInspector] public Animator animator;
    StrikeZoneManager strikeZoneManager;
    BallSpeedManager ballSpeedManager;

    // 투구 데이터
    [SerializeField]Ball ball;
    Vector3 catchPoint;
    float finalSpeed;
    public EPitchPosition PitchPosition { get; set; }
    IPitchData pitchData;
    PitchTypeDataSO curPitchType;
    // 상태 관리
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

    // 이벤트 등록/해제
    void OnEnable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnPitchStart += Pitching;
        }
        else Debug.LogError("이벤트 등록 실패");
    }
   
    // 투구 시작 - 홈런더비 매니저에서 호출
    public void Pitching(int currentDifficulty)
    {        

        // 구종 선택과 투구 설정
        curPitchType = pitchTypeDataSO[UnityEngine.Random.Range(0, pitchTypeDataSO.Length)];
        SetPitching(curPitchType, currentDifficulty);

        // 준비 상태로 전환
        ChangeState(readyState);
    }

    // 구종 설정 및 난이도에 따른 구속 설정
    void SetPitching(IPitchData pitchData, int difficulty)
    {
        // 스트라이크/볼 결정
        PitchPosition = UnityEngine.Random.value < strikeChance ?
            EPitchPosition.STRIKE : EPitchPosition.BALL;

        // 투구 위치 설정
        catchPoint = strikeZoneManager.SetPitchingPoint(PitchPosition);

        // 구종 및 구속 설정
        this.pitchData = pitchData;
        finalSpeed = ballSpeedManager.SetBallSpeed(pitchData, difficulty);
    }

    // 투구 실행 - 애니메이션 이벤트나 상태에서 호출
    public void ExecutePitch()
    {       

        // 공 생성 및 초기화
        ball = ObjectPoolManager.Instance.GetBall();
        HomerunDerbyManager.Instance.CurrentBall = ball;

        // 공 정보 이벤트에 보냄-ui에서 표시.
        EventManager.Instance.PublishOnSetBallData(finalSpeed, PitchPosition, curPitchType.pitchType);

        // 투구 시작
        ball.Init(pitchData, PitchPosition, (int)finalSpeed, ball.transform.position, catchPoint);
        ball.Pitch();
    }

    public void OnWindUpStart()
    {
        EventManager.Instance.PublishWindUpStart();
    }
    // 와인드업 애니메이션 완료 시 호출
    public void OnWindUpComplete()
    {
        ChangeState(throwState);
    }

    // 준비 애니메이션 완료 시 호출
    public void OnReadyComplete()
    {
        ChangeState(windUpState);
    }

    // 던지기 완료->준비. 애니메이션 이벤트
    public void OnPitchCompleteEvent()
    {
        ChangeState(idleState);
    }

    // 상태 변경 메소드
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
        else Debug.LogError("이벤트 해제 실패");
    }
}