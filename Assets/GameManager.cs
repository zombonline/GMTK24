using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    private static GameState gameState;

    public delegate void OnGameStateUpdated(GameState newState);

    public static event OnGameStateUpdated onGameStateUpdated;

    public static void SetGameState(GameState newState)
    {
        if(gameState != newState)
        {    
            onGameStateUpdated?.Invoke(newState);
        }
        gameState = newState;
    }
    
    public static GameState GetGameState()
    {
        return gameState;
    }
}
