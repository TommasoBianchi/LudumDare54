using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class MoveToRect : MonoBehaviour
{
    public float moveDuration = 1.0f;
    public float waitAfterMoveDuration = 2.0f;

    RectTransform rectTransform;
    Vector3 startingPoint;
    Vector3 targetPoint;
    bool isActive;
    float startTime;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startingPoint = rectTransform.position;
        isActive = false;
        gameObject.SetActive(false);
    }

    public void Activate(RectTransform target)
    {
        rectTransform.position = startingPoint;
        Rect targetRect = target.GetWorldRect();
        float xDispersion = targetRect.width / 2.0f * 0.8f;
        float yDispersion = targetRect.height / 2.0f * 0.8f;
        targetPoint = new Vector3(
            Random.Range(target.position.x - xDispersion, target.position.x + xDispersion),
            Random.Range(target.position.y - yDispersion, target.position.y + yDispersion),
            0
        );
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

        if (t > moveDuration + waitAfterMoveDuration)
        {
            isActive = false;
            gameObject.SetActive(false);
            return;
        }

        rectTransform.position = Vector3.Lerp(startingPoint, targetPoint, t / moveDuration);
    }
}

// Taken from https://discussions.unity.com/t/convert-recttransform-rect-to-rect-world/153391/3
public static class RectTransformExtensions
{
    public static Rect GetWorldRect(this RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        // Get the bottom left corner.
        Vector3 position = corners[0];

        Vector2 size = new Vector2(
            rectTransform.lossyScale.x * rectTransform.rect.size.x,
            rectTransform.lossyScale.y * rectTransform.rect.size.y);

        return new Rect(position, size);
    }
}