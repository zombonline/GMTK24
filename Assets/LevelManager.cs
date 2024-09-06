using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [Header("Level Chunks")]
    [SerializeField] List<LevelChunk> levelChunks;
    List<LevelChunk> possibleChunks = new List<LevelChunk>();
    [SerializeField] LevelChunk statBoostLevelChunk;
    [SerializeField] LevelChunk startLevelChunk;
    [SerializeField] LevelChunk winLevelChunk;
    LevelChunk lastSpawnedChunk;
    bool winChunkSpawned = false;

    [Header("Distance parameters")]
    [SerializeField] private float distanceToSpawnWin;
    [SerializeField] private float distanceToSpawnStatBoost;
    [SerializeField] private float distanceToIncreaseSpeed;
    private float distanceOverall, distanceSinceSpawnStatBoost, distanceSinceIncreaseSpeed;

    private float cameraTop;

    [SerializeField] UnityEvent onGameOver, onGameComplete;

    [SerializeField] TextMeshProUGUI distanceTravelledText;

    Transform levelScrollTransform;

    [Header("Screen points")]
    [SerializeField] float deleteChunkHeight;

    [Header("Scroll Speed")]
    [SerializeField] private float initialScrollSpeed;
    [SerializeField] private float topScreenScrollMultiplier, bottomScreenScrollMultiplier, standardSpeedIncrease;
    private float currentScrollSpeed, standardScrollSpeed;
    LTDescr scrollSpeedTween;

    Transform player;
    bool playerAhead, playerAheadLastFrame, playerBehind, playerBehindLastFrame;

    [SerializeField] Transform[] objectsToMoveWithLevel;

    bool isScrolling = true;


    
    private void OnEnable()
    {
        GameManager.onGameStateUpdated += HandleGameStateChange;
        PlayerStats.onLevelIncreased += UpdatePossibleChunks;
    }
    private void OnDisable()
    {
        GameManager.onGameStateUpdated -= HandleGameStateChange;
        PlayerStats.onLevelIncreased -= UpdatePossibleChunks;
    }
    private void Start()
    {
        CreateLevelScrollTransform();
        UpdatePossibleChunks();
        standardScrollSpeed = initialScrollSpeed;
        currentScrollSpeed = standardScrollSpeed;
        cameraTop = PositionOnScreen.GetCameraHeightInWorld();

        player = GameObject.FindWithTag("Player").transform;
    }
    private void Update()
    {
        HandleScrollSpeed();
        TrackDistance();
        ScrollLevel();
        HandleNextChunk();
    }
    private void CreateLevelScrollTransform()
    {
        levelScrollTransform = new GameObject().transform;
        levelScrollTransform.name = "LevelScroll";
        levelScrollTransform.parent = this.transform;
        foreach (Transform item in objectsToMoveWithLevel)
        {
            item.parent = levelScrollTransform;
        }
    }
    private void HandleNextChunk()
    {
        if (winChunkSpawned) { return; }
        if (lastSpawnedChunk == null)
        {
            lastSpawnedChunk = SpawnNewLevelChunk(startLevelChunk, 0);
        }
        if (lastSpawnedChunk.GetPosOfTop() < cameraTop)
        {
            if (distanceOverall >= distanceToSpawnWin)
            {
                lastSpawnedChunk = SpawnNewLevelChunk(winLevelChunk, lastSpawnedChunk.GetComponent<Collider2D>().bounds.max.y);
                winChunkSpawned = true;
                return;
            }
            if (distanceSinceSpawnStatBoost >= distanceToSpawnStatBoost)
            {
                distanceSinceSpawnStatBoost = 0;
                lastSpawnedChunk = SpawnNewLevelChunk(statBoostLevelChunk, lastSpawnedChunk.GetComponent<Collider2D>().bounds.max.y);
            }
            else
            {
                lastSpawnedChunk = SpawnNewLevelChunk(possibleChunks[Random.Range(0, possibleChunks.Count)], lastSpawnedChunk.GetComponent<Collider2D>().bounds.max.y);
            }
        }
    }
    private void UpdatePossibleChunks()
    {
        var stats = FindObjectOfType<PlayerStats>();
        foreach (LevelChunk chunk in levelChunks)
        {
            if (possibleChunks.Contains(chunk)) { continue; }
            var minStatRequirements = chunk.GetMinStatRequirements();
            if (minStatRequirements.build + minStatRequirements.speed + minStatRequirements.jump == 0)
            {
                possibleChunks.Add(chunk);
                continue;
            }
            int requirementsMet = 0;
            if (minStatRequirements.build <= stats.GetLevel(PlayerStatMultiplier.BUILD))
            {
                if(minStatRequirements.build > 0)
                {
                    requirementsMet++;
                }
            }
            if (minStatRequirements.jump <= stats.GetLevel(PlayerStatMultiplier.JUMP))
            {
                if(minStatRequirements.jump > 0)
                {
                    requirementsMet++;
                }
            }
            if (minStatRequirements.speed <= stats.GetLevel(PlayerStatMultiplier.SPEED))
            {
                if (minStatRequirements.speed > 0)
                {
                    requirementsMet++;
                }
            }
            print(chunk.gameObject.name + " " + requirementsMet);
            if (requirementsMet == 0)
            {
                continue;
            }
            else if(requirementsMet != 3 && chunk.GetRequireAllStats())
            {
                continue;
            }
            possibleChunks.Add(chunk);
        }
    }
    private void TrackDistance()
    {
        if(!isScrolling) { return; }
        distanceOverall += currentScrollSpeed * Time.deltaTime;
        distanceSinceSpawnStatBoost += currentScrollSpeed * Time.deltaTime;
        distanceSinceIncreaseSpeed += currentScrollSpeed * Time.deltaTime;
    }
    private void ScrollLevel()
    {
        if(winChunkSpawned)
        {
            if (lastSpawnedChunk.GetComponent<Collider2D>().bounds.max.y < cameraTop)
            {
                isScrolling = false;
            }
        }
        if(!isScrolling) { return; }    
        //scroll level
        levelScrollTransform.position = Vector3.MoveTowards(levelScrollTransform.position,
        levelScrollTransform.position + Vector3.down, currentScrollSpeed * Time.deltaTime);
        //increase standard scroll speed over time.
        if(distanceSinceIncreaseSpeed >= distanceToIncreaseSpeed)
        {
            standardScrollSpeed += standardSpeedIncrease;
            distanceSinceIncreaseSpeed = 0;
        }
        //destroy unused chunks
        foreach (LevelChunk chunk in levelScrollTransform.GetComponentsInChildren<LevelChunk>())
        {
            if(chunk.GetPosOfTop() <= Camera.main.WorldToScreenPoint(new Vector2(0,deleteChunkHeight)).y)
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
        playerBehind = !PositionOnScreen.IsAbove(player.position, Camera.main.pixelHeight / 4f);
        
        if(playerAhead)
        {
            if(playerAheadLastFrame) { return; }
            if (scrollSpeedTween != null) { LeanTween.cancel(scrollSpeedTween.id); }
            scrollSpeedTween = LeanTween.value(currentScrollSpeed, standardScrollSpeed * topScreenScrollMultiplier, 3f).setOnUpdate(SetScrollSpeed);
        }
        else if (playerBehind)
        {
            float intensity = PositionOnScreen.GetScaleBetweenTwoPoints(player.position, Camera.main.pixelHeight / 4, 0f);
            FindObjectOfType<FMODController>().ChangeDangerParameter(intensity);
            //dangerImage.color = new Color(dangerImage.color.r, dangerImage.color.g, dangerImage.color.g, playerLowScale);
            if (playerBehindLastFrame) { return; }
            if (scrollSpeedTween != null) { LeanTween.cancel(scrollSpeedTween.id); }
            scrollSpeedTween = LeanTween.value(currentScrollSpeed, standardScrollSpeed * bottomScreenScrollMultiplier, 1f).setOnUpdate(SetScrollSpeed);
        }
        else
        {
            if(playerBehindLastFrame)
            {
                FindObjectOfType<FMODController>().ChangeDangerParameter(0);
            }
            if (!playerAheadLastFrame && !playerBehindLastFrame) { return; }
            if (scrollSpeedTween != null) { LeanTween.cancel(scrollSpeedTween.id); }
            scrollSpeedTween = LeanTween.value(currentScrollSpeed, standardScrollSpeed, .25f).setOnUpdate(SetScrollSpeed);
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
    public void GameOver()
    {
        if (PlayerPrefs.GetFloat("HIGH SCORE", 0) < distanceOverall)
        {
            PlayerPrefs.SetFloat("HIGH SCORE", distanceOverall);
        }
        onGameOver.Invoke();
    }
    public void GameComplete()
    {
        if (PlayerPrefs.GetFloat("HIGH SCORE", 0) < distanceOverall)
        {
            PlayerPrefs.SetFloat("HIGH SCORE", distanceOverall);
        }
        onGameComplete.Invoke();
    }
    public void SetScrollSpeed(float val)
    {
        currentScrollSpeed = val;
    }
    private void HandleGameStateChange(GameState newState)
    {
        switch (newState)
        {
            case GameState.PLAYING:
                isScrolling = true;
                break;
            case GameState.PAUSED:
                isScrolling = false;
                break;
            case GameState.GAME_OVER:
                GameOver();
                break;
            case GameState.GAME_COMPLETE:
                GameComplete();
                break;
        }
    }
}
