using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button redistributionBtn;
    [SerializeField] Button UpgradeBtn;
    [Header("Current Currency")]
    [SerializeField] TextMeshProUGUI currencyText;
    [Header("Player Stats")]
    [SerializeField] TextMeshProUGUI playerPowerStatText;
    [SerializeField] TextMeshProUGUI playerJudgeStatText;
    [SerializeField] TextMeshProUGUI playerCriticalStatText;
    [SerializeField] TextMeshProUGUI redistributionCostText;
    [Header("Bat Stats")]
    [SerializeField] TextMeshProUGUI batPowerStatText;
    [SerializeField] TextMeshProUGUI batJudgeStatText;
    [SerializeField] TextMeshProUGUI batCriticalStatText;
    [SerializeField] TextMeshProUGUI batLevelText;
    [SerializeField] TextMeshProUGUI upgradeChanceText;
    [SerializeField] TextMeshProUGUI upgradeCostText;

    [SerializeField] int redistributionCost = 10000;     //재분배 비용
    void Awake()
    {
        CheckNull(redistributionBtn, nameof(redistributionBtn));
        CheckNull(UpgradeBtn, nameof(UpgradeBtn));
        CheckNull(currencyText, nameof(currencyText));
        CheckNull(playerPowerStatText, nameof(playerPowerStatText));
        CheckNull(playerJudgeStatText, nameof(playerJudgeStatText));
        CheckNull(playerCriticalStatText, nameof(playerCriticalStatText));
        CheckNull(redistributionCostText, nameof(redistributionCostText));
        CheckNull(batPowerStatText, nameof(batPowerStatText));
        CheckNull(batJudgeStatText, nameof(batJudgeStatText));
        CheckNull(batCriticalStatText, nameof(batCriticalStatText));
        CheckNull(batLevelText, nameof(batLevelText));
        CheckNull(upgradeChanceText, nameof(upgradeChanceText));
        CheckNull(upgradeCostText, nameof(upgradeCostText));
        redistributionBtn.onClick.AddListener(()=>PlayerManager.Instance.Redistribution());
    }
    void OnEnable()
    {
        EventManager.Instance.OnPlayerStatChanged += Redistribution;
    }
    void OnDisable()
    {
        EventManager.Instance.OnPlayerStatChanged -= Redistribution;
    }
    void Start()
    {
        redistributionCostText.text = redistributionCost.ToString();
    }

    void CheckNull(Object obj, string name)
    {
        if (obj == null)
            Debug.LogError($"{name} 누락");
    }

    //배트 업그레이드
    public void Upgrade()
    {

    }
    //재분배
    public void Redistribution(int power, int judge, int critical)
    {
        playerPowerStatText.text = power.ToString();
        playerJudgeStatText.text = judge.ToString();
        playerCriticalStatText.text = critical.ToString();
    }
    //버튼 활성 비활성 여부.
    public void EnableBtn()
    {

    }
}
