namespace Interfaces
{
    public interface IHitterPlayerManager
    {
        float CurrentPower { get; }
        float CurrentJudgeSight { get; }
        float CurrentCritical { get; }
    }

    // IShopPlayerManager.cs  
    public interface IShopPlayerManager
    {
        int Currency { get; }
        void SpendCurrency(int cost);
        bool HasEnoughCurrency(int cost);
        void AddCurrency(int cost);
        void Redistribution();
        void Upgrade();
        int GetUpgradeCost();
        int GetredistributionCostCost();
        void Initialization();
    }
}
