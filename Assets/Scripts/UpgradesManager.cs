using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    public Controller controller;
    public Data data;

    public Upgrades upgrades;

    private double maxUpgradeLevel;

    private double clickUpgradeBaseCost;
    private double clickUpgradeCostMult;

    private double autoClickUpgradeBaseCost;
    private double autoClickUpgradeMult;
    private double autoClickUpgradeProductionBase;
    private double autoClickUpgradeProductionMult;
    private float autoClickUpgradeSpeedBase;
    private float autoClickUpgradeCurrentSpeed;

    public void Update()
    {

    }

    public void UpgradeManagerStart()
    {
        data = controller.data;

        maxUpgradeLevel = 5000;

        clickUpgradeBaseCost = 100;
        clickUpgradeCostMult = 1.1;

        autoClickUpgradeBaseCost = 1000;
        autoClickUpgradeMult = 1.3;
        autoClickUpgradeProductionBase = 100;
        autoClickUpgradeProductionMult = 1.2;
        autoClickUpgradeSpeedBase = 1f;
        UpdateUpgradeUI();
    }

    public void UpdateUpgradeUI()
    {
        upgrades.clickUpgradeLevelText.text = data.clickUpgradeLevel.ToString();
        upgrades.clickUpgradeCostText.text = "$" + controller.CashFormat(ClickUpgradeCost());
        upgrades.autoClickUpgradeLevelText.text = data.autoClickUpgradeLevel.ToString();
        upgrades.autoClickUpgradeCostText.text = "$" + controller.CashFormat(AutoClickUpgradeCost());
    }

    public double ClickUpgradeCost() { return clickUpgradeBaseCost * Math.Pow(clickUpgradeCostMult, data.clickUpgradeLevel); }

    public double AutoClickUpgradeCost() { return autoClickUpgradeBaseCost * Math.Pow(autoClickUpgradeMult, data.autoClickUpgradeLevel); }

    public double AutoClickProductionFormula()
    {
        double production = 0;
        if (data.autoClickUpgradeLevel > 0)
        {
            production = autoClickUpgradeProductionBase * (data.clickUpgradeLevel / 100) * Math.Pow(autoClickUpgradeProductionMult, data.autoClickUpgradeLevel);           
        }

        return production;
    }

    public float CalcAutoClickSpeed()
    {
        float speed = autoClickUpgradeSpeedBase;
        float interval = autoClickUpgradeSpeedBase;

        if (data.autoClickUpgradeLevel >= 25 || data.autoClickUpgradeLevel >= 50)
        {
            interval = speed / 2;
        }
        else if (data.autoClickUpgradeLevel >= 200)
        {
            interval = speed / 3;
        }
        else if (data.autoClickUpgradeLevel >= 500)
        {
            interval = speed / 5;
        }

        return interval;
    }

    public void BuyClickUpgrade()
    {
        double cost = ClickUpgradeCost();

        if (data.clickUpgradeLevel < maxUpgradeLevel)
        {
            if (data.balance >= cost)
            {
                data.balance -= cost;
                data.clickUpgradeLevel++;
            }

            UpdateUpgradeUI();
        }
    }

    public void BuyAutoClickUpgrade()
    {
        double cost = AutoClickUpgradeCost();

        if (data.autoClickUpgradeLevel < maxUpgradeLevel)
        {
            if (data.balance >= cost)
            {
                data.balance -= cost;
                data.autoClickUpgradeLevel++;
            }
            UpdateUpgradeUI();
        }
    }
}
