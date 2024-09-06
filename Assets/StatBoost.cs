using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBoost : MonoBehaviour
{
    [SerializeField] PlayerStatMultiplier statToUpgrade;
    [SerializeField] float amount;
    [SerializeField] string pickupSFX;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) { return; }
        FindObjectOfType<PlayerStats>().IncreaseLevel(statToUpgrade);
        FMODController.PlaySFX(pickupSFX);
        foreach (StatBoost statBoost in FindObjectsOfType<StatBoost>())
        {
           Destroy(statBoost.gameObject);
        }
    }
}
