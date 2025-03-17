using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PitchingController : MonoBehaviour
{
    
    [SerializeField] Ball ball;
    [Header("투구 속성")]
    [SerializeField] Transform pitchingPoint; //투수의 손.
    [SerializeField] Transform end_Point;
    [SerializeField] PitchTypeDataSO pitchTypeDataSO;
    [Header("스트라이크 존")]
    [SerializeField] float zoneHeight;
    [SerializeField] float zonewidth;

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
    void StartPitching(IPitchData pitchData, int currentDifficulty)
    {
        Debug.Log("투구 시작");

        //ui용 구속-소수점 첫째 자리까지 표현 위해
        float finalSpeed = SetBallSpeed(pitchData, currentDifficulty);
        ball = Instantiate(ball, pitchingPoint.position, Quaternion.identity);        //오브젝트 풀 적용 예정
        Vector3 endPoint = SetPitchingPoint();
        ball.Init(pitchTypeDataSO, (int)finalSpeed, pitchingPoint.position, endPoint);
    }
    //구속 설정
    float SetBallSpeed(IPitchData pitchData, int currentDifficulty)
    {
        float baseMultiplier = pitchData.SpeedMultiplier;
        float baseSpeed = pitchData.BaseSpeed;
        float difficultyMultiplier = 1f + (currentDifficulty - 1) * 0.05f;    //0%, 5%, 10%
        float speed = Random.Range(baseSpeed * baseMultiplier,
            baseSpeed * baseMultiplier + 10);

        return speed * difficultyMultiplier;
    }
    //투구 도착 위치
    Vector3 SetPitchingPoint()
    {
        float pointX = Random.Range(end_Point.position.x - zonewidth, end_Point.position.x + zonewidth);
        float pointY = Random.Range(end_Point.position.y - zoneHeight, end_Point.position.y + zoneHeight);
        Vector3 finalPoint = new Vector3(pointX, pointY, end_Point.position.z);

        return finalPoint;
    }
    void OnDrawGizmos()
    {
        Vector3 p = new Vector3(zoneHeight * 2, zonewidth * 2, 0.01f);
        Gizmos.DrawCube(end_Point.position, p);
        Gizmos.color = Color.red;
    }
}
