using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    [SerializeField] private int numLevels;
    [SerializeField] private GameObject[] levels;
    [SerializeField] private GameObject[] bossLevels;
    private const int LevelHeight = 120;

    // Start is called before the first frame update
    private void Start()
    {
        var offset = 0;
        for (var i = 0; i < numLevels; i++)
        {
            Instantiate(GetRandomLevel(), new Vector3(0, offset), Quaternion.identity);
            offset += LevelHeight;
        }
    }

    private GameObject GetRandomLevel()
    {
        var index = Random.Range(0, levels.Length);
        return levels[index];
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
