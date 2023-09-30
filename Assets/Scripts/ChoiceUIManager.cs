using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceUIManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public CanvasGroup canvasGroup;
    public Button button;
    public float appearDuration;

    Choice currentChoice;
    float startAppearTime;
    float endAppearTime;

    void Update()
    {
        canvasGroup.alpha = Mathf.Clamp01((Time.time - startAppearTime) / (endAppearTime - startAppearTime));
    }

    public void DisplayChoice(Choice choice, float delay)
    {
        currentChoice = choice;
        text.SetText(choice.displayText);

        canvasGroup.alpha = 0;
        startAppearTime = Time.time + delay;
        endAppearTime = startAppearTime + appearDuration;

        button.interactable = true;
    }

    public void StopInteractions()
    {
        button.interactable = false;
    }

    public void OnChoiceSelected()
    {
        GameManager.SelectPlayerChoice(currentChoice);
    }
}
