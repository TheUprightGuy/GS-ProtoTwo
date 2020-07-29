using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class EventHandler : MonoBehaviour
{
    #region Singleton

    public static EventHandler Instance;

    private void Awake()
    {
        gameInfo = ScriptableObject.CreateInstance<GameInfo>();
        if (Instance != null)
        {
            Debug.Log("More than one EventHandler in scene!");
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        gameInfo.paused = false;
        SceneManager.activeSceneChanged += OnSceneChanged;
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnSceneChanged(Scene current, Scene next)
    {
        Debug.Log(current.name + " changed to " + next.name);
        gameInfo.paused = false;
    }

    #endregion
    
    //Game state pause
    public GameInfo gameInfo;
    public Action<bool> onPauseToggled;
    public Action<bool> onTogglePauseMenu;
    //Settings menu
    [HideInInspector] public bool settingsMenuOpen;
    public Action<bool> toggleSettingsMenu;
    //Tab menu
    [HideInInspector] public bool tabMenuOpen;
    public Action<bool> onToggleTabMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !settingsMenuOpen)
        {
            TogglePauseMenu();
        }
        
        if (Input.GetKeyDown(KeyCode.Tab) && !gameInfo.pauseMenuOpen && !settingsMenuOpen)
        {
            ToggleTabMenu();
        }
    }

    public void TogglePauseMenu()
    {
        gameInfo.paused = (tabMenuOpen == true)? gameInfo.paused : !gameInfo.paused;
        gameInfo.pauseMenuOpen = !gameInfo.pauseMenuOpen;
        onPauseToggled?.Invoke(gameInfo.paused);
        onTogglePauseMenu(gameInfo.pauseMenuOpen);
    }
    
    public void ToggleTabMenu()
    {
        gameInfo.paused = !gameInfo.paused;
        tabMenuOpen = !tabMenuOpen;
        onPauseToggled?.Invoke(gameInfo.paused);
        onToggleTabMenu(tabMenuOpen);
    }
}
