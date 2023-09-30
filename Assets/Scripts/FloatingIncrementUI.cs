using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingIncrementUI : MonoBehaviour
{
    public float speed = 1.0f;
    public float moveDuration = 1.0f;
    public float fadeDuration = 1.0f;
    public TextMeshProUGUI text;

    RectTransform rectTransform;
    Vector3 startingPoint;
    bool isActive;
    float startTime;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startingPoint = rectTransform.anchoredPosition;
        isActive = false;
        gameObject.SetActive(false);
    }

    public void Show(string displayText)
    {
        text.SetText(displayText);
        rectTransform.anchoredPosition = startingPoint;
        isActive = true;
        startTime = Time.time;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isActive)
        {
            return;
        }

        float t = Time.time - startTime;

        if (t <= moveDuration)
        {
            rectTransform.anchoredPosition = startingPoint + (Vector3)(Vector2.up * speed * t);
        }

        if (t <= fadeDuration)
        {
            Color color = text.color;
            color.a = 1 - t / fadeDuration;
            text.color = color;
        }
    }
}