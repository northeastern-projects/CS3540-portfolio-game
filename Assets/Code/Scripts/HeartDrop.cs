using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartDrop : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if(!PlayerManager.isFullHealth())
            {
                PlayerManager.numberOfHearts++;
                Destroy(gameObject);
            }
        }
    }
}
