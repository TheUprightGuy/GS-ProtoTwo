using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellButton : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public Magic spell;
    public PlayerController player;

    public void Setup(PlayerController _player, Magic _spell)
    {
        text.SetText(_spell.name);
        player = _player;
        spell = _spell;
    }

    public void OnUse()
    {
        CombatController.instance.ChooseTarget(player, spell.Use, spell.offensive);
    }
}
