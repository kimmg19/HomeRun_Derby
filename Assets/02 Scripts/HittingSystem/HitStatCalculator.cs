using UnityEngine;

// 타격 관련 계산을 담당하는 클래스
public class HitStatCalculator:MonoBehaviour
{
    // 볼 타격 성공 확률 계산 (선구안 적용)
    public bool CheckBallHitSuccess(float eyeSight)
    {       
        return Random.value * 100f <= eyeSight;
    }

    // 확률적 비거리 계산 (파워 스탯 적용)
    public float CalculateHitDistance(float baseDistance, EHitTiming timing, float power, bool isCritical)
    {
        // 1. 타이밍 보정
        float timingMultiplier = GetTimingMultiplier(timing);
        float baseHitDistance = baseDistance * timingMultiplier;

        // 2. 거리 범위 설정
        float minDistance = baseHitDistance * 0.8f;
        float maxDistance = baseHitDistance * 1.5f;

        // 3. 파워 비율 기반 편향
        float powerRatio = power / 100f; 
        float powerBias = 1f / (1f + powerRatio * 2f); // 0.0 ~ 1.0 사이로 감소
        float randomFactor = Mathf.Pow(Random.value, powerBias);
        float distance = Mathf.Lerp(minDistance, maxDistance, randomFactor);

        // 4. 크리티컬 보정
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
            case EHitTiming.Perfect: return 1.4f;  
            case EHitTiming.Fast: return 0.85f;
            case EHitTiming.Slow: return 0.85f;     
            default: return 1f;
        }
    }

    // 크리티컬 타격 확률 계산 (크리티컬 스탯 적용)
    public bool CheckCriticalHit(float criticalChance)
    {
        return Random.value * 100f <= criticalChance;
    }
}