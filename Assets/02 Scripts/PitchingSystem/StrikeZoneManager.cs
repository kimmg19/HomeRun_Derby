using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static PitchingManager;

//���� ��ġ.
//����� �׳� �� ��ġ�� ���� �����µ� �������� ������ ��ġ �ٸ��� �ص� ���� ��. Ȯ�������� ���� ��ġ�� �����ִ� ����.
public class StrikeZoneManager : MonoBehaviour
{
    [Header("���� ��ġ")]
    [SerializeField] Transform end_Point;
    //[Header("�� ����")]
    const float strikeZoneHalfHeight = 0.07f;
    const float strikeZoneHalfWidth = 0.065f;
    const float ballZoneHalfHeight = 0.09f;
    const float ballZoneHalfWidth = 0.075f;

    enum BallZone
    {
        High,
        Low,
        Left,
        Right
    }
    public Vector3 SetPitchingPoint(PitchPosition pitchPosition)
    {
        switch (pitchPosition)
        {
            case PitchPosition.Strike:
                {
                    return GetStrikePosition();
                }
            case PitchPosition.Ball:
                {
                    Debug.Log("��");

                    return GetBallPosition((BallZone)Random.Range(0, 4));
                }
            default: return GetStrikePosition();
        }
    }
    Vector3 GetStrikePosition()
    {
        // ���� ��ġ�� ĳ���Ͽ� �ڵ� �ܼ�ȭ
        Vector3 center = end_Point.position;

        return new Vector3(
            Random.Range(center.x - strikeZoneHalfWidth, center.x + strikeZoneHalfWidth),
            Random.Range(center.y - strikeZoneHalfHeight, center.y + strikeZoneHalfHeight),
            center.z
        );
    }
    Vector3 GetBallPosition(BallZone zone)
    {
        // ���� ��ġ�� ĳ���Ͽ� �ڵ� �ܼ�ȭ
        Vector3 center = end_Point.position;

        switch (zone)
        {
            case BallZone.High:
                return new Vector3(
                    Random.Range(center.x - strikeZoneHalfWidth - ballZoneHalfWidth / 2, center.x + strikeZoneHalfWidth + ballZoneHalfWidth / 2),
                    Random.Range(center.y + strikeZoneHalfHeight, center.y + strikeZoneHalfHeight + ballZoneHalfHeight / 2),
                    center.z
                );

            case BallZone.Low:
                return new Vector3(
                    Random.Range(center.x - strikeZoneHalfWidth - ballZoneHalfWidth / 2, center.x + strikeZoneHalfWidth + ballZoneHalfWidth / 2),
                    Random.Range(center.y - strikeZoneHalfHeight, center.y - strikeZoneHalfHeight - ballZoneHalfHeight / 2),
                    center.z
                );

            case BallZone.Left:
                return new Vector3(
                    Random.Range(center.x - strikeZoneHalfWidth - ballZoneHalfWidth / 2, center.x - strikeZoneHalfWidth),
                    Random.Range(center.y - strikeZoneHalfHeight - ballZoneHalfHeight / 2, center.y + strikeZoneHalfHeight + ballZoneHalfHeight / 2),
                    center.z
                );

            case BallZone.Right:
                return new Vector3(
                    Random.Range(center.x + strikeZoneHalfWidth, center.x + strikeZoneHalfWidth + ballZoneHalfWidth / 2),
                    Random.Range(center.y - strikeZoneHalfHeight - ballZoneHalfHeight / 2, center.y + strikeZoneHalfHeight + ballZoneHalfHeight / 2),
                    center.z
                );

            default:                
                return GetStrikePosition();
        }
    }
    void OnDrawGizmos()
    {
        if (end_Point == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawCube(end_Point.position, new Vector3(strikeZoneHalfWidth * 2, strikeZoneHalfHeight * 2, 0.01f));

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(
            end_Point.position,
            new Vector3(strikeZoneHalfWidth * 2 + ballZoneHalfWidth, strikeZoneHalfHeight * 2 + ballZoneHalfHeight, 0.01f)
        );
    }

}
