using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
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
                }
                else
                {
                    instance = Create();
                }
            }
            return instance;
        }
    }

    static PlayerManager Create()
    {
        return Instantiate(Resources.Load<PlayerManager>("PlayerManager"));
    }
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] PlayerStatSO baseStats;
    [SerializeField] BatItemSO defaultBat;
    [SerializeField] int currency;//재화

    [Header("Player Random Stats")]
    [SerializeField] int maxValue = 30;
    [SerializeField,ReadOnly] int powerValue;
    [SerializeField,ReadOnly] int judgeValue;
    [SerializeField,ReadOnly] int criticalValue;

    public int PowerValue => powerValue;
    public int JudgeValue => judgeValue;
    public int CriticalValue => criticalValue;
    public int Currency => currency;
    public float CurrentPower => baseStats.BasePower + powerValue + (defaultBat?.powerBonus ?? 0);
    public float CurrentJudgeSight => baseStats.BaseJudgement + judgeValue + (defaultBat?.judgementBonus ?? 0);
    public float CurrentCritical => baseStats.BaseCriticalChance + criticalValue + (defaultBat?.criticalBonus ?? 0);

    public void SpendCurrency(int cost)
    {
        if (cost >= currency) { print("잔약 부족"); return; }
        currency -= cost;
    }
    public bool HasEnoughCurrency(int cost)
    {
        return currency >= cost;
    }
    public void AddCurrency(int cost)
    {
        if (cost <= 0) { print("돈이 없는데용"); return; }
        currency += cost;
    }
    //재분배
    public void Redistribution()
    {
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
        powerValue = ranArr[0];
        judgeValue = ranArr[1];
        criticalValue = ranArr[2];
        EventManager.Instance.PublishPlayerStatsChanged(powerValue, judgeValue, criticalValue);
    }
}