using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Actions/Inventory")]
public class Inventory : ScriptableObject
{
    public List<Item> items;
}
