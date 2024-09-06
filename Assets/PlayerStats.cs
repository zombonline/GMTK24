using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int speedLevel = 0, jumpLevel = 0, buildLevel = 0;
    private float speedMultiplier = 1, jumpForceMultiplier = 1, buildSpeedMultiplier = 1;
    [SerializeField] private int speedLevelMax, jumpLevelMax, buildLevelMax;
    [SerializeField] private float speedMultiplerEffector, jumpForceMultiplierEffector, buildSpeedMultiplierEffector;
    [SerializeField] TextMeshProUGUI statsTextJump, statsTextBuild, statsTextSpeed;

    public delegate void OnLevelIncreased();

    public static event OnLevelIncreased onLevelIncreased;
    public void IncreaseLevel(PlayerStatMultiplier playerStatMultiplier)
    {
        switch (playerStatMultiplier)
        {
            case PlayerStatMultiplier.SPEED:
                speedLevel++;
                if (speedLevel > speedLevelMax)
                {
                    speedLevel = speedLevelMax;
                }
                speedMultiplier = 1f + (((float)speedLevel / speedLevelMax) * speedMultiplerEffector);
                break;
            case PlayerStatMultiplier.JUMP:
                jumpLevel++;
                if (jumpLevel > jumpLevelMax)
                {
                    jumpLevel = jumpLevelMax;
                }
                print("Jump force multiplier: " + jumpForceMultiplier);
                jumpForceMultiplier = 1f + (((float)jumpLevel / jumpLevelMax) * jumpForceMultiplierEffector);
                print("Jump force multiplier: " + jumpForceMultiplier);
                break;
            case PlayerStatMultiplier.BUILD:
                buildLevel++;
                if (buildLevel > buildLevelMax)
                {
                    buildLevel = buildLevelMax;
                }
                buildSpeedMultiplier = 1f + (((float)buildLevel / buildLevelMax) * buildSpeedMultiplierEffector);
                break;
        }
        onLevelIncreased.Invoke();
        if(statsTextJump == null) { return; }
        statsTextSpeed.text = speedLevel.ToString();
        statsTextJump.text = jumpLevel.ToString();
        statsTextBuild.text = buildLevel.ToString();
    } 
    public float GetMultiplier(PlayerStatMultiplier playerStatMultiplier)
    {
        switch (playerStatMultiplier)
        {
            case PlayerStatMultiplier.SPEED:
                return speedMultiplier;
            case PlayerStatMultiplier.JUMP:
                return jumpForceMultiplier;
            case PlayerStatMultiplier.BUILD:
                return buildSpeedMultiplier;
            default:
                return 1;
        }
    }

    public int GetLevel(PlayerStatMultiplier playerStatMultiplier)
    {
        switch (playerStatMultiplier)
        {
            case PlayerStatMultiplier.SPEED:
                return speedLevel;
            case PlayerStatMultiplier.JUMP:
                return jumpLevel;
            case PlayerStatMultiplier.BUILD:
                return buildLevel;
            default:
                return 0;
        }
    }

}
