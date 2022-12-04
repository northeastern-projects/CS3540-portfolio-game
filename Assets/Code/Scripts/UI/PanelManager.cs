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

    [SerializeField] private GameData gameData;

    private EventSystem eventSystem;

    void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.enabled = false;
        ShowPanel(start);
        HidePanel(pause);
        HidePanel(end);
    }

    void Update()
    {
        if (!gameData.started && Input.GetKeyDown(KeyCode.Return))
        {
            gameData.started = true;
            HidePanel(start);
            eventSystem.enabled = true;
            Debug.Log("started");
        }

        if (gameData.started && !gameData.paused && Input.GetKeyDown(KeyCode.Escape))
        {
            gameData.paused = true;
            ShowPanel(pause);
            eventSystem.enabled = false;
            Debug.Log("paused");
        }

        if (gameData.started && gameData.paused && Input.GetKeyDown(KeyCode.Return))
        {
            gameData.paused = false;
            HidePanel(pause);
            eventSystem.enabled = true;
            Debug.Log("unpaused");
        }

        if (!gameData.ended && !GameObject.FindWithTag("Player"))
        {
            gameData.ended = true;
            LeaderboardManager.CheckBestTime();
            ShowPanel(end);
            eventSystem.enabled = false;
            Debug.Log("end");
        }
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
