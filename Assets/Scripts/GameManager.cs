using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Scene[] allScenes;
    public float waitTimeAfterEndDay = 2.0f;
    public float waitTimeBeforeDisplayChoices = 3.0f;
    public float waitTimeBeforePlayerStatusUpdateDisplay = 1.5f;
    public float waitTimeAfterPlayerStatusUpdateDisplay = 1.5f;
    public GameObject endDayPanel;
    public GameObject gameOverPanel;

    public SceneUIManager sceneUIManager;
    public StatusUIManager statusUIManager;
    public GameObject flavorTextPanel;

    static GameManager instance;

    Agent playerAgent;
    List<AIAgent> aiAgents;

    Dictionary<string, Choice> allChoices;

    int currentSceneIndex = -1;
    List<Choice> currentScenePlayerChoices;

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

        StartDay();
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

    IEnumerator NextScene()
    {
        // Update the scene
        currentSceneIndex = (currentSceneIndex + 1) % allScenes.Length;
        Scene currentScene = allScenes[currentSceneIndex];

        // Select choices (based also on history) for the player
        currentScenePlayerChoices = currentScene.SelectAvailableChoices(playerAgent.history);

        sceneUIManager.DisplayScene(currentScene);
        yield return new WaitForSeconds(waitTimeBeforeDisplayChoices);
        sceneUIManager.DisplayChoices(currentScenePlayerChoices);
    }

    public void StartDay()
    {
        if (currentSceneIndex >= 0)
        {
            // Only if this is not the first day, display salary
            statusUIManager.DisplayAgentStatus(playerAgent);
        }

        StartCoroutine(NextScene());
    }

    void EndDay()
    {
        // Give salaries and other end-of-day stats change
        // TODO: compute salary based on day choices
        int playerSalary = Constants.BASE_MONEY_PER_DAY;
        float playerHealthChange = Constants.BASE_HEALTH_PER_DAY;
        float playerHappynessChange = Constants.BASE_HAPPYNESS_PER_DAY;
        playerAgent.UpdateStatus(playerSalary, playerHealthChange, playerHappynessChange);
        if (playerAgent.IsBroken())
        {
            GameOver();
            return;
        }

        foreach (var agent in aiAgents)
        {
            // TODO: compute salary based on day choices
            int agentSalary = Constants.BASE_MONEY_PER_DAY;
            float agentHealthChange = Constants.BASE_HEALTH_PER_DAY;
            float agentHappynessChange = Constants.BASE_HAPPYNESS_PER_DAY;
            agent.UpdateStatus(agentSalary, agentHealthChange, agentHappynessChange);
        }

        // Update all agents' history
        playerAgent.history.StartDay();
        foreach (var agent in aiAgents)
        {
            agent.history.StartDay();
        }

        // Display end of day panel
        endDayPanel.SetActive(true);
        flavorTextPanel.SetActive(false);
    }

    public static void SelectPlayerChoice(Choice choice)
    {
        instance.sceneUIManager.StopChoices();
        instance.StartCoroutine(instance.SelectPlayerChoiceCoroutine(choice));
    }

    IEnumerator SelectPlayerChoiceCoroutine(Choice choice)
    {
        // Update player agent history
        playerAgent.history.AddChoice(choice);

        // Make also AI agents select their choice
        Choice bias = allScenes[currentSceneIndex].possibleChoices[Random.Range(0, allScenes[currentSceneIndex].possibleChoices.Count - 1)];
        var aiChoices = aiAgents
            .Select(a => a.SelectChoice(allScenes[currentSceneIndex].SelectAvailableChoices(a.history), bias));

        // Count how many agents have made any choice (including the player)
        var choiceCounters = aiChoices
            .Concat(new List<Choice>() { choice })
            .GroupBy(c => c.ID).Select(g => (g.Key, g.Count()))
            .ToDictionary(pair => pair.Item1, pair => pair.Item2);

        // Compute outcomes for each choice
        var choiceOutcomes = choiceCounters.ToDictionary(el => el.Key, el => allChoices[el.Key].ComputeOutcome(el.Value));

        // Visualize all agents' choices
        sceneUIManager.DisplayChoiceIndicators(choiceCounters);
        yield return new WaitForSeconds(waitTimeBeforePlayerStatusUpdateDisplay);
        // Update player status (including UI)
        statusUIManager.DisplayAgentStatus(playerAgent);
        playerAgent.UpdateStatus(choiceOutcomes[choice.ID]);
        yield return new WaitForSeconds(waitTimeAfterPlayerStatusUpdateDisplay);

        // Update other agents' statuses
        if (playerAgent.IsBroken())
        {
            GameOver();
            yield break;
        }
        foreach (var (agent, agentChoice) in aiAgents.Zip(aiChoices, (a, c) => (a, c)))
        {
            agent.UpdateStatus(choiceOutcomes[agentChoice.ID]);
        }

        // Update UI
        sceneUIManager.HideChoices();

        // If this is the last scene of the day, end it
        if (currentSceneIndex == allScenes.Length - 1)
        {
            EndDay();
        }
        // Otherwise, Advance to next scene
        else
        {
            StartCoroutine(NextScene());
        }
    }

    public void GameOver()
    {
        sceneUIManager.HideChoices();
        flavorTextPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }
}
