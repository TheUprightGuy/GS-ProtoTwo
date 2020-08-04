using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivateOtherScene : MonoBehaviour
{
    public GameInfo gameInfo;
    public void Start()
    {
        gameInfo.activeScene = GameInfo.ActiveScene.CombatScene;
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
        }
        if (!Input.GetKeyDown(KeyCode.A)) return;
        Debug.Log("A pressed");
        foreach (var rootGameObject in SceneManager.GetSceneAt(1).GetRootGameObjects())
        {
            rootGameObject.SetActive(gameInfo.activeScene == GameInfo.ActiveScene.CombatScene);
        }
        
        foreach (var rootGameObject in SceneManager.GetSceneAt(0).GetRootGameObjects())
        {
            rootGameObject.SetActive(gameInfo.activeScene == GameInfo.ActiveScene.WorldScene);
        }

        gameInfo.activeScene = (gameInfo.activeScene == GameInfo.ActiveScene.CombatScene)
            ? GameInfo.ActiveScene.WorldScene
            : GameInfo.ActiveScene.CombatScene;
    }
}
