using UnityEngine;

public class Agent
{
    public int currentMoney { get; private set; }
    public float currentHealth { get; private set; }
    public float currentHappyness { get; private set; }

    public Agent()
    {
        currentMoney = Constants.STARTING_MONEY;
        currentHealth = Constants.STARTING_HEALTH;
        currentHappyness = Constants.STARTING_HAPPYNESS;
    }

    public void UpdateStatus(ChoiceOutcome choiceOutcomes)
    {
        UpdateStatus(choiceOutcomes.money, choiceOutcomes.health, choiceOutcomes.happyness);
    }

    public void UpdateStatus(int money, float health, float happyness)
    {
        currentMoney += money;
        currentHealth = Mathf.Clamp01(currentHealth + health);
        currentHappyness = Mathf.Clamp01(currentHappyness + happyness);
    }
}