using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    private void Start()
    {
        EventHandler.Instance.toggleSettingsMenu += ToggleSettingsMenu;
    }

    public void Back()
    {
        //Trigger toggle settings menu action which disables settings menu and enable pause menu
        EventHandler.Instance.settingsMenuOpen = false; 
        EventHandler.Instance.toggleSettingsMenu(false);
    }

    private void ToggleSettingsMenu(bool enableSettings)
    {
        transform.GetChild(0).gameObject.SetActive(enableSettings);
    }
}
