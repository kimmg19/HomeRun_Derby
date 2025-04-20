using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] PlayerStatSO baseStats;
    [SerializeField] BatItemSO defaultBat;

    public BatItemSO equippedBat { get;  set; }
    public int powerValue { get;  set; } //�÷��̾� �Ŀ� �ɷ�ġ
    public int criticalValue { get;  set; }
    public int currency { get;  set; } //��ȭ

    // ���� ����� ���� ���� ���
    public float CurrentPower => baseStats.BasePower + powerValue + (defaultBat?.powerBonus ?? 0);
    public float CurrentEyeSight => baseStats.BaseEyeSight + (defaultBat?.eyeSightBonus ?? 0);
    public float CurrentCritical => baseStats.BaseCriticalChance + criticalValue + (defaultBat?.criticalBonus ?? 0);

    // �ʱ�ȭ, ����/�ε� �޼���
    // ��Ʈ ����, ���� ���׷��̵� ���� �޼��� (���� �ý��۰� ����)
}