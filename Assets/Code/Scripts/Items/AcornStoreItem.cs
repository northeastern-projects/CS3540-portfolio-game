using System.Collections;
using System.Collections.Generic;
using Code.Scripts;
using UnityEngine;

public class AcornStoreItem : MonoBehaviour
{
    [SerializeField] private int cost;
    [SerializeField] private PlayerModifier itemBuffs;
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private int coins;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.transform.CompareTag("Player")) return;
        if (!PlayerManager.PayAcorns(cost)) return;
        PlayerManager.numberOfCoins += coins;

        var playerHealth = collision.GetComponent<Health>();

        //Get player's statmanager component and increase attack damage
        var modifier = collision.GetComponent<PlayerMovement>().movementModifier;
        if (itemBuffs != null)
        {
            modifier.AddModifier(itemBuffs);
        }

        playerHealth.AddMaxHealth(maxHealth);
        playerHealth.Heal(health);
        Destroy(gameObject);
    }
}