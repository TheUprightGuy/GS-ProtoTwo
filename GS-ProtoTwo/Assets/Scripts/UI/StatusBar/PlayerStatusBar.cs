using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusBar : MonoBehaviour
{
    //[HideInInspector]
    public PlayerController player;

    public Image health;
    public TMPro.TextMeshProUGUI healthText;
    public Image mana;
    public TMPro.TextMeshProUGUI manaText;
    public TMPro.TextMeshProUGUI playerName;

    private void Awake()
    {
        CombatController.instance.updateStatus += UpdateValues;
    }

    private void OnDestroy()
    {
        CombatController.instance.updateStatus -= UpdateValues;
    }

    public void Setup(PlayerController _player)
    {
        player = _player;
        playerName.SetText(_player.name);
        UpdateValues(_player);
    }

    public void UpdateHealth()
    {
        health.fillAmount = (float)player.stats.health / (float)player.stats.maxHealth;
        healthText.SetText(player.stats.health + "/" + player.stats.maxHealth);       
    }

    public void UpdateMana()
    {
        mana.fillAmount = (float)player.stats.mana / (float)player.stats.maxMana;
        manaText.SetText(player.stats.mana + "/" + player.stats.maxMana);
    }

    public void UpdateValues(PlayerController _player)
    {
        if (player == _player)
        {
            UpdateHealth();
            UpdateMana();
        }
    }
}
