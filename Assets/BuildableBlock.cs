using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class BuildableBlock : MonoBehaviour
{
    [SerializeField] private bool isBuilt;
    private bool isInRange = false, isTarget = false;
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite builtSprite, unbuiltSprite;
    Collider2D col;
    [SerializeField] SpriteRenderer markerSprite;
    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        if (isBuilt) { Build(); }
        else { spriteRenderer.sprite = unbuiltSprite; }
        LeanTween.moveLocalY(markerSprite.gameObject, markerSprite.transform.localPosition.y + 0.25f, .5f).setLoopPingPong().setEase(LeanTweenType.easeInOutSine);
    }

    private void Update()
    {
        {
            if (isBuilt)
            {
                Build();
            }
            else
            {
                Unbuild();
            }
        }
    }
    public void Interact()
    {
        if(isBuilt) { Unbuild(); }
        else { Build(); }
    }
    public void Build()
    {
        isBuilt = true;
        spriteRenderer.sprite = builtSprite;
        col.isTrigger = false;
    }
    public void Unbuild()
    {
        isBuilt = false;
        spriteRenderer.sprite = unbuiltSprite;
        col.isTrigger = true;
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
            markerSprite.color = Color.green;
        }
        else
        {
            markerSprite.color = Color.white;
        }
    }
    public void ToggleInRangeHilight(bool toggle)
    {
        markerSprite.enabled = toggle;
    }
    public Vector2[] GetColliderPoints()
    {
        //return each corner of the collider
        Vector2[] points = new Vector2[4]
        {
            col.bounds.min,
            new Vector2(col.bounds.min.x, col.bounds.max.y),
            col.bounds.max,
            new Vector2(col.bounds.max.x, col.bounds.min.y)
        };
        return points;
    }
}
