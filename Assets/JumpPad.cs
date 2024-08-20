using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] private float power = 10f;
    [SerializeField] Sprite primedSprite, unprimedSprite;
    [SerializeField] float cooldown = .5f;
    private float cooldownTimer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (LevelManager.GetIsPaused()) { return; }
        if (collision.CompareTag("Player") && cooldownTimer <= 0)
        {
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.GetComponent<Rigidbody2D>().AddForce(Vector2.up * power, ForceMode2D.Impulse);
            spriteRenderer.sprite = unprimedSprite;
            cooldownTimer = cooldown;
        }
    }

    private void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else
        {
            spriteRenderer.sprite = primedSprite;
        }
    }
}
