using System;
using UnityEngine;

public class HitterManager : MonoBehaviour
{
    [Header("Ÿ�� ����")]
    [SerializeField] Transform hittingPoint;
    [SerializeField] float baseDistance = 50f;
    [SerializeField, Range(5f, 25f)] float maxHeight = 15f;
    float distanceCoefficient = 50f;//��Ÿ� ���� +�Ÿ�
    [Header("�÷��̾� ����")]
    PlayerManager playerManager;

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
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.Swing);
        // Ÿ�̹� ����
        float distanceFromHitPoint = hittingPoint.transform.position.z - currentBall.transform.position.z;
        hitTiming = qualityEvaluator.EvaluateHitQuality(distanceFromHitPoint);

        // �� ���� Ÿ�� ���� ���� (������ ����)
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

        // Ÿ�� ���� ó��
        ProcessHit();
    }

    // Ÿ�� ��� ó��
    void ProcessHit()
    {        
        // ���� ��������
        float power = GetPlayerStat(pm => pm.CurrentPower);
        float criticalChance = GetPlayerStat(pm => pm.CurrentCritical);        
        bool isCritical = hitStatCalculator.CheckCriticalHit(criticalChance);
        float angle = trajectoryCalculator.CalculateHitAngle(hitTiming);       
        float distance = hitStatCalculator.CalculateHitDistance(baseDistance, hitTiming, power, isCritical);
        float calculatedDistance = Mathf.Floor(distance * 10f) / 10f+ distanceCoefficient;//�Ҽ��� ù��° ������, +50
        // Ÿ�� �̺�Ʈ ����
        EventManager.Instance.PublishBallHit(hitTiming, calculatedDistance, isCritical);
        EventManager.Instance.PublishHitEffect(currentBall.transform,hitTiming);
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.Hit);
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
        if (HomerunDerbyManager.Instance.CurrentBall == null) return;
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