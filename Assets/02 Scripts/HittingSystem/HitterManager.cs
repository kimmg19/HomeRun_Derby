using UnityEngine.Events;
using UnityEngine;

public class HitterManager : MonoBehaviour
{
    [Header("Ÿ�� ����")]
    [SerializeField] Transform hittingPoint;
    [SerializeField, Range(20f, 80f)] float baseDistance = 40f;  // �⺻ ��Ÿ� ����
    [SerializeField, Range(5f, 25f)] float maxHeight = 15f;      // �ִ� ���� ����

    [Header("�̺�Ʈ")]
    [SerializeField] UnityEvent<EHitTiming, float> onBallHit;    // Ÿ�� �̺�Ʈ
    [SerializeField] UnityEvent onMiss;                          // �̽� �̺�Ʈ

    // ������Ʈ ����
    [HideInInspector] public Animator animator;
    PitcherManager pm;
    public Ball CurrentBall { get; set; }

    // �и��� Ŭ���� �ν��Ͻ�
    HitTrajectoryCalculator trajectoryCalculator;
    HitQualityEvaluator qualityEvaluator;

    // ���� ���� (���� �ڵ� ����)
    IHitterState currentState;
    HitterReadyState hitterReadyState;        // ��� ����
    HitterHitReadyState hitterHitReadyState;  // ���� �غ� ����
    HitterHitState hitterHitState;            // ���� ����

    void Awake()
    {
        // ������Ʈ ���� �ʱ�ȭ
        animator = GetComponent<Animator>();
        pm = FindObjectOfType<PitcherManager>();

        // �и��� Ŭ���� �ʱ�ȭ
        trajectoryCalculator = new HitTrajectoryCalculator();
        qualityEvaluator = new HitQualityEvaluator();

        // ���� ��ü �ʱ�ȭ (���� �ڵ� ����)
        hitterReadyState = new HitterReadyState();
        hitterHitReadyState = new HitterHitReadyState();
        hitterHitState = new HitterHitState();

        // �̺�Ʈ ����
        if (pm != null)
        {
            pm.onPitchComplete.AddListener(OnReady);
        }
    }

    void Start()
    {
        // �ʱ� ���� ���� (���� �ڵ� ����)
        ChangeState(hitterReadyState);
    }

    // Ÿ�� ���� �� �� ����������
    public void CheckHit()
    {
        if (CurrentBall == null) return;

        // Ÿ�̹� ��� (�и��� Ŭ���� ���)
        float dis = hittingPoint.transform.position.z - CurrentBall.transform.position.z;
        EHitTiming hitTiming = qualityEvaluator.EvaluateHitQuality(dis);

        if (hitTiming == EHitTiming.Miss)
        {
            Debug.Log("�̽�");
            onMiss?.Invoke();
            return;
        }

        // Ÿ�ֿ̹� ���� ���� ��� (�и��� Ŭ���� ���)
        float angle = trajectoryCalculator.CalculateHitAngle(hitTiming);

        // ��Ÿ� ���
        float distance = baseDistance;

        // Ÿ�� �̺�Ʈ �߻�
        onBallHit?.Invoke(hitTiming, distance);

        // ������ � ������ ��� (�и��� Ŭ���� ���)
        Vector3 startPoint = hittingPoint.position;
        Vector3 offset1, offset2, endPoint;
        trajectoryCalculator.CalculateTrajectory(startPoint, angle, distance, maxHeight, out offset1, out offset2, out endPoint);

        // Ÿ�� ����
        StartCoroutine(CurrentBall.ApplyHitMovement(startPoint, offset1, offset2, endPoint));
    }

    // ���� ���� �޼���� (���� �ڵ� ����)
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