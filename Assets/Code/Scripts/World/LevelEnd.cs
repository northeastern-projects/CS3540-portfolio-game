using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    private WorldGeneration _worldGeneration;
    private bool _isPlayerTouchingDoor;

    private void Start()
    {
        _worldGeneration = FindObjectOfType<WorldGeneration>();
    }
    
    private void Update()
    {
        if (_isPlayerTouchingDoor && Input.GetKeyDown("w"))
        {
            _worldGeneration.NextLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        _isPlayerTouchingDoor = true;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        _isPlayerTouchingDoor = false;
    }
}
