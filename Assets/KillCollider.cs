using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KillCollider : MonoBehaviour
{
    [SerializeField] UnityEvent onGameOver;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            onGameOver.Invoke();
        }
    }
}
