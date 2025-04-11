using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "HomerunDerby/Player Stats")]
public class PlayerStatSO : ScriptableObject
{
    // 기본 스탯 값 설정
    [SerializeField] float basePower = 10f;        // 기본 파워
    [SerializeField] float baseEyeSight = 10f;     // 기본 선구안
    [SerializeField] float baseCriticalChance = 5f; // 기본 크리티컬 확률

    // 외부 접근용 프로퍼티
    public float BasePower => basePower;
    public float BaseEyeSight => baseEyeSight;
    public float BaseCriticalChance => baseCriticalChance;
}