using System;
using System.Collections;
using UnityEngine;

public class PitchingManager : MonoBehaviour
{
    [SerializeField] PitchTypeDataSO[] pitchTypeDataSO;
    
    [Header("���� ���̵�")]
    [SerializeField] int currentDifficulty = 1;//���̵��� ���� ���� ����. ����� 1~3�ܰ�

    //[Header("��Ʈ����ũ Ȯ��")]
    const float strikeChance = 0.7f;
    StrikeZoneManager strikeZoneManager;
    BallSpeedManager ballSpeedManager;
    Animator animator;
    ObjectPoolManager poolManager;
    Ball ball=null;
    Vector3 endPoint;
    float finalSpeed;
    EPitchPosition pitchPosition;
    IPitchData pitchData;
    public Action ad;
    void Awake()
    {
        strikeZoneManager = GetComponent<StrikeZoneManager>();
        ballSpeedManager=GetComponent<BallSpeedManager>();
        poolManager = GetComponent<ObjectPoolManager>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        StartCoroutine(StartPitching());
    }
    IEnumerator StartPitching()
    {
        while (true)
        {
            if (ball) poolManager.ReturnBall(ball);
            SetPitching(pitchTypeDataSO[UnityEngine.Random.Range(0, pitchTypeDataSO.Length)], currentDifficulty);//�켱�� ��ü ����.
            animator.SetTrigger("WindUp");            
            yield return new WaitForSeconds(7f);
        }
    }
    //���� ����.���̵��� ���� ���� ����.    
    void SetPitching(IPitchData iP, int currentDifficulty)
    {
        Debug.Log("���� ����");
        pitchPosition = UnityEngine.Random.value < strikeChance ? EPitchPosition.Strike : EPitchPosition.Ball;
        endPoint = strikeZoneManager.SetPitchingPoint(pitchPosition);
        pitchData = iP;
        finalSpeed = ballSpeedManager.SetBallSpeed(pitchData, currentDifficulty);
    }

    public void ApplyBallMovement()
    {
        ad?.Invoke();
        ball = poolManager.GetBall();
        ball.Init(pitchData, (int)finalSpeed, ball.transform.position, endPoint);
        ball.Pitch();
    }    
}
