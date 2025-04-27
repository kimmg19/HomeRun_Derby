using UnityEngine;

/// <summary>
/// ��Ʈ����ũ �� ���� Ŭ����
/// ������ ��ǥ ��ġ�� �����ϰ� �ð�ȭ�մϴ�.
/// </summary>
public class StrikeZoneManager : MonoBehaviour
{
    enum BallZone { High, Low, Left, Right }

    [Header("���� ��ġ")]
    [SerializeField] Transform catchPoint;

    [Header("�� ����")]
    [SerializeField,ReadOnly] float strikeZoneHalfHeight = 0.1f;
    [SerializeField,ReadOnly] float strikeZoneHalfWidth = 0.1f;
    [SerializeField, ReadOnly] float ballZoneHalfHeight = 0.2f;
    [SerializeField, ReadOnly] float ballZoneHalfWidth = 0.18f;

    /// <summary>
    /// ���� ��ġ�� �����մϴ�.
    /// </summary>
    /// <param name="pitchPosition">��Ʈ����ũ/�� ����</param>
    /// <returns>���� ��ǥ ��ǥ</returns>
    public Vector3 SetPitchingPoint(EPitchPosition pitchPosition)
    {
        switch (pitchPosition)
        {
            case EPitchPosition.STRIKE:
                Debug.Log("��Ʈ����ũ");
                return GetStrikePosition();

            case EPitchPosition.BALL:
                Debug.Log("��");
                return GetBallPosition((BallZone)Random.Range(0, 4));

            default:
                return GetStrikePosition();
        }
    }

    /// <summary>
    /// ��Ʈ����ũ �� ���� ���� ��ġ�� ��ȯ�մϴ�.
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
    /// �� ���� Ư�� ����(��/��/��/��)���� ���� ��ġ�� ��ȯ�մϴ�.
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
    /// ��Ʈ����ũ ���� �� ���� �ð������� ǥ���մϴ�.
    /// </summary>
    void OnDrawGizmos()
    {
        if (catchPoint == null) return;

        // ��Ʈ����ũ �� (������ ť��)
        Gizmos.color = Color.red;
        Gizmos.DrawCube(catchPoint.position,
            new Vector3(strikeZoneHalfWidth * 2, strikeZoneHalfHeight * 2, 0.01f));

        // �� �� (�ʷϻ� ���̾� ť��)
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(
            catchPoint.position,
            new Vector3(strikeZoneHalfWidth * 2 + ballZoneHalfWidth,
                       strikeZoneHalfHeight * 2 + ballZoneHalfHeight,
                       0.01f)
        );
    }
}