using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchingController : MonoBehaviour
{
    [SerializeField] Ball ball;
    [SerializeField] Transform pitchingPoint; //������ ��.
    [SerializeField] Transform endPoint;
    [SerializeField] PitchTypeDataSO pitchTypeDataSO;

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
    void StartPitching(PitchTypeDataSO pitchType, int currentDifficulty)
    {
        Debug.Log("���� ����");

        //ui�� ����-�Ҽ��� ù° �ڸ����� ǥ�� ����
        float finalSpeed= SetBallSpeed(pitchType, currentDifficulty);
        ball = Instantiate(ball, pitchingPoint.position, Quaternion.identity);        //������Ʈ Ǯ ���� ����
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
