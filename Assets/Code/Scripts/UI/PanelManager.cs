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
    public UIDocument difficulty;

    [SerializeField] private GameData gameData;
    
    [SerializeField] private EnemyData antData;
    [SerializeField] private EnemyData beeData;
    [SerializeField] private EnemyData fireAntData;
    [SerializeField] private EnemyData yellowJacketData;

    private EventSystem eventSystem;

    void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        eventSystem.enabled = false;
        ShowPanel(start);
        HidePanel(difficulty);
        HidePanel(pause);
        HidePanel(end);
    }

    void Update()
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
                eventSystem.enabled = true;
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
                eventSystem.enabled = true;
                Debug.Log("gameStarted");
                gameData.onDifficultyScreen = false;
            }
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
