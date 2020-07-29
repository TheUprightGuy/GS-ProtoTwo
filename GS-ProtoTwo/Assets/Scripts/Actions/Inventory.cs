using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Actions/Inventory")]
public class Inventory : ScriptableObject
{
    public List<Item> items;

    public void AddItem(Item _item)
    {
        if (_item != null)
        {
            foreach (Item n in items)
            {
                // temp, add a name to compare to
                if (n.healthRestored == _item.healthRestored)
                {
                    n.quantity += _item.quantity;
                    return;
                }
            }

            items.Add(_item);
            return;
        }

        Debug.LogError("NOPE");
    }

    public void Setup()
    {
        if (items == null)
        {
            items = new List<Item>();
        }

        for (int i = 0; i < items.Count; i++)
        {
            items[i] = Instantiate(items[i]);
        }
    }
}
