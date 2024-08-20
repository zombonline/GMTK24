using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerStats playerStats;
    Rigidbody2D rb;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f, jumpVelocityFallOff, fallMultiplier, downwardVelocityCap;
    private bool canMove = true;

    [Header("Ground Check")]
    private bool isGrounded = false, isGroundedLastFrame = false;
    [SerializeField] Transform[] groundCheckStartPositions;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] LayerMask groundCheckLayerMask;

    [SerializeField] float jumpPressBuffer;
    private float jumpPressBufferTimer;

    Vector2 storedVelocity;

    private bool isClimbing = false, ladderInRange = false;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if(LevelManager.GetIsPaused()) { return; }
        if (Input.GetKeyDown(KeyCode.W) && ladderInRange && !isClimbing)
        {
            isClimbing = true;
            Physics2D.gravity = Vector2.zero;
        }
        if(isClimbing)
        {
            int vertical = (int)Input.GetAxisRaw("Vertical");
            if(vertical < 0) { vertical = -2; }
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed * playerStats.GetMultiplier(PlayerStatMultiplier.SPEED));
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isClimbing = false;
                rb.gravityScale = 1;
                ExecuteJump();
            }
        }
        if (!ladderInRange)
        {
            isClimbing = false;
            Physics2D.gravity = new Vector2(0, -9.81f);
        }

        if (!canMove) return;
       int horizontal = (int)Input.GetAxisRaw("Horizontal");
        rb.velocity = 
            new Vector2(horizontal * speed * playerStats.GetMultiplier(PlayerStatMultiplier.SPEED), rb.velocity.y);
        
        CheckIsGrounded();

        if (Input.GetButtonDown("Jump"))
        {
            jumpPressBufferTimer = jumpPressBuffer;
        }
        jumpPressBufferTimer -= Time.deltaTime;

        if (jumpPressBufferTimer > 0 && isGrounded && rb.velocity.y <= 0)
        {
            ExecuteJump();
        }
        //jump velocity fall off
        if (!isGrounded && rb.velocity.y < jumpVelocityFallOff * playerStats.GetMultiplier(PlayerStatMultiplier.JUMP))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
        }
        //cap fall speed
        if (rb.velocity.y < downwardVelocityCap)
        {
            rb.velocity = new Vector2(rb.velocity.x, downwardVelocityCap);
        }
    }

    private void ExecuteJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce * playerStats.GetMultiplier(PlayerStatMultiplier.JUMP), ForceMode2D.Impulse);
    }    

    private void CheckIsGrounded()
    {
        isGrounded = false;
        foreach (Transform t in groundCheckStartPositions)
        {
            Physics2D.queriesHitTriggers = false;
            RaycastHit2D rayHit = Physics2D.Raycast(t.position, Vector2.down, groundCheckDistance, groundCheckLayerMask);
            Debug.DrawRay(t.position, Vector2.down * groundCheckDistance, Color.yellow);
            if (rayHit.collider == null) { continue; }
            isGrounded = true;
        }
    }
    public void PauseMovement()
    {
        Debug.Log("Pausing Movement");
        storedVelocity = rb.velocity;
        canMove = false;
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void ContinueMovement()
    {
        canMove = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        if (storedVelocity == Vector2.zero) { return; }
        rb.velocity = storedVelocity;
        storedVelocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder")) 
        {
            ladderInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            ladderInRange = false;
        }
    }

    public bool GetIsClimbing()
    {
        return isClimbing;
    }
}
