using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    #region Singleton
    public static SceneTransition instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than One SceneHandler Exists!");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion Singleton
    #region Setup
    [HideInInspector] public Transition transition;
    public void Start()
    {
        gameInfo = EventHandler.Instance.gameInfo;
        gameInfo.activeScene = GameInfo.ActiveScene.WorldScene;
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));

        transition = GetComponent<Transition>();
    }
    #endregion Setup

    [HideInInspector] public GameInfo gameInfo;

    // Switches Active Objects
    public void SwapScene()
    {
        foreach (var rootGameObject in SceneManager.GetSceneAt(1).GetRootGameObjects())
        {
            rootGameObject.SetActive(gameInfo.activeScene == GameInfo.ActiveScene.CombatScene);
        }

        foreach (var rootGameObject in SceneManager.GetSceneAt(0).GetRootGameObjects())
        {
            rootGameObject.SetActive(gameInfo.activeScene == GameInfo.ActiveScene.WorldScene);
        }
    }

    // Cleans Up Scene
    public void UnloadScene()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
    }

    // Plays Anim then Swaps Scene
    public void GoToCombat()
    {
        AudioManager.instance.SwitchMusicTrack("battle");
        gameInfo.worldPaused = true;
        gameInfo.activeScene = GameInfo.ActiveScene.CombatScene;
        transition.nextSceneToLoad = "CombatScene";
        // Play Anim
        StartCoroutine(transition.TransitionToCombat());
    }

    // Swaps Scene
    public void MoveToCombat()
    {
        var parameters = new LoadSceneParameters(LoadSceneMode.Additive);
        SceneManager.LoadScene(transition.nextSceneToLoad, parameters);

        SwapScene();
    }

    // Swaps Scene
    public void GoToWorld()
    {
        gameInfo.worldPaused = false;
        gameInfo.activeScene = GameInfo.ActiveScene.WorldScene;

        SwapScene();
        UnloadScene();
        AudioManager.instance.SwitchMusicTrack("world");
    }
}
