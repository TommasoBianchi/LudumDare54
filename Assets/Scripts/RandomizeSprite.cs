using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomizeSprite : MonoBehaviour
{
    public Image targetImage;
    public List<Sprite> sprites;

    void Awake()
    {
        targetImage.sprite = sprites[Random.Range(0, sprites.Count)];
    }
}
