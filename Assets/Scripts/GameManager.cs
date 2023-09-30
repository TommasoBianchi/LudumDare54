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

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Duplicate GameManager, autodestroying");
            Destroy(this.gameObject);
        }

        instance = this;

        SetupAgents();

        // TMP
        sceneUIManager.DisplayScene(allScenes[0]);
        sceneUIManager.DisplayChoices(allScenes[0].possibleChoices);
    }

    void SetupAgents()
    {
        playerAgent = new Agent();
        statusUIManager.DisplayAgentStatus(playerAgent);
    }

    public static void SelectPlayerChoice(Choice choice)
    {
        // TODO: compute choices for all other agents
        // TODO: compute outcomes for all other choices, based on other agents' choices
        var outcomes = choice.maxPositiveOutcomes;
        instance.playerAgent.UpdateStatus(outcomes);
        instance.statusUIManager.DisplayAgentStatus(instance.playerAgent);
        instance.sceneUIManager.HideChoices();
    }
}
