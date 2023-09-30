using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scene", menuName = "Scriptable Objects/Scene", order = 0)]
public class Scene : ScriptableObject
{
    public string ID;
    public Sprite backgroundImage;
    public List<Choice> possibleChoices;
}