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
        health.fillAmount = (float)player.health / (float)player.maxHealth;
        healthText.SetText(player.health + "/" + player.maxHealth);       
    }

    public void UpdateMana()
    {
        mana.fillAmount = (float)player.mana / (float)player.maxMana;
        manaText.SetText(player.mana + "/" + player.maxMana);
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
