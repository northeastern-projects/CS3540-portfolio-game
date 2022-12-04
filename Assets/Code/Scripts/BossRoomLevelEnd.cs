using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomLevelEnd : MonoBehaviour
{
    private bool _isPlayerTouchingDoor = false;
    private Rigidbody2D _player;
    
    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
                //Go To Leaderboard scene here
                UnityEditor.EditorApplication.isPlaying = false;
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
