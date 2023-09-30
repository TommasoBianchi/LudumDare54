using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Scene[] allScenes;

    public SceneUIManager sceneUIManager;

    static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Duplicate GameManager, autodestroying");
            Destroy(this.gameObject);
        }

        instance = this;

        // TMP
        sceneUIManager.DisplayScene(allScenes[0]);
        sceneUIManager.DisplayChoices(allScenes[0].possibleChoices);
    }

    public static void SelectPlayerChoice(Choice choice)
    {
        // TMP
        instance.sceneUIManager.HideChoices();
    }
}
