using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    [HideInInspector]public List<TabButton> tabButtons;
    public Sprite tab;
    public Color32 tabIdle;
    public Color32 tabHover;
    public Color32 tabSelected;
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
            button.background.color = tabHover;
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
        button.background.color = tabSelected;
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
            button.background.color = tabIdle;
        }
    }
}
