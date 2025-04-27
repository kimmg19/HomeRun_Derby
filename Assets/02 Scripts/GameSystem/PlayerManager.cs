using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] PlayerStatSO baseStats;
    [SerializeField] BatItemSO defaultBat;

    public BatItemSO equippedBat { get;  set; }
    public int powerValue { get;  set; } //플레이어 파워 능력치
    public int criticalValue { get;  set; }
    public int currency { get;  set; } //재화

    // 현재 적용된 최종 스탯 계산
    public float CurrentPower => baseStats.BasePower + powerValue + (defaultBat?.powerBonus ?? 0);
    public float CurrentEyeSight => baseStats.BaseEyeSight + (defaultBat?.eyeSightBonus ?? 0);
    public float CurrentCritical => baseStats.BaseCriticalChance + criticalValue + (defaultBat?.criticalBonus ?? 0);

    // 초기화, 저장/로드 메서드
    // 배트 장착, 스탯 업그레이드 관련 메서드 (상점 시스템과 연동)
}