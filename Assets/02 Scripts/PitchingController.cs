using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PitchingController : MonoBehaviour
{
    
    [SerializeField] Ball ball;
    [Header("���� �Ӽ�")]
    [SerializeField] Transform pitchingPoint; //������ ��.
    [SerializeField] Transform end_Point;
    [SerializeField] PitchTypeDataSO pitchTypeDataSO;
    [Header("��Ʈ����ũ ��")]
    [SerializeField] float zoneHeight;
    [SerializeField] float zonewidth;

    [Header("���� ���̵�")]
    [SerializeField] int currentDifficulty = 1;//���̵��� ���� ���� ����. ����� 1~3�ܰ�


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartPitching(pitchTypeDataSO, currentDifficulty);
        }
    }

    //���� ����.���̵��� ���� ���� ����.    
    void StartPitching(IPitchData pitchData, int currentDifficulty)
    {
        Debug.Log("���� ����");

        //ui�� ����-�Ҽ��� ù° �ڸ����� ǥ�� ����
        float finalSpeed = SetBallSpeed(pitchData, currentDifficulty);
        ball = Instantiate(ball, pitchingPoint.position, Quaternion.identity);        //������Ʈ Ǯ ���� ����
        Vector3 endPoint = SetPitchingPoint();
        ball.Init(pitchTypeDataSO, (int)finalSpeed, pitchingPoint.position, endPoint);
    }
    //���� ����
    float SetBallSpeed(IPitchData pitchData, int currentDifficulty)
    {
        float baseMultiplier = pitchData.SpeedMultiplier;
        float baseSpeed = pitchData.BaseSpeed;
        float difficultyMultiplier = 1f + (currentDifficulty - 1) * 0.05f;    //0%, 5%, 10%
        float speed = Random.Range(baseSpeed * baseMultiplier,
            baseSpeed * baseMultiplier + 10);

        return speed * difficultyMultiplier;
    }
    //���� ���� ��ġ
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
