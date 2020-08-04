using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    [HideInInspector]public List<TabButton> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabSelected;
    public TabButton selectedTab;
    public List<GameObject> menuCanvases;

    // temp sorry
    public TabMenu tabMenu;

    public void Awake()
    {
        ResetTabs();
    }

    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
        ResetTabs();
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.sprite = tabHover;
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }
    
    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.sprite = tabSelected;
        int index = button.transform.GetSiblingIndex();

        for (int i = 0; i < menuCanvases.Count; i++)
        {
            if (i == index)
            {
                menuCanvases[i].SetActive(true);
            }
            else
            {
                menuCanvases[i].SetActive(false);
            }
        }     
    }

    private void ResetTabs()
    {
        foreach (var button in tabButtons)
        {
            if(button == selectedTab) continue;
            button.background.sprite = tabIdle;
        }
    }
}
