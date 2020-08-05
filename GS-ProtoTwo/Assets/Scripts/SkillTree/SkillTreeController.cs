using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeController : MonoBehaviour
{
    #region Singleton
    [HideInInspector] public static SkillTreeController instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one SkillTreeController Exists!");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion Singleton
    public GameObject skillTree;

    private void Start()
    {
        Invoke("DelayedStart", 0.05f);
    }

    void DelayedStart()
    {
        Toggle(false);
    }

    public void Toggle(bool _toggle)
    {
        skillTree.SetActive(_toggle);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Toggle(!skillTree.activeSelf);
            Cursor.lockState = (skillTree.activeSelf) ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
