using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Move this to event handler in future
using UnityEngine.SceneManagement;

public class EventHandler : MonoBehaviour
{
    #region Singleton
    public static EventHandler instance;
    //private Animator animator;
    private string sceneToLoad;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one EventHandler exists!");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        //animator = GetComponent<Animator>();
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion Singleton

    // Part is Selected
    public event Action<int> toggleTurn;
    public void ToggleTurn(int _id)
    {
        if (toggleTurn != null)
        {
            toggleTurn(_id);
        }
    }
}
