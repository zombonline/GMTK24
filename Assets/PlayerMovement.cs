using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if(!canMove) return;
       int horizontal = (int)Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(horizontal, 0, 0) * Time.deltaTime * speed; 
        CheckIsGrounded();

        if (Input.GetButtonDown("Jump"))
        {
            jumpPressBufferTimer = jumpPressBuffer;
        }
        jumpPressBufferTimer -= Time.deltaTime;

        if (jumpPressBufferTimer > 0 && isGrounded && rb.velocity.y <= 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        //jump velocity fall off
        if (!isGrounded && rb.velocity.y < jumpVelocityFallOff)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
        }
        //cap fall speed
        if (rb.velocity.y < downwardVelocityCap)
        {
            rb.velocity = new Vector2(rb.velocity.x, downwardVelocityCap);
        }
    }

    private void CheckIsGrounded()
    {
        isGrounded = false;
        foreach (Transform t in groundCheckStartPositions)
        {
            RaycastHit2D rayHit = Physics2D.Raycast(t.position, Vector2.down, groundCheckDistance, groundCheckLayerMask);
            Debug.DrawRay(t.position, Vector2.down * groundCheckDistance, Color.yellow);
            if (rayHit.collider != null)
            {
                isGrounded = true;
            }
        }
    }
    public void PauseMovement()
    {
        storedVelocity = rb.velocity;
        canMove = false;
        rb.bodyType = RigidbodyType2D.Static;
        rb.velocity = Vector2.zero; 
    }

    public void ContinueMovement()
    {
        if(storedVelocity == Vector2.zero) { return; }
        canMove = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = storedVelocity;
        storedVelocity = Vector2.zero;
    }
}
