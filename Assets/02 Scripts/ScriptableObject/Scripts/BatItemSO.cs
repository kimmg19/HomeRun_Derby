using UnityEngine;

[System.Serializable]
public class BatLevelData
{
    // ��Ʈ �⺻ ����    
    public int upgradeCost;
    public int level;
    public Material batMaterial;
    public int upgradeChance;
    // ���� ���ʽ�
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