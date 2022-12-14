using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetPowerupChoice : MonoBehaviour
{

    public GameObject[] otherItems;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Health playerHealth = collision.GetComponent<Health>();
            playerHealth.OverHeal(1);
            
            //When this item is picked, print and destroy other objects
            Debug.Log("Chose Helmet");
            for (int i = 0; i < otherItems.Length; i++)
            {
                Destroy(otherItems[i]);
            }
            
            Destroy(gameObject);
        }
    }
    

}
