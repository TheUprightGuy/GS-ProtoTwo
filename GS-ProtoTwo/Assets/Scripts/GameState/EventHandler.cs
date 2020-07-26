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
        if (Instance != null)
        {
            Debug.Log("More than one EventHandler in scene!");
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        isPaused = false;
        SceneManager.activeSceneChanged += OnSceneChanged;
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnSceneChanged(Scene current, Scene next)
    {
        Debug.Log(current.name + " changed to " + next.name);
        isPaused = false;
    }

    #endregion
    
    public bool isPaused;
    public Action<bool> onPaused;
    public Action<bool> toggleSettingsMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePaused();
        }
    }

    public void TogglePaused()
    {
        isPaused = !isPaused;
        onPaused(isPaused);
    }
}
