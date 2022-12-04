using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    
    public void Damage(int amount)
    {
        this.health -= amount;
        Debug.Log("took damage: " + amount);
        StartCoroutine(DamageEffect(Color.red));
        if (health <= 0)
        {
            Die();
        }
    }
    
    public void Heal(int amount)
    {
        this.health += amount;
        Debug.Log("healed: " + amount);
        StartCoroutine(DamageEffect(Color.green));
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
    
    public void OverHeal(int amount)
    {
        this.health += amount;
        Debug.Log("Overhealed: " + amount);
        StartCoroutine(DamageEffect(Color.green));
    }

    public void SetHealth(int health)
    {
        this.health = health;
    }
    
    public int GetHealth()
    {
        return health;
    }

    public bool isFullHealth()
    {
        return health == maxHealth;
    }

    private void Die()
    {
        Debug.Log("Dead!");
        Destroy(this.gameObject);
    }
    
    private IEnumerator DamageEffect(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
