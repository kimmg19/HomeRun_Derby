using UnityEngine;

// Ÿ�� ���� ����� ����ϴ� Ŭ����
public class HitStatCalculator:MonoBehaviour
{
    // �� Ÿ�� ���� Ȯ�� ��� (������ ����)
    public bool CheckBallHitSuccess(float eyeSight)
    {       
        return Random.value * 100f <= eyeSight;
    }

    // Ȯ���� ��Ÿ� ��� (�Ŀ� ���� ����)
    public float CalculateHitDistance(float baseDistance, EHitTiming timing, float power, bool isCritical)
    {
        // 1. Ÿ�̹� ����
        float timingMultiplier = GetTimingMultiplier(timing);
        float baseHitDistance = baseDistance * timingMultiplier;

        // 2. �Ÿ� ���� ����
        float minDistance = baseHitDistance * 0.8f;
        float maxDistance = baseHitDistance * 1.5f;

        // 3. �Ŀ� ���� ��� ����
        float powerRatio = power / 100f; 
        float powerBias = 1f / (1f + powerRatio * 2f); // 0.0 ~ 1.0 ���̷� ����
        float randomFactor = Mathf.Pow(Random.value, powerBias);
        float distance = Mathf.Lerp(minDistance, maxDistance, randomFactor);

        // 4. ũ��Ƽ�� ����
        if (isCritical)
        {
            distance *= 1.5f;
        }

        return distance;
    }

    // Ÿ�ֺ̹� �Ÿ� ���� ���
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

    // ũ��Ƽ�� Ÿ�� Ȯ�� ��� (ũ��Ƽ�� ���� ����)
    public bool CheckCriticalHit(float criticalChance)
    {
        return Random.value * 100f <= criticalChance;
    }
}