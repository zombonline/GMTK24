using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LevelChunk : MonoBehaviour
{
    BoxCollider2D col;
    Vector3[] points;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        points = new Vector3[4]
        {
            col.bounds.min,
            new Vector3(col.bounds.min.x, col.bounds.max.y),
            col.bounds.max,
            new Vector3(col.bounds.max.x, col.bounds.min.y)
        };
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.15f);
        Gizmos.DrawLineStrip(points, true);
    }

    private void Update()
    {
        if(col.bounds.max.y < Camera.main.ScreenToWorldPoint(new Vector3(0,0,0)).y - 10)
        {
            Destroy(gameObject);
        }
    }
}
