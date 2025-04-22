using UnityEngine;

/// <summary>
/// ���� ��� ������ ����ϴ� ���� Ŭ����
/// </summary>
public class ScoreCalculator
{
    // ���� ��� �޼���
    public int CalculateScore(EHitTiming timing, float distance, bool isHomerun,
                             int baseScore, float distanceMultiplier,
                             float longDistanceThreshold)
    {
        if (timing == EHitTiming.Miss) return 0;

        // �⺻ ���� ���
        int score = baseScore;

        // 1. �Ÿ��� ����ϴ� ����
        score += Mathf.FloorToInt(distance * distanceMultiplier);

        // 2. Ÿ�̹� ���ʽ� (Perfect Ÿ�̹��� ��� 2��)
        if (timing == EHitTiming.Perfect)
        {
            score *= 2;
            Debug.Log("Perfect Ÿ�̹�! ���� 2��!");
        }

        // 3. Ȩ�� ����
        if (isHomerun)
        {
            score += 100; // Ȩ�� �߰� ���ʽ�

            // 4. ��Ÿ� Ȩ�� ���ʽ�
            if (distance >= longDistanceThreshold)
            {
                score *= 2;
                Debug.Log($"��Ÿ� Ȩ��! ({distance}m) ���� 2��!");
            }
        }

        return score;
    }

    // Ȩ�� ���� �޼���
    public bool IsHomerun(float distance, float homerunDistance)
    {
        return distance >= homerunDistance;
    }

    // ��Ÿ� Ȩ�� ���� �޼���
    public bool IsLongHomerun(float distance, float longDistanceThreshold)
    {
        return distance >= longDistanceThreshold;
    }
}