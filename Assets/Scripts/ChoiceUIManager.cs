using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceUIManager : MonoBehaviour
{
    public Image choiceImage;

    Choice currentChoice;

    public void DisplayChoice(Choice choice)
    {
        currentChoice = choice;
        choiceImage.sprite = choice.cardImage;
    }

    public void OnChoiceSelected()
    {
        GameManager.SelectPlayerChoice(currentChoice);
    }
}
