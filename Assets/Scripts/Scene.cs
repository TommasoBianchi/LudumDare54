using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Scene", menuName = "Scriptable Objects/Scene", order = 0)]
public class Scene : ScriptableObject
{
    public string ID;
    public Sprite backgroundImage;
    public List<Choice> possibleChoices;
    public string flavorText;

    public List<Choice> SelectAvailableChoices(History agentHistory)
    {
        // Filter requirements
        var allAvailableChoices = possibleChoices
            .Where(c => c.requirements.All(r => agentHistory.DaysSinceChoiceTaken(r.choice.ID) <= r.daysSinceTaken)).ToList();

        // Shuffle (Fisher-Yates algorithm)
        for (int i = allAvailableChoices.Count - 1; i >= 1; --i)
        {
            int j = Random.Range(0, i + 1);
            var tmp = allAvailableChoices[j];
            allAvailableChoices[j] = allAvailableChoices[i];
            allAvailableChoices[i] = tmp;
        }

        return allAvailableChoices.Take(3).ToList();
    }
}