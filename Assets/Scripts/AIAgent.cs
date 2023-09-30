using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIAgent : Agent
{
    Dictionary<string, float> pastChoicesValuations = new Dictionary<string, float>();
    string lastSelectedChoiceID;

    float decayFactor = 0.75f; // TODO

    public Choice SelectChoice(List<Choice> choices)
    {
        List<Choice> unseenChoices = choices.Where(c => !pastChoicesValuations.ContainsKey(c.ID)).ToList();
        if (unseenChoices.Count > 0)
        {
            Choice randomChoice = unseenChoices[Random.Range(0, unseenChoices.Count)];
            lastSelectedChoiceID = randomChoice.ID;
            return randomChoice;
        }

        Choice selectedChoice = choices[0];
        float selectedChoiceValuation = pastChoicesValuations[selectedChoice.ID];
        foreach (var choice in choices)
        {
            if (pastChoicesValuations[choice.ID] > selectedChoiceValuation)
            {
                selectedChoice = choice;
                selectedChoiceValuation = pastChoicesValuations[selectedChoice.ID];
            }
        }

        lastSelectedChoiceID = selectedChoice.ID;
        return selectedChoice;
    }

    public override void UpdateStatus(int money, float health, float happyness)
    {
        base.UpdateStatus(money, health, happyness);
        float choiceValuation = money / Constants.STARTING_MONEY + health + happyness;
        if (pastChoicesValuations.ContainsKey(lastSelectedChoiceID))
        {
            pastChoicesValuations[lastSelectedChoiceID] = pastChoicesValuations[lastSelectedChoiceID] * decayFactor + choiceValuation * (1 - decayFactor);
        }
        else
        {
            pastChoicesValuations[lastSelectedChoiceID] = choiceValuation;
        }
    }
}