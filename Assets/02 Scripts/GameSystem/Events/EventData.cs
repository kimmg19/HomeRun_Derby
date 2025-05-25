using UnityEngine;

[System.Serializable]
public struct HitResultData
{
    public bool isHomerun;
    public float distance;
    public EHitTiming timing;
    public int score;
    public bool isCritical;
    public bool isBigHomerun;

    public HitResultData(bool homerun, float dist, EHitTiming hitTiming,
                        int hitScore, bool critical, bool bigHomerun)
    {
        isHomerun = homerun;
        distance = dist;
        timing = hitTiming;
        score = hitScore;
        isCritical = critical;
        isBigHomerun = bigHomerun;
    }
}
[System.Serializable]
public struct BallData
{
    public float speed;
    public EPitchLocation position;
    public EPitchType pitchType;

    public BallData(float ballSpeed, EPitchLocation pitchPosition, EPitchType type)
    {
        speed = ballSpeed;
        position = pitchPosition;
        pitchType = type;
    }
}
[System.Serializable]
public struct BallHitData
{
    public EHitTiming timing;
    public float distance;
    public bool isCritical;

    public BallHitData(EHitTiming hitTiming, float hitDistance, bool critical)
    {
        timing = hitTiming;
        distance = hitDistance;
        isCritical = critical;
    }
}