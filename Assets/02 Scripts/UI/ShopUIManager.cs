using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Interfaces;
public class ShopUIManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button redistributionBtn;
    [SerializeField] Button upgradeBtn;
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
    [SerializeField] int upgradeCost;
    IShopPlayerManager playerManager;
    public void Initialize(IShopPlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }

    void Awake()
    {
        CheckNull(redistributionBtn, nameof(redistributionBtn));
        CheckNull(upgradeBtn, nameof(upgradeBtn));
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
               
    }
    void OnEnable()
    {
        EventManager.Instance.OnPlayerStatChanged += RedistributionUI;
        EventManager.Instance.OnCurrencyChanged += EnableBtn;
        EventManager.Instance.OnCurrencyChanged += CurrencyUpdate;
        EventManager.Instance.OnBatUpgraded += UpgradeUI;
        redistributionBtn.onClick.AddListener(PressRedistribution);
        upgradeBtn.onClick.AddListener(PressUpgrade);
    }
    void OnDisable()
    {
        EventManager.Instance.OnPlayerStatChanged -= RedistributionUI;
        EventManager.Instance.OnCurrencyChanged -= EnableBtn;
        EventManager.Instance.OnCurrencyChanged -= CurrencyUpdate;
        EventManager.Instance.OnBatUpgraded -= UpgradeUI;
        redistributionBtn.onClick.RemoveListener(PressRedistribution);
        upgradeBtn.onClick.RemoveListener(PressUpgrade);
        
    }
    void Start()
    {
        redistributionCostText.text = playerManager.GetredistributionCostCost().ToString();
        upgradeCost = playerManager.GetUpgradeCost();
        upgradeCostText.text = upgradeCost.ToString();
        playerManager.Initialization();
        EnableBtn(0);        
    }

    void CheckNull(Object obj, string name)
    {
        if (obj == null)
            Debug.LogError($"{name} 누락");
    }
    
    void PressRedistribution()
    {
        playerManager.Redistribution();
    }
    void PressUpgrade()
    {
        playerManager.Upgrade();
    }
    //재분배 UI 업데이트
    public void RedistributionUI(int power, int judge, int critical)
    {
        playerPowerStatText.text = power.ToString();
        playerJudgeStatText.text = judge.ToString();
        playerCriticalStatText.text = critical.ToString();
    }
    //업그레이드 UI 업데이트
    public void UpgradeUI(BatItemSO batItemSO,int level)
    {
        BatLevelData leveldata = batItemSO.GetBatData(level);
        
        batLevelText.text="+"+leveldata.level.ToString();
        batPowerStatText.text=leveldata.powerBonus.ToString();
        batJudgeStatText.text = leveldata.judgementBonus.ToString();
        batCriticalStatText.text = leveldata.criticalBonus.ToString();
        upgradeChanceText.text = leveldata.upgradeChance.ToString()+"%";
        upgradeCostText.text = leveldata.upgradeCost.ToString();

    }
    //버튼 활성 비활성 여부.
    public void EnableBtn(int i)
    {
        if (playerManager.HasEnoughCurrency(playerManager.GetredistributionCostCost()))
        {
            redistributionBtn.interactable = true;
        }
        else
        {
            redistributionBtn.interactable = false;
        }
        if (playerManager.HasEnoughCurrency(playerManager.GetUpgradeCost()))
        {
            upgradeBtn.interactable = true;
        }
        else
        {
            upgradeBtn.interactable = false;
        }
    }
    void CurrencyUpdate(int currency)
    {
        currencyText.text = currency.ToString();
    }
    
}
