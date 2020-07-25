using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element
{
    Fire,
    Water,
    Lightning
}

[CreateAssetMenu(fileName = "Magic", menuName = "Actions/Magic")]
public class Magic : ScriptableObject
{
    public int damage;
    public int manaCost;
    public Element element;

    public void SpendMana(BaseCharacterClass _user)
    {
        _user.SpendMana(manaCost);
    }
    
    public void Use(BaseCharacterClass _user, BaseCharacterClass _tar)
    {
        // temp - Include Elemental Type
        // create proj here for example
        // queue up anim?
        SpendMana(_user);
        _user.Magic(_user, _tar);

        Debug.Log(_user.name + " spent " + manaCost + " mana to cast " + this.name + " on " + _tar.name + " dealing " + damage + " " + element.ToString() + " damage.");
        _tar.TakeDamage(damage);
    }
}
