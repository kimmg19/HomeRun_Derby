using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchingController : MonoBehaviour
{
    [SerializeField] Ball ball;
    [SerializeField] Transform pitchingPoint; //투수의 손.
    [SerializeField] Transform endPoint;
    [SerializeField] PitchTypeDataSO pitchTypeDataSO;

    [Header("현재 난이도")]
    [SerializeField] int currentDifficulty = 1;//난이도에 따라 구속 조절. 현재는 1~3단계
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartPitching(pitchTypeDataSO, currentDifficulty);
        }

    }

    //구종 설정.난이도에 따른 구속 설정.    
    void StartPitching(PitchTypeDataSO pitchType, int currentDifficulty)
    {
        Debug.Log("투구 시작");

        //ui용 구속-소수점 첫째 자리까지 표현 위해
        float finalSpeed= SetBallSpeed(pitchType, currentDifficulty);
        ball = Instantiate(ball, pitchingPoint.position, Quaternion.identity);        //오브젝트 풀 적용 예정
        ball.Init(pitchTypeDataSO, (int)finalSpeed, pitchingPoint.position, endPoint.position);
    }
    float SetBallSpeed(PitchTypeDataSO pitchType, int currentDifficulty)
    {
        float baseMultiplier = pitchType.speedMultiplier;
        float baseSpeed = pitchType.BaseSpeed;
        float difficultyMultiplier = 1f + (currentDifficulty - 1) * 0.05f;    //0%, 5%, 10%

        float speed = Random.Range(baseSpeed * baseMultiplier,
            baseSpeed * baseMultiplier + 10);

        return speed *difficultyMultiplier;
    }
}
