using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//This script will be used on the last door in the final level to track when the full game is complete.  
public class LeaderboardManager : MonoBehaviour
{
    public PlayerManager playerManager;
    
    private static float timeOfLastRun;         //time of run just completed
    private static float fastestTime;              //variable representing the fastest time the game has been completed

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        timeOfLastRun = playerManager.currentTime;
        fastestTime = PlayerPrefs.GetFloat("fastestTime", 9999.0f);
    }

    private void Update()
    {
        timeOfLastRun = playerManager.currentTime;
    }

    public static void CheckBestTime()
    {

        Debug.Log("runTime: " + timeOfLastRun);
        Debug.Log("current record: " + fastestTime);
        if (timeOfLastRun < fastestTime)
        {
            PlayerPrefs.SetFloat("fastestTime", timeOfLastRun);
        }

        fastestTime = timeOfLastRun;

        Debug.Log("fastest time is: " + PlayerPrefs.GetFloat("fastestTime").ToString("0.00"));
    }
    
}
