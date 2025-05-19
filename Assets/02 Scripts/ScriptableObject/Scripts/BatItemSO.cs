using UnityEngine;

[System.Serializable]
public class BatLevelData
{
    // 배트 기본 정보    
    public int upgradeCost;
    public int level;
    public Material batMaterial;
    public int upgradeChance;
    // 스탯 보너스
    public int powerBonus;
    public int judgementBonus;
    public int criticalBonus;
    
}
[CreateAssetMenu(fileName = "NewBat", menuName = "HomerunDerby/Bat Item")]
public class BatItemSO : ScriptableObject
{
    public int maxLevel;
    [SerializeField]BatLevelData[] batLevelDatas;
    public BatLevelData GetBatData(int index)
    {
        return batLevelDatas[index];
    }

}