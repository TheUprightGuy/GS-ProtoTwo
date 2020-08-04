using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Actions/Ability")]
public class Ability : ScriptableObject
{
    public bool offensive;
    public int damage;
    public Element element;
  
    public void Use(BaseCharacterClass _user, BaseCharacterClass _tar)
    {
        // queue up anim?
        _user.Ability(_user, _tar);

        Debug.Log(_user.name + " used " + this.name + " on " + _tar.name + " dealing " + damage + " damage.");
        _tar.TakeDamage(damage + _user.stats.attack * 3, element);
    }
}
