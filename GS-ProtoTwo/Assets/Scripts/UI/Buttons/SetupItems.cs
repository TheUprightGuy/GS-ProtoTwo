using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupItems : MonoBehaviour
{
    public GameObject itemButtonPrefab;

    public void AddItemButton(PlayerController _player, Item _item)
    {
        ItemButton temp = Instantiate(itemButtonPrefab, this.transform).GetComponent<ItemButton>();
        temp.Setup(_player, _item);
    }
}
