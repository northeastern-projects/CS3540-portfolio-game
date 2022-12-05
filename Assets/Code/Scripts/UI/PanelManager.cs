using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour
{
    public UIDocument start;
    public UIDocument pause;
    public UIDocument end;
    public UIDocument difficulty;

    public AudioClip startMusic;
    public AudioClip pauseMusic;
    public AudioClip endMusic;

    [SerializeField] private GameData gameData;
    
    [SerializeField] private EnemyData antData;
    [SerializeField] private EnemyData beeData;
    [SerializeField] private EnemyData fireAntData;
    [SerializeField] private EnemyData yellowJacketData;

    private EventSystem _eventSystem;
    private AudioSource _musicPlayer;
    private AudioClip _previousClip;

    void Start()
    {
        _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        _musicPlayer = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        _eventSystem.enabled = false;
        ShowPanel(start);
        HidePanel(difficulty);
        HidePanel(pause);
        HidePanel(end);
        PlayMusic(startMusic);
    }

    void Update()
    {
        StartCoroutine(Screens());
    }

    IEnumerator Screens()
    {
        if (!gameData.started && !gameData.onDifficultyScreen && Input.GetKeyDown(KeyCode.Return))
        {
            //Want to show difficulty then 
            
            gameData.onDifficultyScreen = true;
            HidePanel(start);
            ShowPanel(difficulty);
            Debug.Log("Showing difficulty");
        }

        if (gameData.onDifficultyScreen)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                HidePanel(difficulty);
                
                
                gameData.started = true;
                PlayerManager.startTimer();
                _eventSystem.enabled = true;
                
                PlayMusic(_previousClip);
                Debug.Log("gameStarted");
                gameData.onDifficultyScreen = false;
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                HidePanel(difficulty);
                
                antData.moveSpeed += 3;
                beeData.moveSpeed += 3;
                fireAntData.moveSpeed += 3;
                yellowJacketData.moveSpeed += 3;
                
                
                gameData.started = true;
                PlayerManager.startTimer();
                _eventSystem.enabled = true;
                Debug.Log("gameStarted");
                gameData.onDifficultyScreen = false;
            }
        }

        if (gameData.started && !gameData.paused && Input.GetKeyDown(KeyCode.Escape))
        {
            gameData.paused = true;
            ShowPanel(pause);
            _eventSystem.enabled = false;
            PlayMusic(pauseMusic);
            Debug.Log("paused");
        }

        if (gameData.started && gameData.paused && Input.GetKeyDown(KeyCode.Return))
        {
            gameData.paused = false;
            HidePanel(pause);
            _eventSystem.enabled = true;
            PlayMusic(_previousClip);
            Debug.Log("unpaused");
        }

        if (!gameData.ended && !GameObject.FindWithTag("Player"))
        {
            gameData.ended = true;
            LeaderboardManager.CheckBestTime();
            ShowPanel(end);
            _eventSystem.enabled = false;
            PlayMusic(endMusic);
            Debug.Log("end");
            
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene( SceneManager.GetActiveScene().name );
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        _previousClip = _musicPlayer.clip;
        _musicPlayer.clip = clip;
        _musicPlayer.Play();
    }

    private void ShowPanel(UIDocument panel)
    {
        panel.GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.Flex;
    }
    
    private void HidePanel(UIDocument panel)
    {
        panel.GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.None;
    }
}
