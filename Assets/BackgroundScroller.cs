using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] float scrollSpeed;
    Material mat;
    Vector2 offSet;
    bool isScrolling = true;
    private void OnEnable()
    {
        GameManager.onGameStateUpdated += HandleGameState;
    }
    private void OnDisable()
    {
        GameManager.onGameStateUpdated -= HandleGameState;
    }
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        offSet = new Vector2(0f, scrollSpeed);
    }
    void Update()
    {
        if (!isScrolling) { return; }
        offSet = new Vector2(0f, scrollSpeed);
        mat.mainTextureOffset += offSet * Time.deltaTime;
    }
    private void HandleGameState(GameState state)
    {
        switch (state)
        {
            case GameState.PLAYING:
                isScrolling = true;
                break;
            default:
                isScrolling = false;
                break;
        }
    }
}