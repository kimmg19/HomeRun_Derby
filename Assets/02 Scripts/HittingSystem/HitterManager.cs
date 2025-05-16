using System;
using UnityEngine;

public class HitterManager : MonoBehaviour
{
    [Header("타격 설정")]
    [SerializeField] Transform hittingPoint;
    [SerializeField] float baseDistance = 50f;
    [SerializeField, Range(5f, 25f)] float maxHeight = 15f;
    float distanceCoefficient = 50f;//비거리 보정 +거리
    [Header("플레이어 스탯")]
    PlayerManager playerManager;

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
        
        playerManager=FindAnyObjectByType<PlayerManager>();        
        hitterReadyState = new HitterReadyState();
        hitterHitReadyState = new HitterHitReadyState();
        hitterHitState = new HitterHitState();
        animator = GetComponent<Animator>();
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
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.Swing);
        // 타이밍 판정
        float distanceFromHitPoint = hittingPoint.transform.position.z - currentBall.transform.position.z;
        hitTiming = qualityEvaluator.EvaluateHitQuality(distanceFromHitPoint);

        // 볼 판정 타격 성공 여부 (선구안 영향)
        if (currentBall.PitchPosition == EPitchLocation.BALL)
        {
            EventManager.Instance.PublishOnBallSwing();

            float eyeSight = GetPlayerStat(pm => pm.CurrentJudgeSight);

            if (!hitStatCalculator.CheckBallHitSuccess(eyeSight))
            {
                return;
            }
        }
        if (hitTiming == EHitTiming.Miss)
        {
            EventManager.Instance.PublishBallHit(hitTiming, 0, false);
            return;
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
        bool isCritical = hitStatCalculator.CheckCriticalHit(criticalChance);
        float angle = trajectoryCalculator.CalculateHitAngle(hitTiming);       
        float distance = hitStatCalculator.CalculateHitDistance(baseDistance, hitTiming, power, isCritical);
        float calculatedDistance = Mathf.Floor(distance * 10f) / 10f+ distanceCoefficient;//소수점 첫번째 까지만, +50
        // 타격 이벤트 발행
        EventManager.Instance.PublishBallHit(hitTiming, calculatedDistance, isCritical);
        EventManager.Instance.PublishHitEffect(currentBall.transform,hitTiming);
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.Hit);
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
        if (HomerunDerbyManager.Instance.CurrentBall == null) return;
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