using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static PitchingManager;

//투구 위치.
//현재는 그냥 볼 위치에 공을 던지는데 구종별로 던지는 위치 다르게 해도 좋을 듯. 확률적으로 던질 위치를 정해주는 거지.
public class StrikeZoneManager : MonoBehaviour
{
    [Header("포구 위치")]
    public Transform end_Point;
    //[Header("존 넓이")]
    const float strikeZoneHalfHeight = 0.2f;
    const float strikeZoneHalfWidth = 0.18f;
    const float ballZoneHalfHeight = 0.15f;
    const float ballZoneHalfWidth = 0.15f;

    enum BallZone
    {
        High,
        Low,
        Left,
        Right
    }
    public Vector3 SetPitchingPoint(EPitchPosition pitchPosition)
    {
        switch (pitchPosition)
        {
            case EPitchPosition.Strike:
                {
                    Debug.Log("스트라이크");
                    return GetStrikePosition();
                }
            case EPitchPosition.Ball:
                {
                    Debug.Log("볼");
                    return GetBallPosition((BallZone)Random.Range(0, 4));
                }
            default: return GetStrikePosition();
        }
    }
    Vector3 GetStrikePosition()
    {
        
        Vector3 center = end_Point.position;

        return new Vector3(
            Random.Range(center.x - strikeZoneHalfWidth, center.x + strikeZoneHalfWidth),
            Random.Range(center.y - strikeZoneHalfHeight, center.y + strikeZoneHalfHeight),
            center.z
        );
    }
    Vector3 GetBallPosition(BallZone zone)
    {
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
        Gizmos.DrawCube(end_Point.position ,new Vector3(strikeZoneHalfWidth * 2, strikeZoneHalfHeight * 2, 0.01f));

        Gizmos.color = Color.green;         
        Gizmos.DrawWireCube(
            end_Point.position,
            new Vector3(strikeZoneHalfWidth * 2 + ballZoneHalfWidth, strikeZoneHalfHeight * 2 + ballZoneHalfHeight, 0.01f)
        );
    }

}
