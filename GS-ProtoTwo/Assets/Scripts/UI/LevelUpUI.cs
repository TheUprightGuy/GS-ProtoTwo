using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventHandler.Instance.onToggleTabMenu += OnToggleTabMenu;
        EventHandler.Instance.onLevelUp += OnLevelUp;
    }
    
    void OnDestroy()
    {
        EventHandler.Instance.onToggleTabMenu -= OnToggleTabMenu;
        EventHandler.Instance.onLevelUp -= OnLevelUp;
    }

    private void OnToggleTabMenu(bool obj)
    {
        Destroy(gameObject);
    }
    
    private void OnLevelUp()
    {
        //New level up UI message will be created so this one should be destroyed
        Destroy(gameObject);
    }
}
