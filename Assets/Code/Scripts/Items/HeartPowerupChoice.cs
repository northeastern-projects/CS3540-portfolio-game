using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPowerupChoice : MonoBehaviour
{
    public GameObject[] otherItems;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health playerHealth = collision.GetComponent<Health>();
        
        if (collision.transform.tag == "Player" && !playerHealth.isFullHealth())
        {
            //When this item is picked, print and destroy other objects
            Debug.Log("Chose Heart");
            
            playerHealth.Heal(2);
            for (int i = 0; i < otherItems.Length; i++)
            {
                Destroy(otherItems[i]);
            }
            Destroy(gameObject);
            
            
        }
    }}
