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
    
    public GameInfo gameInfo;
    public Action<bool> onPaused;
    public bool settingsOpen;
    public Action<bool> toggleSettingsMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !settingsOpen)
        {
            TogglePaused();
        }
    }

    public void TogglePaused()
    {
        gameInfo.paused = !gameInfo.paused;
        onPaused(gameInfo.paused);
    }
}
