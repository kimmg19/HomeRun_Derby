using System;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

// 투수 관리 클래스 - 상태 패턴을 이용해 투수의 행동을 관리
public class PitcherManager : MonoBehaviour
{
    [Header("투구 설정")]
    [SerializeField] PitchTypeDataSO[] pitchTypeDataSO;
    [SerializeField, Range(1, 3)] int currentDifficulty = 1;
    [SerializeField] float strikeChance = 0.7f;

    [Header("이벤트")]
    [SerializeField] public UnityEvent onWindUpStart;
    [SerializeField] public UnityEvent onPitchComplete;
    UnityEvent<Ball, float, EPitchPosition> onBallReleased;

    // 컴포넌트 참조
    [HideInInspector] public Animator animator;
    StrikeZoneManager strikeZoneManager;
    BallSpeedManager ballSpeedManager;
    ObjectPoolManager poolManager;
    HitterManager hitterManager;

    // 투구 데이터
    Ball ball;
    Vector3 catchPoint;
    float finalSpeed;
    EPitchPosition pitchPosition;
    IPitchData pitchData;

    // 상태 관리
    IPitcherState currentState;
    PitcherIdleState idleState;
    PitcherReadyState readyState;
    PitcherWindUpState windUpState;
    PitcherThrowState throwState;

    // 초기화 - 필요한 컴포넌트와 상태 객체를 생성
    void Awake()
    {
        // 컴포넌트 참조 초기화
        hitterManager = FindObjectOfType<HitterManager>();
        strikeZoneManager = GetComponent<StrikeZoneManager>();
        ballSpeedManager = GetComponent<BallSpeedManager>();
        poolManager = GetComponent<ObjectPoolManager>();
        animator = GetComponent<Animator>();

        // 상태 객체 초기화
        idleState = new PitcherIdleState();
        readyState = new PitcherReadyState();
        windUpState = new PitcherWindUpState();
        throwState = new PitcherThrowState();
    }

    // 투구 시작 - 홈런더비 매니저에서 호출
    public void Pitching()
    {
        if (ball != null)
            poolManager.ReturnBall(ball);

        // 구종 선택과 투구 설정
        var randomPitchType = pitchTypeDataSO[UnityEngine.Random.Range(0, pitchTypeDataSO.Length)];
        SetPitching(randomPitchType, currentDifficulty);

        // 준비 상태로 전환
        ChangeState(readyState);
    }

    // 구종 설정 및 난이도에 따른 구속 설정
    void SetPitching(IPitchData pitchData, int difficulty)
    {
        // 스트라이크/볼 결정
        pitchPosition = UnityEngine.Random.value < strikeChance ?
            EPitchPosition.Strike : EPitchPosition.Ball;

        // 투구 위치 설정
        catchPoint = strikeZoneManager.SetPitchingPoint(pitchPosition);

        // 구종 및 구속 설정
        this.pitchData = pitchData;
        finalSpeed = ballSpeedManager.SetBallSpeed(pitchData, difficulty);
    }

    // 투구 실행 - 애니메이션 이벤트나 상태에서 호출
    public void ExecutePitch()
    {
        // 타자에게 투구 완료 신호 전달
        onPitchComplete?.Invoke();

        // 공 생성 및 초기화
        ball = poolManager.GetBall();
        hitterManager.CurrentBall = ball;

        // 공 출발 이벤트 발생
        onBallReleased?.Invoke(ball, finalSpeed, pitchPosition);

        // 투구 시작
        ball.Init(pitchData, (int)finalSpeed, ball.transform.position, catchPoint);
        ball.Pitch();
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
    public void OnPitchComplete()
    {
        ChangeState(idleState);
    }
    // 상태 변경 메서드
    public void ChangeState(IPitcherState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState?.Enter(this);
    }
}