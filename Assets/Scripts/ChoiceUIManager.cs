using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceUIManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI crowdLevelText;
    public Color crowdLevelPositiveColor;
    public Color crowdLevelFullColor;
    public Color crowdLevelNegativeColor;
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
        crowdLevelText.gameObject.SetActive(false);

        currentChoice = choice;
        text.SetText(choice.displayText);

        canvasGroup.alpha = 0;
        startAppearTime = Time.time + delay;
        endAppearTime = startAppearTime + appearDuration;

        button.interactable = true;
    }

    public void DisplayCrowdLevel(int numberOfPeople)
    {
        StartCoroutine(DisplayCrowdLevelCoroutine(numberOfPeople));
    }

    public IEnumerator DisplayCrowdLevelCoroutine(int numberOfPeople)
    {
        yield return new WaitForSeconds(0.75f);

        Color targetColor = numberOfPeople > currentChoice.space ? crowdLevelNegativeColor : crowdLevelPositiveColor;
        var lerpAmount = numberOfPeople > currentChoice.space ? ((float)numberOfPeople) / Constants.MAX_PEOPLE : 1 - ((float)numberOfPeople) / currentChoice.space;

        crowdLevelText.color = Color.Lerp(crowdLevelFullColor, targetColor, lerpAmount);

        string message = "Empty";
        if (numberOfPeople > 1 && numberOfPeople < currentChoice.space)
        {
            message = "Not too crowded";
        }
        else if (numberOfPeople == currentChoice.space)
        {
            message = "Perfect";
        }
        else if (numberOfPeople < Constants.MAX_PEOPLE / 2)
        {
            message = "Crowded";
        }
        else if (numberOfPeople < Constants.MAX_PEOPLE)
        {
            message = "Very Crowded";
        }
        else
        {
            message = "Insanely full";
        }
        crowdLevelText.SetText(message);
        crowdLevelText.gameObject.SetActive(true);
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
