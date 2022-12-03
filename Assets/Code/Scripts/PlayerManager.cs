using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //initial values for coins and hearts
    public static int numberOfCoins = 0;
    public static int numberOfHearts = 2;

    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI heartsText;
    // Update is called once per frame
    void Update()
    {
        coinsText.text = numberOfCoins.ToString();
        heartsText.text = numberOfHearts.ToString();
    }

    public static bool isFullHealth()
    {
        return numberOfHearts == 3;
    }
}
