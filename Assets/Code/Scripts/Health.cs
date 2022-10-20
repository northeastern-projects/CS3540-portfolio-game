using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 200;
    
    public void Damage(int amount)
    {
        this.health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public void SetHealth(int health)
    {
        this.health = health;
    }

    private void Die()
    {
        Debug.Log("Dead!");
        Destroy(this.gameObject);
    }
}
