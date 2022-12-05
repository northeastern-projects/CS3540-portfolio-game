using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomLevelEnd : MonoBehaviour
{
    private bool _isPlayerTouchingDoor = false;
    private Rigidbody2D _player;
    private LeaderboardManager leaderManager;
    
    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_isPlayerTouchingDoor && Input.GetKeyDown("w"))
        {
            Debug.Log("NumKeys: " + PlayerManager.numKeys);
            if (PlayerManager.numKeys == 3)
            {
                Debug.Log("Final Level Complete");

                Destroy(GameObject.FindWithTag("Player"));
                LeaderboardManager.CheckBestTime();
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        _isPlayerTouchingDoor = true;
        Debug.Log("touching door");
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        _isPlayerTouchingDoor = false;

    }
}
