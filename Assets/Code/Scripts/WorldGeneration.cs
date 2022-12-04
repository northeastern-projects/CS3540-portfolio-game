using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    [SerializeField] private int maxLevels;
    [SerializeField] private GameObject[] levels;
    [SerializeField] private AudioClip[] levelTracks;
    [SerializeField] private GameObject[] bossLevels;
    [SerializeField] private AudioSource musicPlayer;

    private GameObject _lastLevel;
    private GameObject _currentLevel;
    private GameObject _nextLevel;
    private AudioClip _nextTrack;
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

    public void NextLevel()
    {
        Destroy(_lastLevel);
        _lastLevel = _currentLevel;
        _currentLevel = _nextLevel;
        musicPlayer.clip = _nextTrack;
        musicPlayer.Play();
        CreateNewLevel();
        _player.transform.position += new Vector3(0.0f, 10.0f);
    }

    private void CreateNewLevel()
    {
        if (_numLevels >= maxLevels) return;
        var index = Random.Range(0, levels.Length);
        _nextLevel = Instantiate(levels[index], _nextLevelPosition, Quaternion.identity, transform);
        _nextTrack = levelTracks[index];
        _numLevels++;
        _nextLevelPosition += new Vector3(0.0f, 120.0f);
    }
}
