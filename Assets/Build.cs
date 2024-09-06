using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Build : MonoBehaviour
{
    PlayerStats playerStats;
    BuildableBlock targetBuildableBlock;

    List<BuildableBlock> buildableBlocksInRange = new List<BuildableBlock>();

    [SerializeField] private float buildSpeed = 2;
    private float buildTimer;

    [SerializeField] UnityEvent onBuildBegin, onBuildEnd;

    [SerializeField] Image progressBar;

    private bool isBuilding = false;

    [SerializeField] private float yBlockSearchOffset = 0;
    [SerializeField] string buildSFX;

    private bool canBuild = true;

    PlayerInput playerInput;

    FMOD.Studio.EventInstance buildSFXInstance;

    private void OnEnable()
    {
        GameManager.onGameStateUpdated += HandleGameState;
        playerInput.actions.FindAction("Interact").started += BuildPressed;
        playerInput.actions.FindAction("Interact").canceled += BuildReleased;
    }
    private void OnDisable()
    {
        GameManager.onGameStateUpdated -= HandleGameState;
        playerInput.actions.FindAction("Interact").started -= BuildPressed;
        playerInput.actions.FindAction("Interact").canceled -= BuildReleased;
    }
    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        playerStats = GetComponentInParent<PlayerStats>();
    }

    public void Start()
    {
        buildTimer = buildSpeed;
    }

    public void Update()
    {
        if(!canBuild) { return; }

        if (isBuilding)
        {
            if (targetBuildableBlock == null) { return ; }
            buildTimer -= Time.deltaTime;
            if (buildTimer < 0 && isBuilding)
            {
                isBuilding = false;
                targetBuildableBlock.Interact();
                onBuildEnd.Invoke();
            }
        }
        progressBar.fillAmount = buildTimer/(buildSpeed/playerStats.GetMultiplier(PlayerStatMultiplier.BUILD));

        BuildableBlock closestBuildableBlock = null;
        float distFromPlayer = Mathf.Infinity;
        //shoot out raycats in all directions
        for (int i = 0; i < 36; i++)
        {
            float angle = i * 10;
            Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
            Vector2 searchOrigin = new Vector2(transform.position.x,
                transform.position.y + yBlockSearchOffset);
            Physics2D.queriesHitTriggers = true;
            RaycastHit2D hit = Physics2D.Raycast(searchOrigin, dir, 100, LayerMask.GetMask("Buildable"));
            Debug.DrawRay(searchOrigin, dir * 100, Color.red);
            if (hit.collider != null)
            {
                BuildableBlock buildableBlock = hit.collider.GetComponent<BuildableBlock>();
                if (buildableBlock != null)
                {
                    float dist = Vector2.Distance(searchOrigin, hit.point);
                    if (dist < distFromPlayer)
                    {
                        distFromPlayer = dist;
                        closestBuildableBlock = buildableBlock;
                    }
                }
            }
        }
        if (closestBuildableBlock != null)
        {
            if(!buildableBlocksInRange.Contains(closestBuildableBlock)) { return; }
            SetTargetBuildableBlock(closestBuildableBlock);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int currentTargetIndex = buildableBlocksInRange.IndexOf(targetBuildableBlock);
            int newTargetIndex = currentTargetIndex + 1;
            if(newTargetIndex >= buildableBlocksInRange.Count)
            {
                newTargetIndex = 0;
            }
            SetTargetBuildableBlock(buildableBlocksInRange[newTargetIndex]);
        }
    }
    private void BuildPressed(InputAction.CallbackContext context)
    {
        if (targetBuildableBlock == null) { return; }
        onBuildBegin.Invoke();
        buildTimer = buildSpeed / playerStats.GetMultiplier(PlayerStatMultiplier.BUILD);
        isBuilding = true;
        buildSFXInstance = FMODController.PlaySFX(buildSFX);
    }
    private void BuildReleased(InputAction.CallbackContext context)
    {
        isBuilding = false;
        buildTimer = buildSpeed;
        onBuildEnd.Invoke();
        FMODController.StopSFX(buildSFXInstance);
    }

    private void SetTargetBuildableBlock(BuildableBlock newTarget)
    {
        if(targetBuildableBlock != null)
        {
            targetBuildableBlock.ToggleTargetHilight(false);
        }
        targetBuildableBlock = newTarget;
        targetBuildableBlock.ToggleTargetHilight(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BuildableBlock buildableBlock = collision.GetComponent<BuildableBlock>();
        if (buildableBlock != null)
        {
            Debug.Log(buildableBlock.name + " entered player range.");
            AddBlockToRange(buildableBlock);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        BuildableBlock buildableBlock = collision.GetComponent<BuildableBlock>();
        if (buildableBlock != null)
        {
            Debug.Log(buildableBlock.name + " left player range.");
            RemoveBlockFromRange(buildableBlock);
        }
    }
    private void AddBlockToRange(BuildableBlock buildableBlock)
    {
        
        buildableBlock.ToggleInRangeHilight(true);
        buildableBlocksInRange.Add(buildableBlock);
        if (targetBuildableBlock == null)
        {
            targetBuildableBlock = buildableBlock;
            targetBuildableBlock.ToggleTargetHilight(true);
        }
    }
    private void RemoveBlockFromRange(BuildableBlock buildableBlock)
    {
        buildableBlock.ToggleInRangeHilight(false);
        buildableBlocksInRange.Remove(buildableBlock);
        if (targetBuildableBlock == buildableBlock)
        {
            targetBuildableBlock.ToggleTargetHilight(false);
            if (buildableBlocksInRange.Count > 0)
            {
                targetBuildableBlock = buildableBlocksInRange[0];
                targetBuildableBlock.ToggleTargetHilight(true);
            }
            else
            {
                targetBuildableBlock = null;
            }
        }
    }

    public bool GetIsBuilding()
    {
        return isBuilding;
    }
    private void HandleGameState(GameState state)
    {
        switch (state)
        {
            case GameState.PLAYING:
                canBuild = true;
                break;
            default:
                canBuild = false;
                break;
        }
    }
}
