using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchingController : MonoBehaviour
{
    [SerializeField] Ball ball;
    [SerializeField] Transform pitchingPosition; //투수의 손.
    [SerializeField] int currentDifficulty = 1;//난이도에 따라 구속 조절. 현재는 1~3단계
    [SerializeField] PitchTypeDataSO pitchTypeDataSO;
    float time = 0f;

    void Start()
    {
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > 2)
        {
            StartPitching(pitchTypeDataSO, currentDifficulty);
            time = 0f;
        }

    }
    //구종 설정.난이도에 따른 구속 설정.
    void StartPitching(PitchTypeDataSO pitchType, int currentDifficulty)
    {
        Debug.Log("투구 시작");
        
        int finalSpeed= (int)SetBallSpeed(pitchType, currentDifficulty);

        ball = Instantiate(ball, pitchingPosition.position, Quaternion.identity);        //오브젝트 풀 적용 예정
        ball.Init(pitchTypeDataSO, finalSpeed);
    }
    float SetBallSpeed(PitchTypeDataSO pitchType, int currentDifficulty)
    {
        float baseMultiplier = pitchType.speedMultiplier;
        float baseSpeed = pitchType.BaseSpeed;
        float difficultyMultiplier = 1f + (currentDifficulty - 1) * 0.05f;    //0%, 5%, 10%
        float speed = Random.Range(baseSpeed * baseMultiplier,
            baseSpeed * baseMultiplier + 10);

        return speed* difficultyMultiplier;
    }
}
