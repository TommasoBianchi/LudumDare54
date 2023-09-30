using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusUIManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public Slider healthBar;
    public Slider happynessBar;

    public FloatingIncrementUI moneyIncrementUI;
    public FloatingIncrementUI healthIncrementUI;
    public FloatingIncrementUI happynessIncrementUI;

    int oldMoney = -1;
    float oldHealth;
    float oldHappyness;

    public void DisplayAgentStatus(Agent agent)
    {
        DisplayStatus(agent.currentMoney, agent.currentHealth, agent.currentHappyness);
    }

    public void DisplayStatus(int money, float health, float happyness)
    {
        moneyText.SetText(money.ToString());
        healthBar.value = health;
        happynessBar.value = happyness;

        if (oldMoney >= 0)
        {
            int moneyIncrement = money - oldMoney;
            moneyIncrementUI.Show((moneyIncrement > 0 ? "+" : "") + moneyIncrement.ToString());
            float healthIncrement = Mathf.Round((health - oldHealth) * 100);
            healthIncrementUI.Show((healthIncrement > 0 ? "+" : "") + healthIncrement.ToString() + "%");
            float happynessIncrement = Mathf.Round((happyness - oldHappyness) * 100);
            happynessIncrementUI.Show((happynessIncrement > 0 ? "+" : "") + happynessIncrement.ToString() + "%");
        }

        oldMoney = money;
        oldHealth = health;
        oldHappyness = happyness;
    }
}