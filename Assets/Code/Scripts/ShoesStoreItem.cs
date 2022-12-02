using System.Collections;
using System.Collections.Generic;
using Code.Scripts;
using UnityEngine;

public class ShoesStoreItem : MonoBehaviour
{
    
    //playerStatManager
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if(PlayerManager.Pay(20))
            {
                
                //Get player's statmanager component and increase attack damage
                //PlayerMovement speedBoost = collision.GetComponent<PlayerMovement>();
                //speedBoost.addBoost();
                

                Destroy(gameObject);
            }
        }
    }
}
