using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    [SerializeField] private int maxLevels;
    [SerializeField] private GameObject[] levels;
    [SerializeField] private AudioClip[] levelTracks;
    [SerializeField] private Sprite bossDoor;
    [SerializeField] private GameObject[] bossLevels;
    [SerializeField] private AudioClip[] bossTracks;
    [SerializeField] private AudioSource musicPlayer;

    private GameObject _lastLevel;
    private GameObject _currentLevel;
    private GameObject _nextLevel;
    private AudioClip _nextTrack;
    private Rigidbody2D _playerRb;

    private int _numLevels;
    private Vector3 _nextLevelPosition;

    // Start is called before the first frame update
    private void Start()
    {
        _playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
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
        _playerRb.transform.position += new Vector3(0.0f, 10.0f);
    }

    private void CreateNewLevel()
    {
        if (_numLevels == maxLevels - 1)
        {
            var index = Random.Range(0, bossLevels.Length);
            var finalDoor = _currentLevel.GetComponentInChildren<Transform>().Find("Level_End");
            finalDoor.GetComponent<SpriteRenderer>().sprite = bossDoor;
            _nextLevel = Instantiate(bossLevels[index], _nextLevelPosition, Quaternion.identity, transform);
            _nextTrack = bossTracks[index];
        }
        else if (_numLevels < maxLevels)
        {
            var index = Random.Range(0, levels.Length);
            _nextLevel = Instantiate(levels[index], _nextLevelPosition, Quaternion.identity, transform);
            _nextTrack = levelTracks[index];
            _nextLevelPosition += new Vector3(0.0f, 120.0f);
        }
        _numLevels++;
    }
}
