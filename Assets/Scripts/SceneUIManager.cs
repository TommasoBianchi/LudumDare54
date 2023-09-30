using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneUIManager : MonoBehaviour
{
    public Image backgroundScenePanel;
    public Transform choicesPanel;

    Scene currentScene;

    public void DisplayScene(Scene scene)
    {
        currentScene = scene;
        backgroundScenePanel.sprite = scene.backgroundImage;
    }

    public void DisplayChoices(List<Choice> choices)
    {
        for (int i = 0; i < choicesPanel.childCount; i++)
        {
            choicesPanel.GetChild(i).GetComponent<ChoiceUIManager>().DisplayChoice(choices[i]);
        }
        choicesPanel.gameObject.SetActive(true);
    }

    public void HideChoices()
    {
        choicesPanel.gameObject.SetActive(false);
    }
}
