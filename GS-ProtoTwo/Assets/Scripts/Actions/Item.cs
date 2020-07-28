using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Actions/Item")]
public class Item : ScriptableObject
{
    public int healthRestored;
    public int manaRestored;
    public bool offensive;
    public int quantity;
  
    public void Use(BaseCharacterClass _user, BaseCharacterClass _tar)
    {
        _user.Item(_user, _tar);

        _tar.RestoreHealth(healthRestored);
        _tar.RestoreMana(manaRestored);

        quantity -= 1;
        if (quantity <= 0)
        {
            // Hide
        }

        GameplayUIScript.instance.UpdateItemQuantity(this);
    }
}
