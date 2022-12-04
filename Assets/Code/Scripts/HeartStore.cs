using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartStore : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Health playerHealth = collision.GetComponent<Health>();
            
            if(!playerHealth.isFullHealth() && PlayerManager.Pay(10))
            {
                
                //make methods in PlayerManager to call for each of these kinda functions later
                playerHealth.Heal(2);
                Destroy(gameObject);
            }
        }
    }
}
