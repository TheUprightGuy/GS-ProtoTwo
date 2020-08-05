using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class SettingsMenu : MonoBehaviour
{
    #region Singleton
    public static SettingsMenu instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one SettingsMenu in scene!");
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    #endregion
    public Slider volumeSlider;
    private void Start()
    {
        EventHandler.Instance.toggleSettingsMenu += ToggleSettingsMenu;
    }

    public void Back()
    {
        AudioManager.instance.PlaySound("ui");
        //Trigger toggle settings menu action which disables settings menu and enable pause menu
        EventHandler.Instance.settingsMenuOpen = false; 
        EventHandler.Instance.toggleSettingsMenu(false);
    }

    private void ToggleSettingsMenu(bool enableSettings)
    {
        transform.GetChild(0).gameObject.SetActive(enableSettings);
    }
}
