// Ÿ�� ǰ�� �򰡱� - Ÿ�̹� �� ǰ�� ���� ���
using UnityEngine;

public class HitQualityEvaluator : MonoBehaviour 
{
    // Ÿ�� ǰ�� ���
    public EHitTiming EvaluateHitQuality(float distance)
    {
        if (Mathf.Abs(distance) <= 0.1f) return EHitTiming.Perfect;   // �Ϻ��� Ÿ�̹�
        else if (distance > 0.1f && distance <= 0.3f) return EHitTiming.Slow;  // ���� Ÿ�̹�
        else if (distance < -0.1f && distance >= -0.3f) return EHitTiming.Fast;  // ���� Ÿ�̹�
        else return EHitTiming.Miss;  // �̽�
        
    }
}