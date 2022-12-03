using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//This script will be used on the last door in the final level to track when the full game is complete.  
public class LeaderboardManager : MonoBehaviour
{
    [SerializeField]
    public PlayerManager playerManager;
    
    public float timeOfLastRun;         //time of run just completed
    public float fastestTime;              //variable representing the fastest time the game has been completed

    // Start is called before the first frame update
    void Start()
    {
        timeOfLastRun = playerManager.currentTime;
        fastestTime = PlayerPrefs.GetFloat("fastestTime");
    }

    public void CheckBestTime()
    {
        if (timeOfLastRun < fastestTime)
        {
            PlayerPrefs.SetFloat("fastestTime", timeOfLastRun);
        }
    }
    
}
