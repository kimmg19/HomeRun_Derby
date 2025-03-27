using System;
using System.Collections;
using UnityEngine;

public class PitchingManager : MonoBehaviour
{
    [SerializeField] PitchTypeDataSO[] pitchTypeDataSO;
    
    [Header("현재 난이도")]
    [SerializeField] int currentDifficulty = 1;//난이도에 따라 구속 조절. 현재는 1~3단계

    //[Header("스트라이크 확률")]
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
            SetPitching(pitchTypeDataSO[UnityEngine.Random.Range(0, pitchTypeDataSO.Length)], currentDifficulty);//우선은 전체 랜덤.
            animator.SetTrigger("WindUp");            
            yield return new WaitForSeconds(7f);
        }
    }
    //구종 설정.난이도에 따른 구속 설정.    
    void SetPitching(IPitchData iP, int currentDifficulty)
    {
        Debug.Log("투구 시작");
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
