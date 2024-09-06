
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private bool isMoving, isJumping, isFalling, isBuilding, isClimbing;
    Rigidbody2D rb;
    Build build;
    PlayerMovement playerMovement;
    [SerializeField] SpriteRenderer spriteRenderer;
    private bool animating = true;

    private void OnEnable()
    {
        GameManager.onGameStateUpdated += HandleGameState;
    }
    private void OnDisable()
    {
        GameManager.onGameStateUpdated -= HandleGameState;
    }
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        build = GetComponentInChildren<Build>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    void Update()
    {
        if(!animating) { return; }
        animator.speed = 1;
        isMoving = rb.velocity.x != 0;    
        isJumping = rb.velocity.y > 0;
        isFalling = rb.velocity.y < 0;
        isBuilding = build.GetIsBuilding();
        isClimbing = playerMovement.GetIsClimbing();

        if(rb.velocity.x != 0)
        {
            spriteRenderer.flipX = rb.velocity.x < 0;
        }
        if (isBuilding)
        {
            animator.CrossFade("Build", 0f);
        }
        else if (isClimbing)
        {
            if(rb.velocity.y == 0)
            {
                animator.speed = 0;
            }
            animator.CrossFade("Climb", 0f);
        }
        else if(isJumping)
        {
            animator.CrossFade("Jump", 0f);
        }
        else if (isFalling)
        {
            animator.CrossFade("Fall", 0f);
        }
        else if (isMoving)
        {
            animator.CrossFade("Walk", 0f);
        }
        else
        {
            animator.CrossFade("Idle", 0f);
        }
    }
    private void HandleGameState(GameState state)
    {
        switch (state)
        {
            case GameState.PLAYING:
                animating = true;
                break;
            default:
                animator.speed = 0;
                animating = false;
                break;
        }
    }
}