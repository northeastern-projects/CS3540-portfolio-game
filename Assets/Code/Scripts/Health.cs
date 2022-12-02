using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 200;
    
    public void Damage(int amount)
    {
        this.health -= amount;
        StartCoroutine(DamageEffect(Color.red));
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
        //Destroy(this.gameObject);
        SceneManager.LoadScene(3);
    }
    
    private IEnumerator DamageEffect(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
