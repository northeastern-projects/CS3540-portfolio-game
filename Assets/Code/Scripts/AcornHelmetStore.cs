using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornHelmetStore : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Health playerHealth = collision.GetComponent<Health>();
            if(PlayerManager.Pay(10))
            {
                
                //Make Helmet can heal more than maximum health but ony for 1 hp
                playerHealth.OverHeal(1);
                Destroy(gameObject);
            }
        }
    }
}
