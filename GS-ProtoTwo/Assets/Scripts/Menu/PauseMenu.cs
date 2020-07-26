using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private void Awake()
    {
        ToggleMenu(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        EventHandler.Instance.onPaused += ToggleMenu;
        EventHandler.Instance.toggleSettingsMenu += ToggleSettingsMenu;
    }

    private void ToggleMenu(bool isPaused)
    {
        transform.GetChild(0).gameObject.SetActive(isPaused);
    }
    
    private void ToggleSettingsMenu(bool enableSettings)
    {
        transform.GetChild(0).gameObject.SetActive(!enableSettings);
    }

    public void Resume()
    {
        EventHandler.Instance.TogglePaused();
    }

    public void ReturnToMenu()
    {
        //Trigger Scene transition - To do
        SceneManager.LoadScene("MenuScene");
    }

    public void OpenSettings()
    {
        EventHandler.Instance.toggleSettingsMenu(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
