using System;
using UnityEngine;

public class HitterManager : MonoBehaviour
{
    [Header("Ÿ�� ����")]
    [SerializeField] Transform hittingPoint;
    [SerializeField,ReadOnly] float baseDistance = 40f;
    [SerializeField, Range(5f, 25f)] float maxHeight = 15f;

    [Header("�÷��̾� ����")]
    [SerializeField] PlayerManager playerManager;

    // �� ����
    Ball currentBall;

    // ������Ʈ ����
    [HideInInspector] public Animator animator;

    // ��� Ŭ������
    HitTrajectoryCalculator trajectoryCalculator;
    HitQualityEvaluator qualityEvaluator;
    HitStatCalculator hitStatCalculator;

    // ���� ����
    IHitterState currentState;
    HitterReadyState hitterReadyState;
    HitterHitReadyState hitterHitReadyState;
    HitterHitState hitterHitState;

    EHitTiming hitTiming;

    void Awake()
    {
        // ������Ʈ �ʱ�ȭ
        animator = GetComponent<Animator>();

        // ���� ��ü �ʱ�ȭ
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
        // �̺�Ʈ ����
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnWindUpStart += OnReady;
            EventManager.Instance.OnSwingStart += Swing;
        }
        else Debug.LogError("HitterManager �̺�Ʈ ��� ����");
    }

    void OnDisable()
    {
        // �̺�Ʈ ���� ����
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnWindUpStart -= OnReady;
            EventManager.Instance.OnSwingStart -= Swing;
        }
        else Debug.LogError("HitterManager �̺�Ʈ ���� ����");

    }

    float GetPlayerStat(Func<PlayerManager, float> statSelector, float defaultValue = 0f)
    {
        return playerManager != null ? statSelector(playerManager) : defaultValue;
    }

    // Ÿ�� ���� �� ��� ó��
    public void CheckHit()
    {        

        currentBall = HomerunDerbyManager.Instance.CurrentBall;
        if (currentBall == null) return;

        // Ÿ�̹� ����
        float distanceFromHitPoint = hittingPoint.transform.position.z - currentBall.transform.position.z;
        hitTiming = qualityEvaluator.EvaluateHitQuality(distanceFromHitPoint);        

        if (hitTiming == EHitTiming.Miss) return;   // Ÿ�� Ÿ�̹� �̽�

        // �� ���� Ÿ�� ���� ���� (������ ����)
        if (currentBall.PitchPosition == EPitchPosition.BALL)
        {
            float eyeSight = GetPlayerStat(pm => pm.CurrentEyeSight);

            if (!hitStatCalculator.CheckBallHitSuccess(eyeSight))
            {
                Debug.Log("Ball Swing - Fail");     // Ÿ�� Ÿ�̹� �������� ���� �꽺��
                return;
            }
        }

        // Ÿ�� ���� ó��
        ProcessHit();
    }

    // Ÿ�� ��� ó��
    void ProcessHit()
    {
        // ���� ��������
        float power = GetPlayerStat(pm => pm.CurrentPower);
        float criticalChance = GetPlayerStat(pm => pm.CurrentCritical);
        EventManager.Instance.PublishEnableBallData(true);

        //Debug.Log("power: " + power);
        //Debug.Log("CriticalChance: " + criticalChance);

        // ũ��Ƽ�� ����
        bool isCritical = hitStatCalculator.CheckCriticalHit(criticalChance);
        

        // Ÿ�ֿ̹� ���� ���� ���
        float angle = trajectoryCalculator.CalculateHitAngle(hitTiming);

        // ���� ��� ��Ÿ� ��� (Ȯ����)
        float distance = hitStatCalculator.CalculateHitDistance(baseDistance, hitTiming, power, isCritical);        

        // Ÿ�� �̺�Ʈ ����
        EventManager.Instance.PublishBallHit(hitTiming, distance,isCritical);

        // �� ���� ������ ���
        Vector3 startPoint = currentBall.transform.position;
        Vector3 offset1, offset2, endPoint;
        trajectoryCalculator.CalculateTrajectory(startPoint, angle, distance, maxHeight,
                                              out offset1, out offset2, out endPoint);

        // �� ���� �ִϸ��̼� ����
        StartCoroutine(currentBall.ApplyHitMovement(startPoint, offset1, offset2, endPoint));
    }

    // ���� ���� �޼ҵ�
    void ChangeState(IHitterState newState)
    {
        //currentState?.Exit(this);
        currentState = newState;
        currentState?.Enter(this);
    }

    // �ܺ� �̺�Ʈ�� ���� ���� ��ȯ �޼ҵ�
    public void Swing()
    {
        ChangeState(hitterHitState);
    }

    // ���� ���ε�� ���� �� ȣ��Ǵ� �̺�Ʈ
    public void OnReady()
    {        
        ChangeState(hitterHitReadyState);
    }
    

    // �ִϸ��̼� �̺�Ʈ���� ȣ��Ǵ� �޼ҵ��
    public void OnToSwingReady()
    {
        ChangeState(hitterReadyState);
    }

    public void OnSwingComplete()
    {
        ChangeState(hitterReadyState);
    }
}