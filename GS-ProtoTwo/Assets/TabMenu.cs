using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabMenu : MonoBehaviour
{
    private void Start()
    {
        EventHandler.Instance.onToggleTabMenu += ToggleTabMenu;
    }

    public void Back()
    {
        EventHandler.Instance.ToggleTabMenu();
    }

    private void ToggleTabMenu(bool enableTabMenu)
    {
        transform.GetChild(0).gameObject.SetActive(enableTabMenu);
    }
}
