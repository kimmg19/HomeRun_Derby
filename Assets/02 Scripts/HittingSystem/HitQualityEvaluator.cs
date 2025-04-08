// 타격 품질 평가기 - 타이밍 및 품질 판정 담당
using UnityEngine;

public class HitQualityEvaluator : MonoBehaviour 
{
    // 타격 품질 계산
    public EHitTiming EvaluateHitQuality(float distance)
    {
        if (Mathf.Abs(distance) <= 0.1f) return EHitTiming.Perfect;   // 완벽한 타이밍
        else if (distance > 0.1f && distance <= 0.3f) return EHitTiming.Slow;  // 느린 타이밍
        else if (distance < -0.1f && distance >= -0.3f) return EHitTiming.Fast;  // 빠른 타이밍
        else return EHitTiming.Miss;  // 미스
        
    }
}