using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupMagic : MonoBehaviour
{
    public GameObject magicButtonPrefab;
    
    public void AddMagicButton(PlayerController _player, Magic _spell)
    {
        SpellButton temp = Instantiate(magicButtonPrefab, this.transform).GetComponent<SpellButton>();
        temp.Setup(_player, _spell);
    }
}
