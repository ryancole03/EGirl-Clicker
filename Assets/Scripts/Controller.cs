using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public UpgradesManager upgradesManager;
    public Data data;

    [SerializeField] private Text balanceText;
    [SerializeField] private Text balanceText2;
    [SerializeField] private Text gainPerSecond;
    [SerializeField] private Text gainPerSecond2;
    public double clickPower() { return 1 + data.clickUpgradeLevel; }

    public void Start()
    {
        data = new Data();

        upgradesManager.UpgradeManagerStart();

        StartCoroutine(AutoClickUpdate());
        StartCoroutine(ClickFarmUpdate());
    }

    IEnumerator AutoClickUpdate()
    {
        float speed = upgradesManager.CalcAutoClickSpeed();
        float autoClickUpgradeCurrentSpeed = upgradesManager.CalcAutoClickSpeed();

        while (true)
        {
            AddBalance(upgradesManager.AutoClickProductionFormula());

            yield return new WaitForSeconds(speed);
        }            
    }

    IEnumerator ClickFarmUpdate()
    {
        float speed = upgradesManager.CalcClickFarmSpeed();
        float clickFarmUpgradeCurrentSpeed = upgradesManager.CalcClickFarmSpeed();

        while (true)
        {
            AddBalance(upgradesManager.ClickFarmProductionFormula());

            yield return new WaitForSeconds(speed);
        }   
    }

    public void Update()
    {
        balanceText.text = CashFormat(data.balance) + "  E-Girls";
        balanceText2.text = CashFormat(data.balance) + " E-Girls";
        gainPerSecond.text = "Automation: " + CashFormat(Round(CalcGainPerSecond())) + " /s";
        gainPerSecond2.text = "Automation: " + CashFormat(Round(CalcGainPerSecond())) + " /s";
    }

    private double Round(double input)
    {
        if (input !> 999)
        {
            return Math.Round(input);
        }
        else
        {
            return Math.Round(input, 1);
        }
    }    

    public double CalcGainPerSecond() { return upgradesManager.AutoClickProductionFormula() + upgradesManager.ClickFarmProductionFormula(); }

    public string CashFormat(double balance)
    {
        string result;
        string[] ScoreNames = new string[] { "", "k", "M", "B", "T", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", };
        int i;
 
        for (i = 0; i < ScoreNames.Length; i++)
            if (balance < 1000)
                break;
            else balance = Math.Round(balance / 100, 2) / 10;
 
        if (balance == Math.Round(balance, 2))
            result = balance.ToString() + ScoreNames[i];
        else result = balance.ToString("F2") + ScoreNames[i];
        return result;
    }

    public void AddBalance(double amount)
    {
        data.balance += amount;
    }

    public void SubtractBalance(double amount)
    {
        data.balance -= amount;
    }

    public void Click()
    {
        AddBalance(clickPower());
    }

    public void Exit()
    {
        Application.Quit();
    }
}
