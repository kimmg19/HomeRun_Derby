using UnityEngine;
using UnityEngine.Events;

// 타격 궤적 계산기 - 베지어 곡선 및 타격 방향 계산 담당
public class HitTrajectoryCalculator :MonoBehaviour
{
    // 베지어 곡선 제어점 계산
    public void CalculateTrajectory(Vector3 startPoint, float angle, float distance, float height,
                                   out Vector3 offset1, out Vector3 offset2, out Vector3 endPoint)
    {
        // 각도를 라디안으로 변환
        float angleRad = angle * Mathf.Deg2Rad;

        // 방향 벡터 계산
        Vector3 direction = new Vector3(
            Mathf.Sin(angleRad),  // X (좌우)
            0,                    // Y (높이는 별도 계산)
            Mathf.Cos(angleRad)   // Z (앞뒤)
        );

        // 제어점 계산
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

        // 착지점 계산
        endPoint = startPoint + direction * distance;
    }

    // 타이밍에 따른 방향 계산
    public float CalculateHitAngle(EHitTiming timing)
    {
        switch (timing)
        {
            case EHitTiming.Perfect:
                return Random.Range(-10f, 10f);     // 중앙 방향
            case EHitTiming.Fast:
                return Random.Range(-45f, -10f);    // 왼쪽 방향
            case EHitTiming.Slow:
                return Random.Range(10f, 45f);      // 오른쪽 방향
            default:
                return -45f;
        }
    }
}