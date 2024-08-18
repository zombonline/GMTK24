using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBoost : MonoBehaviour
{
    [SerializeField] PlayerStatMultiplier statToUpgrade;
    [SerializeField] float amount;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<PlayerStats>().IncreaseMultiplier(statToUpgrade, amount);
            StatBoost[] statBoosts = FindObjectsOfType<StatBoost>();
            foreach (StatBoost statBoost in statBoosts)
            {
                Destroy(statBoost.gameObject);
            }
        }
    }
}
