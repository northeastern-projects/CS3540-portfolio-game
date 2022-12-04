using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    
    //component for players health script
    [SerializeField] private Health playerHealth;
    //initial values for coins and hearts
    public static int acorns;
    public static int numberOfCoins;
    public static int numberOfHearts;
    public static float currentTime;
    public static int numKeys = 0;

    public TextMeshProUGUI acornText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI heartsText;
    public TextMeshProUGUI timerText;

    private void Start()
    {
        acorns = PlayerPrefs.GetInt("acorns");
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        
        numberOfHearts = playerHealth.GetHealth();
        acornText.text = acorns.ToString();
        coinsText.text = numberOfCoins.ToString();
        heartsText.text = numberOfHearts.ToString();
        timerText.text = currentTime.ToString("0.00");
    }
    
    //Pays coins and return true on sucess, coins unchanaged and returns False if can't afford
    public static bool Pay(int amount)
    {
        bool paymentSuccess = true;

        if (numberOfCoins >= amount)
        {
            numberOfCoins -= amount;
        }
        else
        {
            paymentSuccess = false;
        }

        return paymentSuccess;
    }

    public static void startTimer()
    {
        currentTime = 0.0f;
    }

    public static void CollectAcorn()
    {
        acorns++;
        PlayerPrefs.SetInt("acorns", acorns);
    }
}
