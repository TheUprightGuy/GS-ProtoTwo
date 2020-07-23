using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    #region Singleton
    public static EventHandler instance;
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
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion Singleton
}
