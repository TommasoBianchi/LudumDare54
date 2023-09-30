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

    public void DisplayAgentStatus(Agent agent)
    {
        DisplayStatus(agent.currentMoney, agent.currentHealth, agent.currentHappyness);
    }

    public void DisplayStatus(int money, float health, float happyness)
    {
        moneyText.SetText(money.ToString());
        healthBar.value = health;
        happynessBar.value = happyness;
    }
}