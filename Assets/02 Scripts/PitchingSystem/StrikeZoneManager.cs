using UnityEngine;

/// <summary>
/// 스트라이크 존 관리 클래스
/// 투구의 목표 위치를 설정하고 시각화합니다.
/// </summary>
public class StrikeZoneManager : MonoBehaviour
{
    enum BallZone { High, Low, Left, Right }

    [Header("포구 위치")]
    [SerializeField] Transform catchPoint;

    [Header("존 설정")]
    [SerializeField,ReadOnly] float strikeZoneHalfHeight = 0.1f;
    [SerializeField,ReadOnly] float strikeZoneHalfWidth = 0.1f;
    [SerializeField, ReadOnly] float ballZoneHalfHeight = 0.2f;
    [SerializeField, ReadOnly] float ballZoneHalfWidth = 0.18f;

    /// <summary>
    /// 투구 위치를 설정합니다.
    /// </summary>
    /// <param name="pitchPosition">스트라이크/볼 여부</param>
    /// <returns>투구 목표 좌표</returns>
    public Vector3 SetPitchingPoint(EPitchPosition pitchPosition)
    {
        switch (pitchPosition)
        {
            case EPitchPosition.STRIKE:
                Debug.Log("스트라이크");
                return GetStrikePosition();

            case EPitchPosition.BALL:
                Debug.Log("볼");
                return GetBallPosition((BallZone)Random.Range(0, 4));

            default:
                return GetStrikePosition();
        }
    }

    /// <summary>
    /// 스트라이크 존 내의 랜덤 위치를 반환합니다.
    /// </summary>
    Vector3 GetStrikePosition()
    {
        Vector3 center = catchPoint.position;
        return new Vector3(
            Random.Range(center.x - strikeZoneHalfWidth, center.x + strikeZoneHalfWidth),
            Random.Range(center.y - strikeZoneHalfHeight, center.y + strikeZoneHalfHeight),
            center.z
        );
    }

    /// <summary>
    /// 볼 존의 특정 영역(상/하/좌/우)에서 랜덤 위치를 반환합니다.
    /// </summary>
    Vector3 GetBallPosition(BallZone zone)
    {
        Vector3 center = catchPoint.position;

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

    /// <summary>
    /// 스트라이크 존과 볼 존을 시각적으로 표시합니다.
    /// </summary>
    void OnDrawGizmos()
    {
        if (catchPoint == null) return;

        // 스트라이크 존 (빨간색 큐브)
        Gizmos.color = Color.red;
        Gizmos.DrawCube(catchPoint.position,
            new Vector3(strikeZoneHalfWidth * 2, strikeZoneHalfHeight * 2, 0.01f));

        // 볼 존 (초록색 와이어 큐브)
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(
            catchPoint.position,
            new Vector3(strikeZoneHalfWidth * 2 + ballZoneHalfWidth,
                       strikeZoneHalfHeight * 2 + ballZoneHalfHeight,
                       0.01f)
        );
    }
}