using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartStore : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if(!PlayerManager.isFullHealth() && PlayerManager.numberOfCoins >= 2)
            {
                
                //make methods in PlayerManager to call for each of these kinda functions later
                PlayerManager.numberOfHearts++;
                PlayerManager.numberOfCoins -= 2;
                Destroy(gameObject);
            }
        }
    }
}
