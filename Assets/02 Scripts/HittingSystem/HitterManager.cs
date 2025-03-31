using UnityEngine.Events;
using UnityEngine;

public class HitterManager : MonoBehaviour
{
    [Header("타격 설정")]
    [SerializeField] Transform hittingPoint;
    [SerializeField, Range(20f, 80f)] float baseDistance = 40f;  // 기본 비거리 설정
    [SerializeField, Range(5f, 25f)] float maxHeight = 15f;      // 최대 높이 설정

    [Header("이벤트")]
    [SerializeField] UnityEvent<EHitTiming, float> onBallHit;    // 타격 이벤트
    [SerializeField] UnityEvent onMiss;                          // 미스 이벤트

    // 컴포넌트 참조
    [HideInInspector] public Animator animator;
    PitcherManager pm;
    public Ball CurrentBall { get; set; }

    // 분리된 클래스 인스턴스
    HitTrajectoryCalculator trajectoryCalculator;
    HitQualityEvaluator qualityEvaluator;

    // 상태 관리 (기존 코드 유지)
    IHitterState currentState;
    HitterReadyState hitterReadyState;        // 대기 상태
    HitterHitReadyState hitterHitReadyState;  // 스윙 준비 상태
    HitterHitState hitterHitState;            // 스윙 상태

    void Awake()
    {
        // 컴포넌트 참조 초기화
        animator = GetComponent<Animator>();
        pm = FindObjectOfType<PitcherManager>();

        // 분리된 클래스 초기화
        trajectoryCalculator = new HitTrajectoryCalculator();
        qualityEvaluator = new HitQualityEvaluator();

        // 상태 객체 초기화 (기존 코드 유지)
        hitterReadyState = new HitterReadyState();
        hitterHitReadyState = new HitterHitReadyState();
        hitterHitState = new HitterHitState();

        // 이벤트 구독
        if (pm != null)
        {
            pm.onPitchComplete.AddListener(OnReady);
        }
    }

    void Start()
    {
        // 초기 상태 설정 (기존 코드 유지)
        ChangeState(hitterReadyState);
    }

    // 타격 판정 및 공 날려보내기
    public void CheckHit()
    {
        if (CurrentBall == null) return;

        // 타이밍 계산 (분리된 클래스 사용)
        float dis = hittingPoint.transform.position.z - CurrentBall.transform.position.z;
        EHitTiming hitTiming = qualityEvaluator.EvaluateHitQuality(dis);

        if (hitTiming == EHitTiming.Miss)
        {
            Debug.Log("미스");
            onMiss?.Invoke();
            return;
        }

        // 타이밍에 따른 각도 계산 (분리된 클래스 사용)
        float angle = trajectoryCalculator.CalculateHitAngle(hitTiming);

        // 비거리 계산
        float distance = baseDistance;

        // 타격 이벤트 발생
        onBallHit?.Invoke(hitTiming, distance);

        // 베지어 곡선 제어점 계산 (분리된 클래스 사용)
        Vector3 startPoint = hittingPoint.position;
        Vector3 offset1, offset2, endPoint;
        trajectoryCalculator.CalculateTrajectory(startPoint, angle, distance, maxHeight, out offset1, out offset2, out endPoint);

        // 타구 실행
        StartCoroutine(CurrentBall.ApplyHitMovement(startPoint, offset1, offset2, endPoint));
    }

    // 상태 관련 메서드들 (기존 코드 유지)
    public void OnSwing()
    {
        ChangeState(hitterHitState);
    }

    void OnReady()
    {
        ChangeState(hitterHitReadyState);
    }

    public void OnSwingReadyComplete()
    {
        ChangeState(hitterReadyState);
    }

    public void OnSwingComplete()
    {
        ChangeState(hitterReadyState);
    }

    void ChangeState(IHitterState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState?.Enter(this);
    }

    void OnDisable()
    {
        if (pm != null)
        {
            pm.onPitchComplete.RemoveListener(OnReady);
        }
    }
}