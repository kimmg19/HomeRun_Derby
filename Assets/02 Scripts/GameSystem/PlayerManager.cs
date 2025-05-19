using UnityEngine;

[System.Serializable]
public struct PlayerStats
{
    public int powerValue;
    public int judgeValue;
    public int criticalValue;
}

[DefaultExecutionOrder(-10000)]
public class PlayerManager : MonoBehaviour
{
    static PlayerManager instance;
    public static PlayerManager Instance
    {
        get
        {
            if (instance == null && Time.timeScale != 0)
            {
                var obj = FindAnyObjectByType<PlayerManager>();
                if (obj != null)
                {
                    instance = obj;
                    print("플매 찾고 지정");
                }
                else
                {
                    print("플매 생성");
                    instance = Create();
                }
            }
            return instance;
        }
    }
    static PlayerManager Create()
    {
        Debug.LogWarning("PlayerManager 생성");
        return Instantiate(Resources.Load<PlayerManager>("PlayerManager"));
    }

    [Header("플레이어 정보")]
    [SerializeField] PlayerStatSO baseStats;
    [SerializeField] BatItemSO currentBat;
    [SerializeField] int currentBatLevel;

    [Header("플레이어 스탯")]
    [SerializeField] int maxValue = 30;
    [SerializeField] PlayerStats playerstat;

    //재화 정보
    [SerializeField] int currency;//재화
    [SerializeField] int redistributionCost = 10000;     //재분배 비용
    [SerializeField] int upgradeCost;

    BatLevelData levelData;


    public int PowerValue { get { return playerstat.powerValue; } set { playerstat.powerValue = value; } }
    public int JudgeValue { get { return playerstat.judgeValue; } set { playerstat.judgeValue = value; } }
    public int CriticalValue { get { return playerstat.criticalValue; } set { playerstat.criticalValue = value; } }

    public int Currency => currency;

    public float CurrentPower => baseStats.BasePower + PowerValue + levelData.powerBonus;
    public float CurrentJudgeSight => baseStats.BaseJudgement + JudgeValue + levelData.judgementBonus;
    public float CurrentCritical => baseStats.BaseCriticalChance + CriticalValue + levelData.criticalBonus;

    void Awake()
    {
        if (instance != null && instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        
            instance = this;
            DontDestroyOnLoad(gameObject);
        
        LoadData();
        SetBatLevel();
    }
    
    void SetBatLevel()
    {
        levelData = currentBat.GetBatData(currentBatLevel);
    }
    public void Reset()
    {
        PlayerPrefs.SetInt("Currency", 100000);
        PlayerPrefs.SetInt("PowerValue", 0);
        PlayerPrefs.SetInt("JudgeValue", 0);
        PlayerPrefs.SetInt("CriticalValue", 0);
        PlayerPrefs.SetInt("currentBatLevel", 0);
        PlayerPrefs.Save();
        LoadData();
        SetBatLevel();
        Initialization();

    }
    void LoadData()
    {
        currency = PlayerPrefs.GetInt("Currency", 100000);
        PowerValue = PlayerPrefs.GetInt("PowerValue", 10);
        JudgeValue = PlayerPrefs.GetInt("JudgeValue", 10);
        CriticalValue = PlayerPrefs.GetInt("CriticalValue", 10);
        currentBatLevel = PlayerPrefs.GetInt("currentBatLevel", 0);
    }
    void SaveData()
    {
        print("데이터 저장");

        PlayerPrefs.SetInt("Currency", currency);
        PlayerPrefs.SetInt("PowerValue", PowerValue);
        PlayerPrefs.SetInt("JudgeValue", JudgeValue);
        PlayerPrefs.SetInt("CriticalValue", CriticalValue);
        PlayerPrefs.SetInt("currentBatLevel", currentBatLevel);
        PlayerPrefs.Save();

    }    

    public void Initialization()
    {
        EventManager.Instance.PublishCurrencyChanged(currency);
        EventManager.Instance.PublishPlayerStatsChanged(PowerValue, JudgeValue, CriticalValue);
        EventManager.Instance.PublishBatUpgraded(currentBat, currentBatLevel);
    }

    
    public void SpendCurrency(int cost)
    {
        if (cost > currency) { print("잔약 부족"); return; }
        currency -= cost;
        EventManager.Instance.PublishCurrencyChanged(currency);
    }
    public bool HasEnoughCurrency(int cost)
    {
        return currency >= cost;
    }
    public void AddCurrency(int cost)
    {
        if (cost <= 0) { print("추가할 돈이 없는데용"); return; }
        currency += cost;
        EventManager.Instance.PublishCurrencyChanged(currency);
    }
    //재분배
    public void Redistribution()
    {
        //if (!HasEnoughCurrency(redistributionCost)) return;
        SpendCurrency(redistributionCost);
        int x, y;
        x = Random.Range(0, maxValue + 1);
        y = Random.Range(0, maxValue + 1);

        if (x > y)
        {
            (x, y) = (y, x);
        }
        int a = x;
        int b = y - x;
        int c = maxValue - y;
        int[] ranArr = { a, b, c };
        for (int i = 0; i < ranArr.Length; i++)
        {
            int randIndex = Random.Range(i, ranArr.Length);
            (ranArr[i], ranArr[randIndex]) = (ranArr[randIndex], ranArr[i]);
        }
        PowerValue = ranArr[0];
        JudgeValue = ranArr[1];
        CriticalValue = ranArr[2];
        EventManager.Instance.PublishPlayerStatsChanged(PowerValue, JudgeValue, CriticalValue);
    }
    public void Upgrade()
    {
        //if (!HasEnoughCurrency(levelData.upgradeCost)) return;
        SpendCurrency(levelData.upgradeCost);

        if ( currentBatLevel < currentBat.maxLevel)
        {
            print(levelData.upgradeChance);
            if (Random.value * 100 <= levelData.upgradeChance)
            {
                currentBatLevel++;
                SetBatLevel();
                EventManager.Instance.PublishBatUpgraded(currentBat, currentBatLevel);
            }
        }
        else return;
    }
    public int GetUpgradeCost()
    {
        return levelData.upgradeCost;
    }
    
    public int GetredistributionCostCost()
    {
        return redistributionCost;
    }
    void OnApplicationQuit()
    {
        SaveData();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveData();
        }
    }
}