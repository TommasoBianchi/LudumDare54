using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ChoiceRequirement
{
    public Choice choice;
    public int daysSinceTaken;
}

[System.Serializable]
public struct ChoiceOutcome
{
    public int money;
    [Range(-1f, 1f)]
    public float health;
    [Range(-1f, 1f)]
    public float happyness;

    ChoiceOutcome(int money, float health, float happyness)
    {
        this.money = money;
        this.health = health;
        this.happyness = happyness;
    }

    public ChoiceOutcome Lerp(ChoiceOutcome other, float t)
    {
        return new ChoiceOutcome(
            Mathf.RoundToInt(Mathf.Lerp(money, other.money, t)),
            Mathf.Lerp(health, other.health, t),
            Mathf.Lerp(happyness, other.happyness, t)
        );
    }
}

[CreateAssetMenu(fileName = "Choice", menuName = "Scriptable Objects/Choice", order = 1)]
public class Choice : ScriptableObject
{
    public string ID;
    public string displayText;
    public List<ChoiceRequirement> requirements;
    public int space;
    public ChoiceOutcome maxPositiveOutcomes;
    public ChoiceOutcome fullSpaceOutcomes;
    public ChoiceOutcome maxNegativeOutcomes;

    public ChoiceOutcome ComputeOutcome(int numberOfPeople)
    {
        var maxOutcome = numberOfPeople > space ? maxNegativeOutcomes : maxPositiveOutcomes;
        var lerpAmount = numberOfPeople > space ? ((float)numberOfPeople) / Constants.MAX_PEOPLE : 1 - ((float)numberOfPeople) / space;
        return fullSpaceOutcomes.Lerp(maxOutcome, lerpAmount);
    }
}