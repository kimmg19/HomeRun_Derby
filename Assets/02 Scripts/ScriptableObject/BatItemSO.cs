using UnityEngine;

[CreateAssetMenu(fileName = "NewBat", menuName = "HomerunDerby/Bat Item")]
public class BatItemSO : ScriptableObject
{
    // ��Ʈ �⺻ ����
    public string batName;
    public Material batMaterial;
    public int cost;

    // ���� ���ʽ�
    public float powerBonus;
    public float eyeSightBonus;
    public float criticalBonus;
}