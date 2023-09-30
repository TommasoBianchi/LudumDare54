using UnityEngine;

public class Agent
{
    public int currentMoney { get; private set; }
    public float currentHealth { get; private set; }
    public float currentHappyness { get; private set; }

    public History history { get; private set; }

    public Agent()
    {
        currentMoney = Constants.STARTING_MONEY;
        currentHealth = Constants.STARTING_HEALTH;
        currentHappyness = Constants.STARTING_HAPPYNESS;

        history = new History();
        history.StartDay();
    }

    public bool IsBroken()
    {
        return currentMoney <= 0 || currentHealth <= 0 || currentHappyness <= 0;
    }

    public void UpdateStatus(ChoiceOutcome choiceOutcomes)
    {
        UpdateStatus(choiceOutcomes.money, choiceOutcomes.health, choiceOutcomes.happyness);
    }

    public virtual void UpdateStatus(int money, float health, float happyness)
    {
        currentMoney += money;
        currentHealth = Mathf.Clamp01(currentHealth + health);
        currentHappyness = Mathf.Clamp01(currentHappyness + happyness);
    }
}