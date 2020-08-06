﻿using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabMenu : MonoBehaviour
{
    public GameObject statsUiPrefab;
    public GameObject itemsUiPrefab;
    public GameObject itemUiPrefab;
    public GameObject abilitiesUiPrefab;
    public GameObject abilityUiPrefab;
    public List<Stats> playerStats;

    public List<GameObject> menuScreens;
    public GameInfo gameInfo;
    public Inventory inventory;
    
    private void Awake()
    {
        LoadTabMenuScreenData();
    }

    public void LoadTabMenuScreenData()
    {
        UnloadOldItems();
        SetUpItems();
        foreach (var player in playerStats)
        {
            SetUpPlayerStats(player);

            SetUpAbilities(player);
        }
    }

    public void UnloadOldItems()
    {
        foreach (var menuScreen in menuScreens)
        {
            for (int i = 0; i < menuScreen.transform.childCount; i++)
            {
                Destroy(menuScreen.transform.GetChild(i));
            }
        }
    }

    private void SetUpAbilities(Stats player)
    {
        //Contains abilities and spells
        var playerAbilitiesUi = Instantiate(abilitiesUiPrefab, menuScreens[2].transform, true);
        playerAbilitiesUi.name = player.name.ToUpper();
        foreach (var t in player.abilities)
        {
            var playerAbilityUi = Instantiate(abilityUiPrefab, playerAbilitiesUi.transform.GetChild(1), true);
            playerAbilityUi.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = t.name.ToUpper();
            playerAbilityUi.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ("Damage: " + t.damage).ToUpper();
        }
        
        foreach (var t in player.spells)
        {
            var currentSpellUi = Instantiate(abilityUiPrefab, playerAbilitiesUi.transform.GetChild(1), true);
            currentSpellUi.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = t.name.ToUpper();;
            currentSpellUi.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ("Damage: " + t.damage + " Cost: " + t.manaCost).ToUpper();
        }
    }

    private void SetUpItems()
    {
        var itemsUi = Instantiate(itemsUiPrefab, menuScreens[1].transform, true);
        foreach (var item in inventory.items)
        { 
            var itemUi = Instantiate(itemUiPrefab, itemsUi.transform, true);
            itemUi.GetComponent<TextMeshProUGUI>().text = (item.quantity + " " + item.name + "s" ).ToUpper();
        }
    }

    private void SetUpPlayerStats(Stats player)
    {
        var playerStatsUi = Instantiate(statsUiPrefab, menuScreens[0].transform, true);
        playerStatsUi.name = player.name;
        playerStatsUi.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Health: " + player.health;
        if (player.maxMana > 0)
            playerStatsUi.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Mana: " + player.mana;
        else
            Destroy(playerStatsUi.transform.GetChild(1).GetChild(1));
        playerStatsUi.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Speed: " + player.speed;
        playerStatsUi.transform.GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Damage: " + player.damage;
    }

    private void Start()
    {
        EventHandler.Instance.onToggleTabMenu += ToggleTabMenu;
        gameInfo = EventHandler.Instance.gameInfo;
    }

    private void OnDestroy()
    {
        EventHandler.Instance.onToggleTabMenu -= ToggleTabMenu;
    }

    public void Back()
    {
        EventHandler.Instance.ToggleTabMenu();
    }

    public void ToggleTabMenu(bool enableTabMenu)
    {
        AudioManager.instance.PlaySound("ui");
        transform.GetChild(0).gameObject.SetActive(enableTabMenu);
        if(SkillTreeController.instance.skillTree.activeSelf) SkillTreeController.instance.skillTree.SetActive(false);
    }
}
