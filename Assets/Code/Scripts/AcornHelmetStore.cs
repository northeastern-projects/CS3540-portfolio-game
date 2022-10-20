using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornHelmetStore : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if(PlayerManager.numberOfCoins >= 5)
            {
                
                //make methods in PlayerManager to call for each of these kinda functions later
                PlayerManager.numberOfCoins -= 5;
                Destroy(gameObject);
            }
        }
    }
}
