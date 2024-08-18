using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScroll : MonoBehaviour
{
    [SerializeField] private float initialScrollSpeed, scrollSpeedIncreaseAmount, scrollSpeedIncreaseFrequency;
    private float scrollSpeed;
    private float normalScrollSpeed;
    private float scrollSpeedIncreaseTimer;

    private Transform player;
    [SerializeField] private float playerAheadScrollSpeed;
    private bool playerAhead = false, playerAheadLastFrame;   
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        normalScrollSpeed = initialScrollSpeed;
        scrollSpeed = normalScrollSpeed;
        scrollSpeedIncreaseTimer = scrollSpeedIncreaseFrequency;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, 
            transform.position + Vector3.up, scrollSpeed * Time.deltaTime);

        scrollSpeedIncreaseTimer -= Time.deltaTime;
        if(scrollSpeedIncreaseTimer <= 0)
        {
            normalScrollSpeed += scrollSpeedIncreaseAmount;
            scrollSpeedIncreaseTimer = scrollSpeedIncreaseFrequency;
        }

        playerAhead = (player.position.y > Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight)).y / 2f);
        if(playerAhead && !playerAheadLastFrame)
        {
            if (normalScrollSpeed < playerAheadScrollSpeed) { return; }
            LeanTween.cancelAll();
            LeanTween.value(scrollSpeed, playerAheadScrollSpeed, 3f).setOnUpdate(SetScrollSpeed);
        }
        else if (!playerAhead && playerAheadLastFrame)
        {
            LeanTween.cancelAll();
            LeanTween.value(scrollSpeed, normalScrollSpeed, .25f).setOnUpdate(SetScrollSpeed);
        }

        playerAheadLastFrame = playerAhead;
    }
    public void SetScrollSpeed(float val)
    {
        scrollSpeed = val;
    }
    public float GetCurrentScrollSpeed()
    {
        return scrollSpeed;
    }
}
