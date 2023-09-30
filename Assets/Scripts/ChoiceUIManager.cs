using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceUIManager : MonoBehaviour
{
    public Image choiceImage;
    public TextMeshProUGUI titleText;
    public CanvasGroup canvasGroup;
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
        choiceImage.sprite = choice.cardImage;

        canvasGroup.alpha = 0;
        startAppearTime = Time.time + delay;
        endAppearTime = startAppearTime + appearDuration;

        // TMP
        titleText.SetText(choice.ID);
    }

    public void OnChoiceSelected()
    {
        GameManager.SelectPlayerChoice(currentChoice);
    }
}
