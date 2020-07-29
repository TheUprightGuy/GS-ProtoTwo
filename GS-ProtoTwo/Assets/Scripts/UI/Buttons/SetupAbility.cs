using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupAbility : MonoBehaviour
{
    public GameObject abilityButtonPrefab;

    public void AddAbilityButton(PlayerController _player, Ability _ability)
    {
        AbilityButton temp = Instantiate(abilityButtonPrefab, this.transform).GetComponent<AbilityButton>();
        temp.Setup(_player, _ability);
    }
}
