using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private float spawnChance;
    [SerializeField] private GameObject[] items;
    
    // Start is called before the first frame update
    private void Start()
    {
        if (Random.Range(0.0f, 1.0f) <= spawnChance)
        {
            Instantiate(RandomItem(), transform);
        }
    }

    private GameObject RandomItem()
    {
        return items[Random.Range(0, items.Length)];
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
