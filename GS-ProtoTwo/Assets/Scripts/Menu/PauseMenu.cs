using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private void Awake()
    {
        TogglePauseMenu(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        EventHandler.Instance.onTogglePauseMenu += TogglePauseMenu;
        EventHandler.Instance.toggleSettingsMenu += ToggleSettingsMenu;
    }

    private void TogglePauseMenu(bool isPaused)
    {
        transform.GetChild(0).gameObject.SetActive(isPaused);
    }
    
    private void ToggleSettingsMenu(bool enableSettings)
    {
        transform.GetChild(0).gameObject.SetActive(!enableSettings);
    }

    public void Resume()
    {
        EventHandler.Instance.TogglePauseMenu();
    }

    public void ReturnToMenu()
    {
        //Trigger Scene transition - To do
        SceneManager.LoadScene("MenuScene");
    }

    public void OpenSettings()
    {
        EventHandler.Instance.settingsMenuOpen = true;
        EventHandler.Instance.toggleSettingsMenu(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
