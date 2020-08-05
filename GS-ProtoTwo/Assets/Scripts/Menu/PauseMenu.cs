using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    #region Singleton
    public static PauseMenu instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one Pausemenu in scene!");
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    #endregion
    
    #region Callbacks

    // Start is called before the first frame update
    void Start()
    {
        EventHandler.Instance.onTogglePauseMenu += TogglePauseMenu;
        EventHandler.Instance.toggleSettingsMenu += ToggleSettingsMenu;
        gameInfo = EventHandler.Instance.gameInfo;
        TogglePauseMenu(false);
    }

    private void OnDestroy()
    {
        EventHandler.Instance.onTogglePauseMenu -= TogglePauseMenu;
        EventHandler.Instance.toggleSettingsMenu -= ToggleSettingsMenu;
    }
    #endregion Callbacks

    public GameInfo gameInfo;
    private void TogglePauseMenu(bool isPaused)
    {
        if (AudioManager.instance != null) AudioManager.instance.PlaySound("ui");
        transform.GetChild(0).gameObject.SetActive(isPaused);
    }
    
    private void ToggleSettingsMenu(bool enableSettings)
    {
        AudioManager.instance.PlaySound("ui");
        transform.GetChild(0).gameObject.SetActive(!enableSettings);
    }

    public void Resume()
    {
        AudioManager.instance.PlaySound("ui");
        EventHandler.Instance.TogglePauseMenu();
    }

    public void ReturnToMenu()
    {
        AudioManager.instance.PlaySound("ui");
        //Trigger Scene transition - To do
        SceneManager.LoadScene("MenuScene");
    }

    public void OpenSettings()
    {
        AudioManager.instance.PlaySound("ui");
        EventHandler.Instance.settingsMenuOpen = true;
        EventHandler.Instance.toggleSettingsMenu(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
