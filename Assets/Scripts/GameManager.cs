using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Scene[] allScenes;

    public SceneUIManager sceneUIManager;
    public StatusUIManager statusUIManager;

    static GameManager instance;

    Agent playerAgent;
    // AI agents

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

        SetupAgents();

        NextScene();
    }

    void SetupAgents()
    {
        playerAgent = new Agent();
        statusUIManager.DisplayAgentStatus(playerAgent);
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
        // TODO: compute choices for all other agents
        // TODO: compute outcomes for all other choices, based on other agents' choices
        var outcomes = choice.maxPositiveOutcomes;
        instance.playerAgent.UpdateStatus(outcomes);
        instance.statusUIManager.DisplayAgentStatus(instance.playerAgent);
        instance.sceneUIManager.HideChoices();
        instance.NextScene();
    }
}
