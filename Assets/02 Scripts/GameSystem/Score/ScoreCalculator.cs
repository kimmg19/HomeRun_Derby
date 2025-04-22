using UnityEngine;

/// <summary>
/// 점수 계산 로직만 담당하는 순수 클래스
/// </summary>
public class ScoreCalculator
{
    // 점수 계산 메서드
    public int CalculateScore(EHitTiming timing, float distance, bool isHomerun,
                             int baseScore, float distanceMultiplier,
                             float longDistanceThreshold)
    {
        if (timing == EHitTiming.Miss) return 0;

        // 기본 점수 계산
        int score = baseScore;

        // 1. 거리에 비례하는 점수
        score += Mathf.FloorToInt(distance * distanceMultiplier);

        // 2. 타이밍 보너스 (Perfect 타이밍일 경우 2배)
        if (timing == EHitTiming.Perfect)
        {
            score *= 2;
            Debug.Log("Perfect 타이밍! 점수 2배!");
        }

        // 3. 홈런 여부
        if (isHomerun)
        {
            score += 100; // 홈런 추가 보너스

            // 4. 장거리 홈런 보너스
            if (distance >= longDistanceThreshold)
            {
                score *= 2;
                Debug.Log($"장거리 홈런! ({distance}m) 점수 2배!");
            }
        }

        return score;
    }

    // 홈런 판정 메서드
    public bool IsHomerun(float distance, float homerunDistance)
    {
        return distance >= homerunDistance;
    }

    // 장거리 홈런 판정 메서드
    public bool IsLongHomerun(float distance, float longDistanceThreshold)
    {
        return distance >= longDistanceThreshold;
    }
}