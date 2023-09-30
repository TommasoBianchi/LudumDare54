using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIAgent : Agent
{
    Dictionary<string, float> pastChoicesValuations = new Dictionary<string, float>();
    string lastSelectedChoiceID;

    float decayFactor = 0.75f; // TODO
    float biasFactor = 0.5f;

    public Choice SelectChoice(List<Choice> choices, Choice bias)
    {
        Choice selectedChoice;

        List<Choice> unseenChoices = choices.Where(c => !pastChoicesValuations.ContainsKey(c.ID)).ToList();
        if (unseenChoices.Count > 0)
        {
            if (Random.Range(0.0f, 1.0f) < biasFactor && unseenChoices.Contains(bias))
            {
                selectedChoice = bias;
            }
            else
            {
                selectedChoice = unseenChoices[Random.Range(0, unseenChoices.Count)];
            }
        }
        else
        {
            selectedChoice = choices[0];
            float selectedChoiceValuation = pastChoicesValuations[selectedChoice.ID];
            foreach (var choice in choices)
            {
                if (pastChoicesValuations[choice.ID] > selectedChoiceValuation)
                {
                    selectedChoice = choice;
                    selectedChoiceValuation = pastChoicesValuations[selectedChoice.ID];
                }
            }
        }

        Debug.Log("Agent selected " + selectedChoice);
        history.AddChoice(selectedChoice);
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