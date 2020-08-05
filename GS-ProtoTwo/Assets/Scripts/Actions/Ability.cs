using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Actions/Ability")]
public class Ability : ScriptableObject
{
    public bool offensive;
    public int damage;
    public Element element;
    public List<string> swingSounds;
  
    public void Use(BaseCharacterClass _user, BaseCharacterClass _tar)
    {
        AudioManager.instance.PlaySound(swingSounds[Random.Range(0,1)]);
        // queue up anim?
        _user.Ability(_user, _tar);

        Debug.Log(_user.name + " used " + this.name + " on " + _tar.name + " dealing " + damage + " damage.");
        _tar.TakeDamage(damage + _user.stats.attack * 3, element);
    }
}
