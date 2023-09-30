using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceUIManager : MonoBehaviour
{
    public Image choiceImage;
    public TextMeshProUGUI titleText;
    Choice currentChoice;

    public void DisplayChoice(Choice choice)
    {
        currentChoice = choice;
        choiceImage.sprite = choice.cardImage;
        // TMP
        titleText.SetText(choice.ID);
    }

    public void OnChoiceSelected()
    {
        GameManager.SelectPlayerChoice(currentChoice);
    }
}
