using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EndMenu : MonoBehaviour
{
    private VisualElement root;

    private Label callToAction;
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        callToAction = root.Q<Label>("CallToAction");
        Time.timeScale = 1;
        InvokeRepeating(nameof(blinkCallToAction), 0.25f, 0.25f);

    }

    void blinkCallToAction()
    {
        callToAction.visible = !callToAction.visible;
    }
}
