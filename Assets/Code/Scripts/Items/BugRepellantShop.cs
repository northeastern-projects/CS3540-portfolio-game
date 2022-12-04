using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugRepellantShop : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if(PlayerManager.Pay(20))
            {
                
                //Get player's statmanager component and increase attack damage
                //StatManager manager = collision.GetComponent<StatManager> 
                //manager.AttackBuff();
                

                Destroy(gameObject);
            }
        }
    }
}
