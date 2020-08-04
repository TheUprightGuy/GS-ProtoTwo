using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTestScene2 : MonoBehaviour
{
    public Scene scene;
    // Start is called before the first frame update
    void Start()
    {
        var parameters = new LoadSceneParameters(LoadSceneMode.Additive);
        scene = SceneManager.LoadScene("TestScene2", parameters);
        foreach (var rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            rootGameObject.SetActive(false);
        }
    }
}
