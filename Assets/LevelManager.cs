using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [SerializeField] LevelChunk[] levelChunks;
    [SerializeField] LevelChunk statBoostLevelChunk;
    [SerializeField] LevelChunk startLevelChunk;
    [SerializeField] LevelChunk winLevelChunk;
    [SerializeField] private float statBoostCooldownInMeters = 10, totalMetersToWin;
    private float metressSinceStatBoost = 0;
    private float totalMetersTravelled = 0;
    private static bool isPaused = false, isGameOver;

    LevelChunk lastSpawnedChunk;
    private float cameraTop;

    [SerializeField] UnityEvent onPause, onGameOver, onContinue, onGameComplete;

    private bool winSpawned = false;

    [SerializeField] TextMeshProUGUI distanceTravelledText;

    Transform levelScrollTransform;

    [Header("Screen points")]
    [SerializeField] float deleteChunkHeight;


    [Header("Scroll Speed")]
    [SerializeField] private float aheadScrollSpeed, behindScrollSpeed, initialScrollSpeed;
    private float currentScrollSpeed, standardScrollSpeed;
    private float scrollSpeedIncreaseTimer;
    LTDescr scrollSpeedTween;

    Transform player;
    bool playerAhead, playerAheadLastFrame, playerBehind, playerBehindLastFrame;

    [SerializeField] Transform[] objectsToMoveWithLevel;

    bool isScrolling = true;

    private void OnEnable()
    {
        GameManager.onGameStateUpdated += HandleGameStateChange;
    }
    private void onDisable()
    {
        GameManager.onGameStateUpdated -= HandleGameStateChange;
    }
    private void HandleGameStateChange(GameState newState)
    {
        switch(newState)
        {
            case GameState.PLAYING
        }
    }

    private void Awake()
    {
        levelScrollTransform = new GameObject().transform;
        levelScrollTransform.parent = this.transform;
        foreach(Transform item in objectsToMoveWithLevel)
        {
            item.parent = levelScrollTransform;
        }
    }

    private void Start()
    {
        standardScrollSpeed = initialScrollSpeed;
        currentScrollSpeed = standardScrollSpeed;
        cameraTop = PositionOnScreen.GetCameraHeightInWorld();
        lastSpawnedChunk = SpawnNewLevelChunk(startLevelChunk, 0);
        ContinueGame();

        player = GameObject.FindWithTag("Player").transform;

        GameManager.SetGameState(GameState.PAUSED);
        GameManager.SetGameState(GameState.PLAYING);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isGameOver) { return; }
            if(!isPaused) 
            { 
                FindObjectOfType<FMODController>().PlayPauseSnaphot();

                PauseGame();
            }
            else 
            {
                FindObjectOfType<FMODController>().StopPauseSnapshot();
                ContinueGame(); 
            }
        }
        ScrollLevel();
        HandleNextChunk();
        if(winSpawned)
        {
            if(lastSpawnedChunk.GetComponent<Collider2D>().bounds.max.y < cameraTop)
            {
                isScrolling = false;
            }
        }
        if (isPaused || winSpawned) { return; }
        totalMetersTravelled += currentScrollSpeed * Time.deltaTime;
        metressSinceStatBoost += currentScrollSpeed * Time.deltaTime;
        distanceTravelledText.text = "Distance Scaled: " + totalMetersTravelled.ToString("0.00") + "\nHighest: " + PlayerPrefs.GetFloat("HIGH SCORE", 0).ToString("0.00");
    }

    private void HandleNextChunk()
    {
        if (lastSpawnedChunk.GetPosoOfTop() < cameraTop)
        {
            if (totalMetersTravelled >= totalMetersToWin)
            {
                winSpawned = true;
                lastSpawnedChunk = SpawnNewLevelChunk(winLevelChunk, lastSpawnedChunk.GetComponent<Collider2D>().bounds.max.y);
                return;
            }
            if (metressSinceStatBoost >= statBoostCooldownInMeters)
            {
                metressSinceStatBoost = 0;
                lastSpawnedChunk = SpawnNewLevelChunk(statBoostLevelChunk, lastSpawnedChunk.GetComponent<Collider2D>().bounds.max.y);
            }
            else
            {
                lastSpawnedChunk = SpawnNewLevelChunk(levelChunks[Random.Range(0, levelChunks.Length)], lastSpawnedChunk.GetComponent<Collider2D>().bounds.max.y);
            }
        }
    }

    private void ScrollLevel()
    {
        levelScrollTransform.position = Vector3.MoveTowards(levelScrollTransform.position,
        levelScrollTransform.position + Vector3.down, currentScrollSpeed * Time.deltaTime);
        foreach (LevelChunk chunk in levelScrollTransform.GetComponentsInChildren<LevelChunk>())
        {

            if(chunk.GetPosoOfTop() <= Camera.main.WorldToScreenPoint(new Vector2(0,deleteChunkHeight)).y)
            {
                Debug.Log("Destroying " + chunk.gameObject.name);
                Destroy(chunk.gameObject,1f);
            }
        }
    }
    private void HandleScrollSpeed()
    {
        //player is high up on the screen
        playerAhead = PositionOnScreen.IsAbove(player.position, Camera.main.pixelHeight / 4f * 3f);
        playerBehind = PositionOnScreen.IsAbove(player.position, Camera.main.pixelHeight / 4f);
        
        if(playerAhead && !playerAheadLastFrame)
        {
            if (scrollSpeedTween != null) { LeanTween.cancel(scrollSpeedTween.id); }
            scrollSpeedTween = LeanTween.value(currentScrollSpeed, aheadScrollSpeed, 3f).setOnUpdate(SetScrollSpeed);
        }
        else if (!playerAhead && playerAheadLastFrame)
        {
            if (scrollSpeedTween != null) { LeanTween.cancel(scrollSpeedTween.id); }
            scrollSpeedTween = LeanTween.value(currentScrollSpeed, standardScrollSpeed, .25f).setOnUpdate(SetScrollSpeed);
        }
        
        //player is low down the screen
        if (playerBehind)
        {
            if(!playerBehindLastFrame)
            {
                if (scrollSpeedTween != null) { LeanTween.cancel(scrollSpeedTween.id); }
                scrollSpeedTween = LeanTween.value(currentScrollSpeed, behindScrollSpeed, 3f).setOnUpdate(SetScrollSpeed);
            }
            float intensity = PositionOnScreen.GetScaleBetweenTwoPoints(player.position, Camera.main.pixelHeight / 4, 0f);
            FindObjectOfType<FMODController>().ChangeDangerParameter(intensity);
            //dangerImage.color = new Color(dangerImage.color.r, dangerImage.color.g, dangerImage.color.g, playerLowScale);
        }
        playerAheadLastFrame = playerAhead;
        playerBehindLastFrame = playerBehind;
    }
    private LevelChunk SpawnNewLevelChunk(LevelChunk levelChunkPrefab, float yPos)
    {
        LevelChunk newLevelChunk = 
            Instantiate(levelChunkPrefab.gameObject, new Vector2(0, yPos) , Quaternion.identity).GetComponent<LevelChunk>();
        newLevelChunk.transform.SetParent(levelScrollTransform.transform);
        return newLevelChunk;
    }

    public static bool GetIsPaused()
    {
        return isPaused;
    }
    public static bool GetIsGameOver()
    {
        return isGameOver;
    }
    public void GameOver()
    {
        if(PlayerPrefs.GetFloat("HIGH SCORE", 0) < totalMetersTravelled)
        {
            PlayerPrefs.SetFloat("HIGH SCORE", totalMetersTravelled);
        }
        isGameOver = true;
        isPaused = true;
        onGameOver.Invoke();
    }
    public void PauseGame()
    {
        isPaused = true;
        onPause.Invoke();
    }
    public void ContinueGame()
    {
        isPaused = false;
        onContinue.Invoke();
    }
    public void GameComplete()
    {
        if (PlayerPrefs.GetFloat("HIGH SCORE", 0) < totalMetersTravelled)
        {
            PlayerPrefs.SetFloat("HIGH SCORE", totalMetersTravelled);
        }
        isGameOver = true;
        isPaused = true;
        onGameComplete.Invoke();
    }
    public void SetScrollSpeed(float val)
    {
        currentScrollSpeed = val;
    }

}
