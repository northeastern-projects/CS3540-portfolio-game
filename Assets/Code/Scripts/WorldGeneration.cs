using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    [SerializeField] private int maxLevels;
    [SerializeField] private GameObject[] levels;
    [SerializeField] private GameObject[] bossLevels;

    private GameObject _lastLevel;
    private GameObject _currentLevel;
    private GameObject _nextLevel;
    private Rigidbody2D _player;

    private int _numLevels;
    private Vector3 _nextLevelPosition;
    
    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        CreateNewLevel();
    }

    private GameObject GetRandomLevel()
    {
        var index = Random.Range(0, levels.Length);
        Debug.Log($"Created level {index}");
        return levels[index];
    }
    

    public void NextLevel()
    {
        Destroy(_lastLevel);
        _lastLevel = _currentLevel;
        _currentLevel = _nextLevel;
        CreateNewLevel();
        _player.transform.position += new Vector3(0.0f, 10.0f);
    }

    private void CreateNewLevel()
    {
        if (_numLevels >= maxLevels) return;
        _nextLevel = Instantiate(GetRandomLevel(), _nextLevelPosition, Quaternion.identity, transform);
        _numLevels++;
        _nextLevelPosition += new Vector3(0.0f, 120.0f);
    }
}
