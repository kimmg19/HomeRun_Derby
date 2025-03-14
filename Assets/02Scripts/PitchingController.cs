using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchingController : MonoBehaviour
{
    [SerializeField] Ball ball;
    [SerializeField] Transform pitchingPosition; //������ ��.
    [SerializeField] int currentDifficulty = 1;//���̵��� ���� ���� ����. ����� 1~3�ܰ�
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
    //���� ����.���̵��� ���� ���� ����.
    void StartPitching(PitchTypeDataSO pitchType, int currentDifficulty)
    {
        Debug.Log("���� ����");
        
        int finalSpeed= (int)SetBallSpeed(pitchType, currentDifficulty);

        ball = Instantiate(ball, pitchingPosition.position, Quaternion.identity);        //������Ʈ Ǯ ���� ����
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
