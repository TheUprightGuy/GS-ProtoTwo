using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseCharacterClass
{
    public void Punch()
    {
        inAction = true;

        animator.SetTrigger("Punch");
    }

    public override void Attack(BaseCharacterClass _user, BaseCharacterClass _tar)
    {
        //Debug.Log(_user.name + " attacked " + _tar.name + " dealing " + stats.damage + " damage.");

        target = _tar;
        ActionsList(()=>MoveToTarget(target), ()=>Punch(), ()=>MoveToStart());
    }

    public override void Magic(BaseCharacterClass _user, BaseCharacterClass _tar)
    {
        //Debug.Log("used magic on " + _tar.name);

        target = _tar;
        ActionsList(()=>HoldPriority(this));
    }
    public override void Ability(BaseCharacterClass _user, BaseCharacterClass _tar)
    {
        //Debug.Log("used ability on " + _tar.name);

        target = _tar;
        ActionsList();
    }
    public override void Item(BaseCharacterClass _user, BaseCharacterClass _tar)
    {
        Debug.Log("This does nothing currently.");

        target = _tar;
        ActionsList();
    }

    public override void SpendMana(int _mana)
    {
        //Debug.Log(name + " spent " + _mana + " mana.");
        stats.mana -= _mana;
        CombatController.instance.UpdateStatus(this);
    }
}
