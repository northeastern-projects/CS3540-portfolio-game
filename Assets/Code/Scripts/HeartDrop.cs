using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartDrop : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Health playerHealth = collision.GetComponent<Health>();
            
            Debug.Log("Collision detected");
            Debug.Log(playerHealth.GetHealth());
            if(!playerHealth.isFullHealth())
            {
                Debug.Log("Health before touch: "+ playerHealth.GetHealth());
                playerHealth.Heal(1);
                Debug.Log("Health after touch: "+ playerHealth.GetHealth());
                Debug.Log("Dissapeat");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Player is full health");
            }
        }
    }
}
