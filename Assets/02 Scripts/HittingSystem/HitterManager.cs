using System;
using UnityEngine;

public class HitterManager : MonoBehaviour
{
    [Header("타격 설정")]
    [SerializeField] Transform hittingPoint;
    [SerializeField,ReadOnly] float baseDistance = 40f;
    [SerializeField, Range(5f, 25f)] float maxHeight = 15f;

    [Header("플레이어 스탯")]
    [SerializeField] PlayerManager playerManager;

    // 공 참조
    Ball currentBall;

    // 컴포넌트 참조
    [HideInInspector] public Animator animator;

    // 계산 클래스들
    HitTrajectoryCalculator trajectoryCalculator;
    HitQualityEvaluator qualityEvaluator;
    HitStatCalculator hitStatCalculator;

    // 상태 관리
    IHitterState currentState;
    HitterReadyState hitterReadyState;
    HitterHitReadyState hitterHitReadyState;
    HitterHitState hitterHitState;

    EHitTiming hitTiming;

    void Awake()
    {
        // 컴포넌트 초기화
        animator = GetComponent<Animator>();

        // 상태 객체 초기화
        hitterReadyState = new HitterReadyState();
        hitterHitReadyState = new HitterHitReadyState();
        hitterHitState = new HitterHitState();

        trajectoryCalculator = GetComponent<HitTrajectoryCalculator>();
        qualityEvaluator = GetComponent<HitQualityEvaluator>();
        hitStatCalculator = GetComponent<HitStatCalculator>();

        if (qualityEvaluator == null || trajectoryCalculator == null || hitStatCalculator == null)
        {
            Debug.LogError("Calculator Null!");
        }
    }

    void Start()
    {
        ChangeState(hitterReadyState);
    }

    void OnEnable()
    {
        // 이벤트 구독
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnWindUpStart += OnReady;
            EventManager.Instance.OnSwingStart += Swing;
        }
        else Debug.LogError("HitterManager 이벤트 등록 실패");
    }

    void OnDisable()
    {
        // 이벤트 구독 해제
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnWindUpStart -= OnReady;
            EventManager.Instance.OnSwingStart -= Swing;
        }
        else Debug.LogError("HitterManager 이벤트 해제 실패");

    }

    float GetPlayerStat(Func<PlayerManager, float> statSelector, float defaultValue = 0f)
    {
        return playerManager != null ? statSelector(playerManager) : defaultValue;
    }

    // 타격 판정 및 결과 처리
    public void CheckHit()
    {        

        currentBall = HomerunDerbyManager.Instance.CurrentBall;
        if (currentBall == null) return;

        // 타이밍 판정
        float distanceFromHitPoint = hittingPoint.transform.position.z - currentBall.transform.position.z;
        hitTiming = qualityEvaluator.EvaluateHitQuality(distanceFromHitPoint);        

        if (hitTiming == EHitTiming.Miss) return;   // 타격 타이밍 미스

        // 볼 판정 타격 성공 여부 (선구안 영향)
        if (currentBall.PitchPosition == EPitchPosition.BALL)
        {
            float eyeSight = GetPlayerStat(pm => pm.CurrentEyeSight);

            if (!hitStatCalculator.CheckBallHitSuccess(eyeSight))
            {
                Debug.Log("Ball Swing - Fail");     // 타격 타이밍 좋았지만 볼에 헛스윙
                return;
            }
        }

        // 타격 성공 처리
        ProcessHit();
    }

    // 타격 결과 처리
    void ProcessHit()
    {
        // 스탯 가져오기
        float power = GetPlayerStat(pm => pm.CurrentPower);
        float criticalChance = GetPlayerStat(pm => pm.CurrentCritical);
        EventManager.Instance.PublishEnableBallData(true);

        //Debug.Log("power: " + power);
        //Debug.Log("CriticalChance: " + criticalChance);

        // 크리티컬 판정
        bool isCritical = hitStatCalculator.CheckCriticalHit(criticalChance);
        

        // 타이밍에 따른 방향 계산
        float angle = trajectoryCalculator.CalculateHitAngle(hitTiming);

        // 스탯 기반 비거리 계산 (확률적)
        float distance = hitStatCalculator.CalculateHitDistance(baseDistance, hitTiming, power, isCritical);        

        // 타격 이벤트 발행
        EventManager.Instance.PublishBallHit(hitTiming, distance,isCritical);

        // 공 궤적 제어점 계산
        Vector3 startPoint = currentBall.transform.position;
        Vector3 offset1, offset2, endPoint;
        trajectoryCalculator.CalculateTrajectory(startPoint, angle, distance, maxHeight,
                                              out offset1, out offset2, out endPoint);

        // 공 궤적 애니메이션 실행
        StartCoroutine(currentBall.ApplyHitMovement(startPoint, offset1, offset2, endPoint));
    }

    // 상태 변경 메소드
    void ChangeState(IHitterState newState)
    {
        //currentState?.Exit(this);
        currentState = newState;
        currentState?.Enter(this);
    }

    // 외부 이벤트에 의한 상태 전환 메소드
    public void Swing()
    {
        ChangeState(hitterHitState);
    }

    // 투수 와인드업 시작 시 호출되는 이벤트
    public void OnReady()
    {        
        ChangeState(hitterHitReadyState);
    }
    

    // 애니메이션 이벤트에서 호출되는 메소드들
    public void OnToSwingReady()
    {
        ChangeState(hitterReadyState);
    }

    public void OnSwingComplete()
    {
        ChangeState(hitterReadyState);
    }
}