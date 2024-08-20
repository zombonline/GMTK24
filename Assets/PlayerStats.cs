using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float speedMultiplier = 2, jumpForceMultiplier = 2, buildSpeedMultiplier = 2;
    [SerializeField] private float speedMultiplierMax, jumpForceMultiplierMax, buildSpeedMultiplierMax;
    [SerializeField] TextMeshProUGUI statsTextJump, statsTextBuild, statsTextSpeed;
    public void IncreaseMultiplier(PlayerStatMultiplier playerStatMultiplier, float amount)
    {
        switch (playerStatMultiplier)
        {
            case PlayerStatMultiplier.SPEED:
                speedMultiplier += amount;
                if (speedMultiplier > speedMultiplierMax)
                {
                    speedMultiplier = speedMultiplierMax;
                }
                break;
            case PlayerStatMultiplier.JUMP:
                jumpForceMultiplier += amount;
                if (jumpForceMultiplier > jumpForceMultiplierMax)
                {
                    jumpForceMultiplier = jumpForceMultiplierMax;
                }
                break;
            case PlayerStatMultiplier.BUILD:
                buildSpeedMultiplier += amount;
                if (buildSpeedMultiplier > buildSpeedMultiplierMax)
                {
                    buildSpeedMultiplier = buildSpeedMultiplierMax;
                }
                break;
        }
        if(statsTextJump == null) { return; }
        //statsText.text = "SPEED - " + speedMultiplier + "\nJUMP - " + jumpForceMultiplier + "\nBUILD - " + buildSpeedMultiplier;
        statsTextBuild.text = buildSpeedMultiplier.ToString("0.0").Split('.')[1];
        statsTextJump.text = jumpForceMultiplier.ToString("0.0").Split('.')[1];
        statsTextSpeed.text = speedMultiplier.ToString("0.0").Split('.')[1];
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

}
