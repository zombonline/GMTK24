using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PositionOnScreen
{
    public static bool IsAbove(Vector2 objectToCheck, float screenPointToCheck)
    {
        return Camera.main.WorldToScreenPoint(objectToCheck).y > screenPointToCheck;
    }
    public static bool IsBetween(Vector2 objectToCheck, float aboveScreenPointToCheck, float belowScreenPointToCheck)
    {
        return IsAbove(objectToCheck, belowScreenPointToCheck) && !IsAbove(objectToCheck, aboveScreenPointToCheck);
    }
    public static float GetScaleBetweenTwoPoints(Vector2 objectToCheck, float pointToCheck0, float pointToCheck1)
    {
        return Mathf.Clamp(Camera.main.WorldToScreenPoint(objectToCheck).y, pointToCheck0, pointToCheck1);
    }
    public static float GetCameraHeightInWorld()
    {
        return Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight)).y;
    }

    public static Vector2 GetMousePositionOnScreen()
    {
        return Input.mousePosition;
    }
}
