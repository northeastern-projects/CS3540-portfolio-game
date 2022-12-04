using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EndMenu : MonoBehaviour
{
    public VisualElement root;

    private Label callToAction;
    private Label highScore;
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        callToAction = root.Q<Label>("CallToAction");
        highScore = root.Q<Label>("Highscore");
        Time.timeScale = 1;
        InvokeRepeating(nameof(blinkCallToAction), 0.25f, 0.25f);
        Debug.Log("Record time set to: " + PlayerPrefs.GetFloat("fastestTime").ToString("0.00"));
    }

    private void Update()
    {
        highScore.text = "Record Time: " + PlayerPrefs.GetFloat("fastestTime").ToString("0.00");
    }

    void blinkCallToAction()
    {
        callToAction.visible = !callToAction.visible;
    }
}