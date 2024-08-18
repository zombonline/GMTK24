using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] LevelScroll levelScroll;
    [SerializeField] GameObject[] levelChunks;

    GameObject lastSpawnedChunk;
    private float cameraTop;

    private void Start()
    {
        cameraTop = Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight)).y;
        lastSpawnedChunk = SpawnNewLevelChunk(0);
    }

    private void Update()
    {
        if (lastSpawnedChunk.GetComponent<Collider2D>().bounds.max.y < cameraTop)
        {
            lastSpawnedChunk = SpawnNewLevelChunk(lastSpawnedChunk.GetComponent<Collider2D>().bounds.max.y);
        }
    }
    private GameObject SpawnNewLevelChunk(float yPos)
    {
        GameObject newLevelChunk = 
            Instantiate(levelChunks[Random.Range(0, levelChunks.Length)], new Vector2(0, yPos) , Quaternion.identity);
        newLevelChunk.transform.SetParent(levelScroll.transform);
        return newLevelChunk;
    }
}
