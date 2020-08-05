using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Start()
    {
        AudioManager.instance.SwitchMusicTrack("menu+town");
    }

    public Transition transition;
    public void StartGame()
    {
        AudioManager.instance.PlaySound("ui");
        StartCoroutine(transition.TransitionToWorld());
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
