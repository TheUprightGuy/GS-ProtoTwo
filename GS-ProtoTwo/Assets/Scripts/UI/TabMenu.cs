using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.UI;

public class TabMenu : MonoBehaviour
{
    public GameObject statsUIPrefab;
    public GameObject itemsUIPrefab;
    public GameObject abilitiesUIPrefab;
    public GameObject spellsUIPrefab;
    public List<Stats> playerStats;

    public List<GameObject> menuScreens;
    public GameInfo gameInfo;
    
    private void Awake()
    {
        LoadTabMenuScreenData();
    }

    private void LoadTabMenuScreenData()
    {
        foreach (var player in playerStats)
        {
            var playerStatsUI = Instantiate(statsUIPrefab, menuScreens[0].transform, true);
            playerStatsUI.name = player.name;
            playerStatsUI.transform.GetChild(1).GetComponent<Text>().text = "Health: " + player.health;
            if(player.maxMana > 0) playerStatsUI.transform.GetChild(2).GetComponent<Text>().text = "Mana: " + player.mana;
            playerStatsUI.transform.GetChild(3).GetComponent<Text>().text = "Speed: " + player.speed;
            playerStatsUI.transform.GetChild(4).GetComponent<Text>().text = "Damage: " + player.damage;
            
            var playerItemsUI = Instantiate(itemsUIPrefab, menuScreens[1].transform, true);
            playerItemsUI.name = player.name;

            var playerAbilitiesUI = Instantiate(abilitiesUIPrefab, menuScreens[2].transform, true);
            playerAbilitiesUI.name = player.name;
            playerAbilitiesUI.transform.GetChild(1).GetComponent<Text>().text =
                player.abilities[0].name + " Damage: " + player.abilities[0].damage;

            
            var playerSpellsUI = Instantiate(spellsUIPrefab, menuScreens[3].transform, true);
            playerSpellsUI.name = player.name;
            for (int i = 0; i < player.spells.Count; i++)
            {
                var currentSpellUI = playerSpellsUI.transform.GetChild(i+1);
                currentSpellUI.GetComponent<Text>().text = player.spells[i].name;
                currentSpellUI.transform.GetChild(0).GetComponent<Text>().text = " Damage: " + player.spells[i].damage;
                currentSpellUI.transform.GetChild(1).GetComponent<Text>().text = " Cost: " + player.spells[i].manaCost;
            }
        }
    }

    private void Start()
    {
        EventHandler.Instance.onToggleTabMenu += ToggleTabMenu;
        gameInfo = EventHandler.Instance.gameInfo;
    }

    public void Back()
    {
        EventHandler.Instance.ToggleTabMenu();
    }

    private void ToggleTabMenu(bool enableTabMenu)
    {
        AudioManager.instance.PlaySound("ui");
        transform.GetChild(0).gameObject.SetActive(enableTabMenu);
    }
}
