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
    [SerializeField] private Text gainPerSecond;

    public double clickPower() { return 1 + data.clickUpgradeLevel; }

    public void Start()
    {
        data = new Data();

        upgradesManager.UpgradeManagerStart();

        StartCoroutine(AutoClickUpdate());
    }

    IEnumerator AutoClickUpdate()
    {
        float speed = upgradesManager.CalcAutoClickSpeed();
        float autoClickUpgradeCurrentSpeed = upgradesManager.CalcAutoClickSpeed();

        while (true)
        {
            data.balance += upgradesManager.AutoClickProductionFormula();

            yield return new WaitForSeconds(speed);
        }            
    }

    public void Update()
    {
        balanceText.text = CashFormat(data.balance) + "  E-Girls";
        gainPerSecond.text = "Automation: " + CashFormat(Math.Round(CalcGainPerSecond(), 1)) + " /s";
    }    

    public double CalcGainPerSecond()
    {
        return upgradesManager.AutoClickProductionFormula();
    }

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

    private void AddBalance(double amount)
    {
        data.balance += amount;
    }

    private void SubtractBalance(double amount)
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
