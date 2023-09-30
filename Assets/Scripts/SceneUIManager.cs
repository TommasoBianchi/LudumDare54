using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneUIManager : MonoBehaviour
{
    public Image backgroundScenePanel;
    public Transform choicesPanel;
    public float timeBetweenChoicesDisplay;
    public RectTransform agentChoiceIndicatorsContainer;

    public MoveToRect agentChoiceIndicatorPrefab;

    List<MoveToRect> agentChoiceIndicators = new List<MoveToRect>();
    Dictionary<string, RectTransform> choiceIDToRectTransform = new Dictionary<string, RectTransform>();

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
            if (!choiceIDToRectTransform.ContainsKey(element.Key))
            {
                // This is likely a choice that some AI agent made but that was not available to the player
                continue;
            }

            RectTransform target = choiceIDToRectTransform[element.Key];
            for (int i = 0; i < element.Value && firstFreeIndicatorIndex + i < agentChoiceIndicators.Count; ++i)
            {
                agentChoiceIndicators[firstFreeIndicatorIndex + i].Activate(target);
            }
            firstFreeIndicatorIndex += element.Value;
        }
    }

    public void DisplayScene(Scene scene)
    {
        currentScene = scene;
        backgroundScenePanel.sprite = scene.backgroundImage;
    }

    public void DisplayChoices(List<Choice> choices)
    {
        choiceIDToRectTransform.Clear();

        for (int i = 0; i < choicesPanel.childCount; i++)
        {
            choicesPanel.GetChild(i).GetComponent<ChoiceUIManager>().DisplayChoice(choices[i], i * timeBetweenChoicesDisplay);
            choiceIDToRectTransform[choices[i].ID] = choicesPanel.GetChild(i).GetComponent<RectTransform>();
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
