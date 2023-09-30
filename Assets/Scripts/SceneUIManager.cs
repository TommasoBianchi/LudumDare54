using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneUIManager : MonoBehaviour
{
    public Image backgroundScenePanel;
    public Transform choicesPanel;
    public float timeBetweenChoicesDisplay;
    public RectTransform agentChoiceIndicatorsContainer;
    public TextMeshProUGUI flavorText;

    public MoveToRect agentChoiceIndicatorPrefab;

    List<MoveToRect> agentChoiceIndicators = new List<MoveToRect>();
    Dictionary<string, ChoiceUIManager> choiceIDToUIManager = new Dictionary<string, ChoiceUIManager>();

    Scene currentScene;

    void Awake()
    {
        for (int i = 0; i < Constants.MAX_PEOPLE; i++)
        {
            float randomAngle = Random.Range(0, 2 * Mathf.PI);
            Vector3 randomOutOfScreenPosition = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * 1500;
            MoveToRect agentIndicator = Instantiate(
                agentChoiceIndicatorPrefab,
                randomOutOfScreenPosition + transform.position,
                Quaternion.identity,
                agentChoiceIndicatorsContainer
            );
            agentChoiceIndicators.Add(agentIndicator);
        }
    }

    public void DisplayChoiceIndicators(Dictionary<string, int> choicesCounter)
    {
        int firstFreeIndicatorIndex = 0;
        foreach (var element in choicesCounter)
        {
            if (!choiceIDToUIManager.ContainsKey(element.Key))
            {
                // This is likely a choice that some AI agent made but that was not available to the player
                continue;
            }

            ChoiceUIManager choiceUIManager = choiceIDToUIManager[element.Key];
            choiceUIManager.DisplayCrowdLevel(element.Value);
            for (int i = 0; i < element.Value && firstFreeIndicatorIndex + i < agentChoiceIndicators.Count; ++i)
            {
                agentChoiceIndicators[firstFreeIndicatorIndex + i].Activate(choiceUIManager.gameObject.GetComponent<RectTransform>());
            }
            firstFreeIndicatorIndex += element.Value;
        }
    }

    public void DisplayScene(Scene scene)
    {
        currentScene = scene;
        backgroundScenePanel.sprite = scene.backgroundImage;
        flavorText.SetText(scene.flavorText);
        flavorText.transform.parent.gameObject.SetActive(true);
        flavorText.gameObject.SetActive(true);
    }

    public void DisplayChoices(List<Choice> choices)
    {
        choiceIDToUIManager.Clear();

        for (int i = 0; i < choicesPanel.childCount; i++)
        {
            ChoiceUIManager choiceUIManager = choicesPanel.GetChild(i).GetComponent<ChoiceUIManager>();
            choiceUIManager.DisplayChoice(choices[i], i * timeBetweenChoicesDisplay);
            choiceIDToUIManager[choices[i].ID] = choiceUIManager;
        }
        choicesPanel.gameObject.SetActive(true);
    }

    public void StopChoices()
    {
        for (int i = 0; i < choicesPanel.childCount; i++)
        {
            choicesPanel.GetChild(i).GetComponent<ChoiceUIManager>().StopInteractions();
        }
    }

    public void HideChoices()
    {
        choicesPanel.gameObject.SetActive(false);
    }
}
