using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SlicedSpriteCollider : MonoBehaviour
{
    BoxCollider2D col;
    [SerializeField] Vector2 colliderSizeMultiplier = Vector2.one;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        col.size = GetComponent<SpriteRenderer>().size * colliderSizeMultiplier;
    }
}
