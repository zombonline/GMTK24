using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStatMultiplier
{
    SPEED,
    JUMP,
    BUILD
}
public enum GameState
{
    MENU,
    PLAYING,
    PAUSED,
    GAME_OVER,
    GAME_COMPLETE,
}
[System.Serializable]
public struct MinStatRequirements
{
    public int speed, jump, build;
}
