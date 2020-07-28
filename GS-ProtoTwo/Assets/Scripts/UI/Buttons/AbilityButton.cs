using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public Ability ability;
    public PlayerController player;

    public void Setup(PlayerController _player, Ability _ability)
    {
        text.SetText(_ability.name);
        player = _player;
        ability = _ability;
    }

    public void OnUse()
    {
        CombatController.instance.ChooseTarget(player, ability.Use, ability.offensive);
    }
}
