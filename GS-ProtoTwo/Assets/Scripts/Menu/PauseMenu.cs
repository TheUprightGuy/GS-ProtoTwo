using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameInfo gameInfo;
    private void Awake()
    {
        TogglePauseMenu(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        EventHandler.Instance.onTogglePauseMenu += TogglePauseMenu;
        EventHandler.Instance.toggleSettingsMenu += ToggleSettingsMenu;
        gameInfo = EventHandler.Instance.gameInfo;
    }

    private void TogglePauseMenu(bool isPaused)
    {
        AudioManager.Instance.PlaySound("ui");
        transform.GetChild(0).gameObject.SetActive(isPaused);
    }
    
    private void ToggleSettingsMenu(bool enableSettings)
    {
        AudioManager.Instance.PlaySound("ui");
        transform.GetChild(0).gameObject.SetActive(!enableSettings);
    }

    public void Resume()
    {
        AudioManager.Instance.PlaySound("ui");
        EventHandler.Instance.TogglePauseMenu();
    }

    public void ReturnToMenu()
    {
        AudioManager.Instance.PlaySound("ui");
        //Trigger Scene transition - To do
        SceneManager.LoadScene("MenuScene");
    }

    public void OpenSettings()
    {
        AudioManager.Instance.PlaySound("ui");
        EventHandler.Instance.settingsMenuOpen = true;
        EventHandler.Instance.toggleSettingsMenu(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
