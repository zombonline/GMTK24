using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerStats playerStats;
    Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f, jumpVelocityFallOff, fallMultiplier, downwardVelocityCap;
    private bool canMove = true;
    [SerializeField] float jumpPressBuffer;
    private float jumpPressBufferTimer;
    Vector2 storedVelocity;

    [Header("Ground Check")]
    [SerializeField] Transform[] groundCheckStartPositions;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] LayerMask groundCheckLayerMask;
    private bool isGrounded = false;

    [Header("Ladder")]
    [SerializeField] string climbingSFX;
    FMOD.Studio.EventInstance climbingSFXInstance;
    [SerializeField] Vector2 ladderDismountHopForce;
    private bool isClimbing = false;
    private Ladder ladderInRange;

    private void OnEnable()
    {
        GameManager.onGameStateUpdated += HandleGameState;
        playerInput.actions.FindAction("Jump").performed += JumpPressed;
    }
    private void OnDisable()
    {
        GameManager.onGameStateUpdated -= HandleGameState;
        playerInput.actions.FindAction("Jump").performed -= JumpPressed;
    }
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerStats = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void Update()
    {
        if (!canMove) { return; }
        HorizontalMovement();
        CheckIsGrounded();
        JumpPhysics();
        LadderMovement();
    }
    private void HorizontalMovement()
    {
        var moveX = playerInput.actions.FindAction("Move").ReadValue<Vector2>().x;
        rb.velocity = new Vector2(moveX * speed * playerStats.GetMultiplier(PlayerStatMultiplier.SPEED), rb.velocity.y);
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
    private void JumpPhysics()
    {
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
    private void JumpPressed(InputAction.CallbackContext context)
    {
        jumpPressBufferTimer = jumpPressBuffer;
    }
    private void ExecuteJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce * playerStats.GetMultiplier(PlayerStatMultiplier.JUMP), ForceMode2D.Impulse);
    }
    private void LadderMovement()
    {
        float moveY = playerInput.actions.FindAction("Move").ReadValue<Vector2>().y;
        if (moveY != 0 && ladderInRange != null && !isClimbing)
        {
            isClimbing = true;
            climbingSFXInstance = FMODController.PlaySFX(climbingSFX);
            Physics2D.gravity = Vector2.zero;
            transform.position = new Vector2(ladderInRange.transform.position.x, transform.position.y);
        }
        else if (isClimbing)
        {
            if (moveY == 0)
            {
                FMODController.StopSFX(climbingSFXInstance);
                rb.velocity = Vector2.zero;
            }
            else
            {
                if(!FMODController.GetIsPlaying(climbingSFXInstance))
                {
                    climbingSFXInstance = FMODController.PlaySFX(climbingSFX);
                }
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(moveY) * speed * playerStats.GetMultiplier(PlayerStatMultiplier.SPEED));
            }
            if (Input.GetKeyDown(KeyCode.Space))
            { 
                isClimbing = false;
                Physics2D.gravity = new Vector2(0, -9.81f);
                FMODController.StopSFX(climbingSFXInstance);
                ExecuteJump();
            }
        }
        if (!ladderInRange && isClimbing)
        {
            isClimbing = false;
            rb.AddForce(ladderDismountHopForce, ForceMode2D.Impulse);
            Physics2D.gravity = new Vector2(0, -9.81f);
            FMODController.StopSFX(climbingSFXInstance);
        }
    }
    public bool GetIsClimbing()
    {
        return isClimbing;
    }
    public void SetLadderInRange(Ladder val)
    {
        ladderInRange = val;
    }
    public void RemoveLadderInRange(Ladder val)
    {
        if (ladderInRange != val) { return; }
        ladderInRange = null;
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
    private void HandleGameState(GameState state)
    {
        switch (state)
        {
            case GameState.PLAYING:
                ContinueMovement();
                break;
            default:
                PauseMovement();
                break;
        }
    }
}
