using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element
{
    Fire,
    Water,
    Lightning,
    Earth,
    Holy
}

[CreateAssetMenu(fileName = "Magic", menuName = "Actions/Magic")]
public class Magic : ScriptableObject
{
    public int damage;
    public int manaCost;
    public bool offensive;
    public Element element;
    public Projectile spellPrefab;

    public void SpendMana(BaseCharacterClass _user)
    {
        _user.SpendMana(manaCost);
    }
    
    public void Use(BaseCharacterClass _user, BaseCharacterClass _tar)
    {
        SpendMana(_user);
        _user.Magic(_user, _tar);

        if (spellPrefab)
        {
            Projectile temp = Instantiate<Projectile>(spellPrefab, _user.transform.position, _user.transform.rotation);
            temp.Setup(this);
            temp.Seek(_tar);
        }
        else
        {
            Debug.Log(_user.name + " spent " + manaCost + " mana to cast " + this.name + " on " + _tar.name);
            _tar.TakeDamage(damage);
        }
    }
}
