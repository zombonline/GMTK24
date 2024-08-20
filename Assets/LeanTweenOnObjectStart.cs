using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenOnObjectStart : MonoBehaviour
{
    [SerializeField] public Vector2 targetPos;
    [SerializeField] public LeanTweenType tweenType;
    [SerializeField] public float duration, delay;
    public void Start()
    {
        Invoke(nameof(Execute), delay);
    }
    public void Execute()
    {
        LeanTween.moveLocal(gameObject, targetPos, duration).setEase(tweenType);
    }
}
