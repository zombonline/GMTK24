using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScroll : MonoBehaviour
{
    [SerializeField] private float initialScrollSpeed, scrollSpeedIncreaseAmount, scrollSpeedIncreaseFrequency;
    private float currentScrollSpeed;
    private float scrollSpeedIncreaseTimer;

    private void Start()
    {
        currentScrollSpeed = initialScrollSpeed;
        scrollSpeedIncreaseTimer = scrollSpeedIncreaseFrequency;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, 
            transform.position + Vector3.up, currentScrollSpeed * Time.deltaTime);

        scrollSpeedIncreaseTimer -= Time.deltaTime;
        if(scrollSpeedIncreaseTimer <= 0)
        {
            currentScrollSpeed += scrollSpeedIncreaseAmount;
            scrollSpeedIncreaseTimer = scrollSpeedIncreaseFrequency;
        }
    }
}
