using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Scene[] allScenes;

    public SceneUIManager sceneUIManager;
    public StatusUIManager statusUIManager;

    static GameManager instance;

    Agent playerAgent;
    List<AIAgent> aiAgents;

    Dictionary<string, Choice> allChoices;

    int currentSceneIndex = -1;
    List<Choice> currentSceneSelectedChoices;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Duplicate GameManager, autodestroying");
            Destroy(this.gameObject);
        }

        instance = this;

        allChoices = allScenes
            .Select(s => s.possibleChoices)
            .Aggregate(new List<Choice>(), (l1, l2) => l1.Concat(l2).ToList())
            .ToDictionary(c => c.ID, c => c);

        SetupAgents();

        NextScene();
    }

    void SetupAgents()
    {
        playerAgent = new Agent();
        statusUIManager.DisplayAgentStatus(playerAgent);

        aiAgents = new List<AIAgent>();
        for (int i = 0; i < Constants.MAX_PEOPLE; i++)
        {
            aiAgents.Add(new AIAgent());
        }
    }

    void NextScene()
    {
        bool lastSceneOfDay = currentSceneIndex == allScenes.Length - 1;
        currentSceneIndex = (currentSceneIndex + 1) % allScenes.Length;
        Scene currentScene = allScenes[currentSceneIndex];

        // TODO: select choices based also on history
        currentSceneSelectedChoices = currentScene.possibleChoices; // TMP

        // TODO: add timings if necessary (use coroutine)
        sceneUIManager.DisplayScene(currentScene);
        sceneUIManager.DisplayChoices(currentSceneSelectedChoices);

        if (lastSceneOfDay)
        {
            EndDay(); // TODO: display some kind of end day screen
        }
    }

    void EndDay()
    {
        // TODO: compute salary based on day choices
        int playerSalary = Constants.BASE_MONEY_PER_DAY;
        float playerHealthChange = Constants.BASE_HEALTH_PER_DAY;
        float playerHappynessChange = Constants.BASE_HAPPYNESS_PER_DAY;
        playerAgent.UpdateStatus(playerSalary, playerHealthChange, playerHappynessChange);

        // TODO: give salary also to other agents
    }

    public static void SelectPlayerChoice(Choice choice)
    {
        // Make also AI agents select their choice
        var aiChoices = instance.aiAgents.Select(a => a.SelectChoice(instance.currentSceneSelectedChoices));

        // Count how many agents have made any choice (including the player)
        var choiceCounters = aiChoices
            .Concat(new List<Choice>() { choice })
            .GroupBy(c => c.ID).Select(g => (g.Key, g.Count()))
            .ToDictionary(pair => pair.Item1, pair => pair.Item2);

        // TMP
        Debug.Log(choiceCounters.Select(el => el.Key + " - " + el.Value).Aggregate((a, b) => a + "\n" + b));

        // Compute outcomes for each choice
        var choiceOutcomes = choiceCounters.ToDictionary(el => el.Key, el => instance.allChoices[el.Key].ComputeOutcome(el.Value));

        // Update agents' statuses
        instance.playerAgent.UpdateStatus(choiceOutcomes[choice.ID]);
        foreach (var (agent, agentChoice) in instance.aiAgents.Zip(aiChoices, (a, c) => (a, c)))
        {
            agent.UpdateStatus(choiceOutcomes[agentChoice.ID]);
        }

        // Update UI
        instance.statusUIManager.DisplayAgentStatus(instance.playerAgent);
        instance.sceneUIManager.HideChoices();

        // Advance to next scene
        instance.NextScene();
    }
}
