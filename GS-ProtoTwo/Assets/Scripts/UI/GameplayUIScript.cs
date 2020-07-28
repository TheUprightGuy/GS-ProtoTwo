using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUIScript : MonoBehaviour
{
    #region Singleton
    public static GameplayUIScript instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Another GameplayUI Script already exists!");
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    #endregion Singleton

    public GameObject victoryCanvas;
    public GameObject gameOverCanvas;

    public GameObject actionCanvasPrefab;
    public SetupStatusBar statusBars;
    public List<ActionScript> playerUI;

    public List<TargetScript> targets;

    private void Start()
    {
        CombatController.instance.toggleActionCanvas += ToggleActionCanvas;
        CombatController.instance.setupCanvas += SetupCanvas;
        CombatController.instance.turnOffTarget += TurnOff;

        ToggleVictory(false);
        ToggleGameOver(false);
    }

    private void OnDestroy()
    {
        CombatController.instance.toggleActionCanvas -= ToggleActionCanvas;
        CombatController.instance.setupCanvas -= SetupCanvas;
        CombatController.instance.turnOffTarget -= TurnOff;
    }

    public void SetupCanvas(PlayerController _player)
    {
        ActionScript temp = Instantiate(actionCanvasPrefab, this.transform).GetComponent<ActionScript>();
        temp.PlayerRef(_player);
        playerUI.Add(temp);

        foreach (Ability n in _player.stats.abilities)
        {
            temp.abilityMenu.GetComponent<SetupAbility>().AddAbilityButton(_player, n);
        }
        foreach (Magic n in _player.stats.spells)
        {
            temp.magicMenu.GetComponent<SetupMagic>().AddMagicButton(_player, n);
        }
        foreach (Item n in _player.stats.items)
        {
            temp.itemMenu.GetComponent<SetupItems>().AddItemButton(_player, n);
        }

        statusBars.Setup(_player);
    }
       
    public void ToggleActionCanvas(BaseCharacterClass _player, bool _toggle)
    {
        foreach (ActionScript n in playerUI)
        {
            if (n.player == _player)
            {
                n.gameObject.SetActive(_toggle);
            }
            else
            {
                n.gameObject.SetActive(false);
            }
        }
    }

    public void TurnOff(int _id)
    {
        for (int i = targets.Count - 1; i >= 0; i--)
        {
            if (targets[i].id == _id)
            {
                Destroy(targets[i].gameObject);
                targets.RemoveAt(i);
            }
        }
    }

    public void ToggleVictory(bool _toggle)
    {
        victoryCanvas.SetActive(_toggle);
    }

    public void ToggleGameOver(bool _toggle)
    {
        gameOverCanvas.SetActive(_toggle);
    }

    public event Action<Item> updateItemQuantity;
    public void UpdateItemQuantity(Item _item)
    {
        if (updateItemQuantity != null)
        {
            updateItemQuantity(_item);
        }
    }
}
