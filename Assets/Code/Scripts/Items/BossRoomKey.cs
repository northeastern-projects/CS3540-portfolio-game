using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomKey : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            PlayerManager.numKeys++;
            Debug.Log("Added 1 key, new total num keys:" + PlayerManager.numKeys);
            Destroy(gameObject);
        }
    }
}
