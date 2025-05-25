using UnityEngine;
using UnityEngine.Events;

// Ÿ�� ���� ���� - ������ � �� Ÿ�� ���� ��� ���
public class HitTrajectoryCalculator :MonoBehaviour
{
    // ������ � ������ ���
    public void CalculateTrajectory(Vector3 startPoint, float angle, float distance, float height,
                                   out Vector3 offset1, out Vector3 offset2, out Vector3 endPoint)
    {
        // ������ �������� ��ȯ
        float angleRad = angle * Mathf.Deg2Rad;

        // ���� ���� ���
        Vector3 direction = new Vector3(
            Mathf.Sin(angleRad),  // X (�¿�)
            0,                    // Y (���̴� ���� ���)
            Mathf.Cos(angleRad)   // Z (�յ�)
        );

        // ������ ���
        offset1 = startPoint + new Vector3(
            direction.x * distance * 0.3f,
            height * 0.5f,
            direction.z * distance * 0.3f
        );

        offset2 = startPoint + new Vector3(
            direction.x * distance * 0.7f,
            height * 0.5f,
            direction.z * distance * 0.7f
        );

        // ������ ���
        endPoint = startPoint + direction * distance;
    }

    // Ÿ�ֿ̹� ���� ���� ���
    public float CalculateHitAngle(EHitTiming timing)
    {
        switch (timing)
        {
            case EHitTiming.Perfect:
                return Random.Range(-10f, 10f);     // �߾� ����
            case EHitTiming.Fast:
                return Random.Range(-45f, -10f);    // ���� ����
            case EHitTiming.Slow:
                return Random.Range(10f, 45f);      // ������ ����
            default:
                return -45f;
        }
    }
}