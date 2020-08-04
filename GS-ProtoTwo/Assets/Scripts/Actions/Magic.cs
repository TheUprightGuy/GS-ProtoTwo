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

public enum SpellType
{
    Projectile,
    Targeted,
    Self
}

[CreateAssetMenu(fileName = "Magic", menuName = "Actions/Magic")]
public class Magic : ScriptableObject
{
    public int damage;
    public int manaCost;
    public Element element;
    public SpellType spellType;

    public SpellPrefab spellPrefab;

    public bool offensive;


    public void SpendMana(BaseCharacterClass _user)
    {
        _user.SpendMana(manaCost);
    }
    
    public void Use(BaseCharacterClass _user, BaseCharacterClass _tar)
    {
        SpendMana(_user);
        _user.Magic(_user, _tar);

        switch(spellType)
        {
            case SpellType.Projectile:
            {
                SpellPrefab temp = Instantiate<SpellPrefab>(spellPrefab, _user.transform.position, _user.transform.rotation);
                temp.Setup(this, _tar);
                break;
            }
            case SpellType.Targeted:
            {
                SpellPrefab temp = Instantiate<SpellPrefab>(spellPrefab, _tar.transform.position, _tar.transform.rotation);
                temp.Setup(this, _tar);
                break;
            }
            case SpellType.Self:
            {

                break;
            }
        }
    }
}
