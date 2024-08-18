using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] LevelScroll levelScroll;
    [SerializeField] LevelChunk[] levelChunks;
    [SerializeField] LevelChunk statBoostLevelChunk;
    [SerializeField] LevelChunk startLevelChunk;
    [SerializeField] private float statBoostChunkCooldown = 45f;
    private float statBoostChunkCooldownTimer;


    GameObject lastSpawnedChunk;
    private float cameraTop;

    private void Start()
    {
        statBoostChunkCooldownTimer = statBoostChunkCooldown;
        cameraTop = Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight)).y;
        lastSpawnedChunk = SpawnNewLevelChunk(startLevelChunk, 0);
    }

    private void Update()
    {
        if (statBoostChunkCooldownTimer > 0)
        {
            statBoostChunkCooldownTimer -= Time.deltaTime;
        }
        else
        {
            lastSpawnedChunk = SpawnNewLevelChunk(statBoostLevelChunk, lastSpawnedChunk.GetComponent<Collider2D>().bounds.max.y);
            statBoostChunkCooldownTimer = statBoostChunkCooldown;
        }
        if (lastSpawnedChunk.GetComponent<Collider2D>().bounds.max.y < cameraTop)
        {
            lastSpawnedChunk = SpawnNewLevelChunk(levelChunks[Random.Range(0, levelChunks.Length)], lastSpawnedChunk.GetComponent<Collider2D>().bounds.max.y);
        }
    }
    private GameObject SpawnNewLevelChunk(LevelChunk levelChunk, float yPos)
    {
        GameObject newLevelChunk = 
            Instantiate(levelChunk.gameObject, new Vector2(0, yPos) , Quaternion.identity);
        newLevelChunk.transform.SetParent(levelScroll.transform);
        return newLevelChunk;
    }
}
