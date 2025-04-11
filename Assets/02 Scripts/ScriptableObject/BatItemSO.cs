using UnityEngine;

[CreateAssetMenu(fileName = "NewBat", menuName = "HomerunDerby/Bat Item")]
public class BatItemSO : ScriptableObject
{
    // 배트 기본 정보
    public string batName;
    public Material batMaterial;
    public int cost;

    // 스탯 보너스
    public float powerBonus;
    public float eyeSightBonus;
    public float criticalBonus;
}