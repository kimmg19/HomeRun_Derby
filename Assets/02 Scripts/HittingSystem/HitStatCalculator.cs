using UnityEngine;

// 타격 관련 계산을 담당하는 클래스
public class HitStatCalculator:MonoBehaviour
{
    // 볼 타격 성공 확률 계산 (선구안 적용)
    public bool CheckBallHitSuccess(float eyeSight)
    {
        // 선구안 스탯이 높을수록 볼 타격 성공률 증가
        float successRate = eyeSight / 100f;
        Debug.Log("볼 타격 성공률: "+successRate);
        // 최대 50% 제한
        successRate = Mathf.Clamp(successRate, 0f, 0.5f);

        return Random.value <= successRate;
    }

    // 확률적 비거리 계산 (파워 스탯 적용)
    public float CalculateHitDistance(float baseDistance, EHitTiming timing, float power, bool isCritical)
    {
        // 기본 거리
        float baseHitDistance = baseDistance;

        // 타이밍 보너스
        float timingMultiplier = GetTimingMultiplier(timing);
        baseHitDistance *= timingMultiplier;

        // 파워 기반 거리 범위 설정
        float minDistance = baseHitDistance * 0.8f;
        float maxDistance = baseHitDistance * 1.2f;

        // 파워 스탯 비율 (0~1)
        float powerRatio = Mathf.Clamp01(power / 100f);

        // 파워가 높을수록 더 높은 거리가 나올 확률 증가 (편향된 랜덤)
        float powerBias = 1f - (powerRatio * 0.8f);
        float randomFactor = Mathf.Pow(Random.value, powerBias);
        float distance = Mathf.Lerp(minDistance, maxDistance, randomFactor);

        // 크리티컬 보너스
        if (isCritical)
        {
            distance *= 1.5f;
        }

        return distance;
    }

    // 타이밍별 거리 배율 계산
    public float GetTimingMultiplier(EHitTiming timing)
    {
        switch (timing)
        {
            case EHitTiming.Perfect: return 1.3f;  // 30% 보너스
            case EHitTiming.Fast: return 0.7f;
            case EHitTiming.Slow: return 0.7f;     // 30% 감소
            default: return 0.5f;
        }
    }

    // 크리티컬 타격 확률 계산 (크리티컬 스탯 적용)
    public bool CheckCriticalHit(float criticalChance)
    {
        return Random.value * 100f < criticalChance;
    }
}