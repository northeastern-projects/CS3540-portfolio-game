using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class PanelManager : MonoBehaviour
{
    public UIDocument start;
    public UIDocument pause;
    public UIDocument end;

    public AudioClip startMusic;
    public AudioClip pauseMusic;
    public AudioClip endMusic;

    [SerializeField] private GameData gameData;

    private EventSystem _eventSystem;
    private AudioSource _musicPlayer;
    private AudioClip _previousClip;

    void Start()
    {
        _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        _musicPlayer = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        _eventSystem.enabled = false;
        ShowPanel(start);
        HidePanel(pause);
        HidePanel(end);
        PlayMusic(startMusic);
    }

    void Update()
    {
        if (!gameData.started && Input.GetKeyDown(KeyCode.Return))
        {
            gameData.started = true;
            HidePanel(start);
            _eventSystem.enabled = true;
            PlayMusic(_previousClip);
            Debug.Log("started");
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
            ShowPanel(end);
            _eventSystem.enabled = false;
            PlayMusic(endMusic);
            Debug.Log("end");
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
