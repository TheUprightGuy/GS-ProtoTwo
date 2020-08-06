using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Awake()
    {
        //Reset gamestate
        EventHandler.Instance.gameInfo.paused = false;
        EventHandler.Instance.gameInfo.worldPaused = false;
        EventHandler.Instance.gameInfo.pauseMenuOpen = false;
        //Search for pause menu and destroy if it still exists from world/combat scene
        var pMenu = FindObjectOfType<PauseMenu>();
        if(pMenu != null) Destroy(pMenu.gameObject);
        if (TabMenu.instance != null) Destroy(TabMenu.instance);
    }

    public void Start()
    {
        AudioManager.instance.SwitchMusicTrack("menu+town");
    }

    public Transition transition;
    public void StartGame()
    {
        AudioManager.instance.PlaySound("ui");
        AudioManager.instance.SwitchMusicTrack("world");
        StartCoroutine(transition.TransitionToWorld());
    }

    public void Settings()
    {
        AudioManager.instance.PlaySound("ui");
        SettingsMenu.instance.transform.GetChild(0).gameObject.SetActive(true);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
