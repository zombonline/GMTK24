using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenController : MonoBehaviour
{
    float valueToUse = 0;
    public void SetValue(float value)
    {
        valueToUse = value;
    }
    public void ScaleObjectX(GameObject item)
    {
        LeanTween.scaleX(item, valueToUse, .25f);
    }
    public static void MoveObject(GameObject targetObject, Vector2 targetPosition)
    {
        LeanTween.move(targetObject, targetPosition, .5f);
    }
    public static void MoveObjectUI(RectTransform targetObject, Vector2 targetPosition)
    {
        LeanTween.move(targetObject, targetPosition, .5f);
    }
}