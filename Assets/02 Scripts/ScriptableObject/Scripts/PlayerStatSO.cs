using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "HomerunDerby/Player Stats")]
public class PlayerStatSO : ScriptableObject
{
    // �⺻ ���� �� ����
    [SerializeField] float basePower = 5f;        // �⺻ �Ŀ�
    [SerializeField] float baseJudgement = 5f;     // �⺻ ������
    [SerializeField] float baseCriticalChance = 5f; // �⺻ ũ��Ƽ�� Ȯ��

    // �ܺ� ���ٿ� ������Ƽ
    public float BasePower => basePower;
    public float BaseJudgement => baseJudgement;
    public float BaseCriticalChance => baseCriticalChance;
}