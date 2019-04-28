using System;

public class Player
{
    public int Score{get; set;}
    public int CostToNextUpgrade{get;set;}
    public int CostToUpgradePayPeriod{get;set;}
    public int MoneyLevel {get;set;}
    public int PayPeriodLevel {get;set;}
    public int GambleLvl {get;set;}
    public int UnlockGambleCost {get;set;}
    public int GambleCost {get;set;}
    public int CostToQuit{get;set;}
    public int CostToUnlockLove{get;set;}
    public bool LoveUnlocked {get;set;}

    public Player(int upgradeCost, int payPeriodCost, int unlockGambleCost, int costToQutit, int costToUnlockLove)
    {
        Score = 0;
        GambleCost = 0;
        MoneyLevel = 1;
        CostToNextUpgrade = upgradeCost;
        CostToUpgradePayPeriod = payPeriodCost;
        UnlockGambleCost = unlockGambleCost;
        CostToQuit = costToQutit;
        LoveUnlocked = false;
        CostToUnlockLove = costToUnlockLove;
    }

    public void IncreaseScore()
    {
        Score += MoneyLevel;
    }
}