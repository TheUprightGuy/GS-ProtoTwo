using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public TMPro.TextMeshProUGUI nameText;
    public TMPro.TextMeshProUGUI quantityText;
    public Item item;
    public PlayerController player;

    public void Setup(PlayerController _player, Item _item)
    {
        nameText.SetText(_item.name);
        quantityText.SetText(_item.quantity.ToString());
        player = _player;
        item = _item;

        GameplayUIScript.instance.updateItemQuantity += UpdateQuantity;
    }

    private void OnDestroy()
    {
        GameplayUIScript.instance.updateItemQuantity -= UpdateQuantity;
    }

    public void OnUse()
    {
        CombatController.instance.ChooseTarget(player, item.Use, item.offensive);
    }

    public void UpdateQuantity(Item _item)
    {
        if (item == _item)
        {
            quantityText.SetText(item.quantity.ToString());
        }
    }
}
