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

    private double clickFarmUpgradeBaseCost;
    private double clickFarmUpgradeMult;
    private double clickFarmUpgradeProductionBase;
    private double clickFarmUpgradeProductionMult;
    private float clickFarmUpgradeSpeedBase;
    private float clickFarmUpgradeCurrentSpeed;  

    public void Update()
    {

    }

    public void UpgradeManagerStart()
    {
        data = controller.data;

        maxUpgradeLevel = 5000;

        clickUpgradeBaseCost = 100;
        clickUpgradeCostMult = 1.13;

        autoClickUpgradeBaseCost = 10000;
        autoClickUpgradeMult = 1.3;
        autoClickUpgradeProductionBase = 100;
        autoClickUpgradeProductionMult = 1.15;
        autoClickUpgradeSpeedBase = 1f;

        clickFarmUpgradeBaseCost = 100000;
        clickFarmUpgradeMult = 1.3;
        clickFarmUpgradeProductionBase = 1000;
        clickFarmUpgradeProductionMult = 1.16;
        clickFarmUpgradeSpeedBase = 3f;
        UpdateUpgradeUI();
    }

    public void UpdateUpgradeUI()
    {
        upgrades.clickUpgradeLevelText.text = data.clickUpgradeLevel.ToString();
        upgrades.clickUpgradeCostText.text = "$" + controller.CashFormat(Round(ClickUpgradeCost()));
        upgrades.autoClickUpgradeLevelText.text = data.autoClickUpgradeLevel.ToString();
        upgrades.autoClickUpgradeCostText.text = "$" + controller.CashFormat(Round(AutoClickUpgradeCost()));
        upgrades.clickFarmUpgradeLevelText.text = data.clickFarmUpgradeLevel.ToString();
        upgrades.clickFarmUpgradeCostText.text = "$" + controller.CashFormat(Round(ClickFarmUpgradeCost()));
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

    public double ClickUpgradeCost() { return clickUpgradeBaseCost * Math.Pow(clickUpgradeCostMult, data.clickUpgradeLevel); }

    public double AutoClickUpgradeCost() { return autoClickUpgradeBaseCost * Math.Pow(autoClickUpgradeMult, data.autoClickUpgradeLevel); }

    public double ClickFarmUpgradeCost() { return clickFarmUpgradeBaseCost * Math.Pow(clickFarmUpgradeMult, data.clickFarmUpgradeLevel); }

    public double AutoClickProductionFormula()
    {
        double production = 0;
        if (data.autoClickUpgradeLevel > 0)
        {
            production = autoClickUpgradeProductionBase * Math.Pow(autoClickUpgradeProductionMult, data.autoClickUpgradeLevel);      

            if (data.clickUpgradeLevel > 0)
            {
                production += data.clickUpgradeLevel * (production * 0.01);
            }
        }

        return production;
    }

    public double ClickFarmProductionFormula()
    {
        double production = 0;
        if (data.clickFarmUpgradeLevel > 0)
        {
            production = clickFarmUpgradeProductionBase * (1 + data.autoClickUpgradeLevel / 100) * Math.Pow(clickFarmUpgradeProductionMult, data.clickFarmUpgradeLevel);

            if (data.autoClickUpgradeLevel > 0)
            {
                production += data.autoClickUpgradeLevel * (production * 0.01);
            }
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

    public float CalcClickFarmSpeed()
    {
        float speed = clickFarmUpgradeSpeedBase;
        float interval = clickFarmUpgradeSpeedBase;

        if (data.clickFarmUpgradeLevel >= 25 || data.clickFarmUpgradeLevel >= 50)
        {
            interval = speed / 1.5f;
        }
        else if (data.clickFarmUpgradeLevel >= 200)
        {
            interval = speed / 2;
        }
        else if (data.clickFarmUpgradeLevel >= 500)
        {
            interval = speed / 3.5f;
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
                controller.SubtractBalance(cost);
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
                controller.SubtractBalance(cost);
                data.autoClickUpgradeLevel++;
            }
            UpdateUpgradeUI();
        }
    }

    public void BuyClickFarmUpgrade()
    {
        double cost = ClickFarmUpgradeCost();

        if (data.clickFarmUpgradeLevel < maxUpgradeLevel)
        {
            if (data.balance >= cost)
            {
                controller.SubtractBalance(cost);
                data.clickFarmUpgradeLevel++;
            }
            UpdateUpgradeUI();
        }
    }
}
