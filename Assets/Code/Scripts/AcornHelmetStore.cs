using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornHelmetStore : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if(PlayerManager.Pay(5))
            {
                
                //Make Helmet do something here

                Destroy(gameObject);
            }
        }
    }
}
