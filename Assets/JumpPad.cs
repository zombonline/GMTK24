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
    [SerializeField] string bounceSFX;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameManager.GetGameState() != GameState.PLAYING) { return; }
        if(!collision.CompareTag("Player")) { return; }
        if(cooldownTimer > 0) { return; }
        if(collision.GetComponent<PlayerMovement>().GetIsClimbing()) { return; }
        FMODController.PlaySFX(bounceSFX);
        collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        collision.GetComponent<Rigidbody2D>().AddForce(Vector2.up * power, ForceMode2D.Impulse);
        spriteRenderer.sprite = unprimedSprite;
        cooldownTimer = cooldown;
    }
}
