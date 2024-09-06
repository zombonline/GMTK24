using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) { return; }

        GameManager.SetGameState(GameState.GAME_COMPLETE);
        FindObjectOfType<LevelManager>().GameComplete();
    }
}
