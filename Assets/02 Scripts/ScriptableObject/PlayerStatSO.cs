using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "HomerunDerby/Player Stats")]
public class PlayerStatSO : ScriptableObject
{
    // �⺻ ���� �� ����
    [SerializeField] float basePower = 10f;        // �⺻ �Ŀ�
    [SerializeField] float baseEyeSight = 10f;     // �⺻ ������
    [SerializeField] float baseCriticalChance = 5f; // �⺻ ũ��Ƽ�� Ȯ��

    // �ܺ� ���ٿ� ������Ƽ
    public float BasePower => basePower;
    public float BaseEyeSight => baseEyeSight;
    public float BaseCriticalChance => baseCriticalChance;
}