using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableBlock : MonoBehaviour
{
    private bool isBuilt = false, isInRange = false, isTarget = false;
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite builtSprite, unbuiltSprite;
    Collider2D col;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }
    
    public void Build()
    {
        isBuilt = true;
        GetComponent<SpriteRenderer>().sprite = builtSprite;
        col.isTrigger = false;
    }
    public bool GetIsBuilt()
    {
        return isBuilt;
    }
    public void ToggleTargetHilight(bool toggle)
    {
        isTarget = toggle;
        if(toggle)
        {
            spriteRenderer.color = Color.yellow;
        }
        else
        {
            if (isInRange)
            {
                spriteRenderer.color = Color.green;
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }
    }
    public void ToggleInRangeHilight(bool toggle)
    {
        isInRange = toggle;
        if(toggle)
        {
            spriteRenderer.color = Color.green;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }
}
