using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector2 teleportDistance = new Vector2(0.0f, 10.0f);

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
        if (_isPlayerTouchingDoor && Input.GetAxis("Vertical") > 0.0f)
        {
            // Enter door (currently teleport up one floor)
            _player.position += teleportDistance;
            _isPlayerTouchingDoor = false;
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
