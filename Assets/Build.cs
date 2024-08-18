using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Build : MonoBehaviour
{

    BuildableBlock targetBuildableBlock;

    List<BuildableBlock> buildableBlocksInRange = new List<BuildableBlock>();

    [SerializeField] private float buildSpeed = 2;
    private float buildTimer;

    [SerializeField] UnityEvent onBuildBegin, onBuildEnd;

    [SerializeField] Image progressBar;

    public void Start()
    {
        buildTimer = buildSpeed;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(targetBuildableBlock == null) { return; }
            onBuildBegin.Invoke();
            buildTimer = buildSpeed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            if(targetBuildableBlock == null) { return ; }
            buildTimer -= Time.deltaTime;
            if (buildTimer < 0)
            {
                targetBuildableBlock.Build();
                RemoveBlockFromRange(targetBuildableBlock);
                onBuildEnd.Invoke();
            }
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            buildTimer = buildSpeed;
            onBuildEnd.Invoke();
        }
        progressBar.fillAmount = buildTimer/buildSpeed;

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            targetBuildableBlock.ToggleTargetHilight(false);
            int currentTargetIndex = buildableBlocksInRange.IndexOf(targetBuildableBlock);
            int newTargetIndex = currentTargetIndex + 1;
            if(newTargetIndex >= buildableBlocksInRange.Count)
            {
                newTargetIndex = 0;
            }
            targetBuildableBlock = buildableBlocksInRange[newTargetIndex];
            targetBuildableBlock.ToggleTargetHilight(true);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BuildableBlock buildableBlock = collision.GetComponent<BuildableBlock>();
        if (buildableBlock != null)
        {
            AddBlockToRange(buildableBlock);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        BuildableBlock buildableBlock = collision.GetComponent<BuildableBlock>();
        if (buildableBlock != null)
        {
            RemoveBlockFromRange(buildableBlock);
        }
    }
    private void AddBlockToRange(BuildableBlock buildableBlock)
    {
        if (buildableBlock.GetIsBuilt())
        {
            return;
        }
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
}
