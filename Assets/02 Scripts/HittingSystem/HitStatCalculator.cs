using UnityEngine;

// Ÿ�� ���� ����� ����ϴ� Ŭ����
public class HitStatCalculator:MonoBehaviour
{
    // �� Ÿ�� ���� Ȯ�� ��� (������ ����)
    public bool CheckBallHitSuccess(float eyeSight)
    {
        // ������ ������ �������� �� Ÿ�� ������ ����
        float successRate = eyeSight / 100f;
        Debug.Log("�� Ÿ�� ������: "+successRate);
        // �ִ� 50% ����
        successRate = Mathf.Clamp(successRate, 0f, 0.5f);

        return Random.value <= successRate;
    }

    // Ȯ���� ��Ÿ� ��� (�Ŀ� ���� ����)
    public float CalculateHitDistance(float baseDistance, EHitTiming timing, float power, bool isCritical)
    {
        // �⺻ �Ÿ�
        float baseHitDistance = baseDistance;

        // Ÿ�̹� ���ʽ�
        float timingMultiplier = GetTimingMultiplier(timing);
        baseHitDistance *= timingMultiplier;

        // �Ŀ� ��� �Ÿ� ���� ����
        float minDistance = baseHitDistance * 0.8f;
        float maxDistance = baseHitDistance * 1.2f;

        // �Ŀ� ���� ���� (0~1)
        float powerRatio = Mathf.Clamp01(power / 100f);

        // �Ŀ��� �������� �� ���� �Ÿ��� ���� Ȯ�� ���� (����� ����)
        float powerBias = 1f - (powerRatio * 0.8f);
        float randomFactor = Mathf.Pow(Random.value, powerBias);
        float distance = Mathf.Lerp(minDistance, maxDistance, randomFactor);

        // ũ��Ƽ�� ���ʽ�
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
            case EHitTiming.Perfect: return 1.3f;  // 30% ���ʽ�
            case EHitTiming.Fast: return 0.7f;
            case EHitTiming.Slow: return 0.7f;     // 30% ����
            default: return 0.5f;
        }
    }

    // ũ��Ƽ�� Ÿ�� Ȯ�� ��� (ũ��Ƽ�� ���� ����)
    public bool CheckCriticalHit(float criticalChance)
    {
        return Random.value * 100f < criticalChance;
    }
}