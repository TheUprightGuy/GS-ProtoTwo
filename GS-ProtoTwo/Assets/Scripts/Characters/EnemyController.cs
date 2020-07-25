﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseCharacterClass
{
    public void Punch()
    {
        inAction = true;

        DamageEnemy();
        FinishedTask();
    }

    public override void Attack(BaseCharacterClass _user, BaseCharacterClass _tar)
    {
        target = _tar;
        ActionsList(() => MoveToTarget(target), () => Punch(), () => MoveToStart());
    }

    public override void TakeDamage(int _damage)
    {
        //Debug.Log(name + " took " + _damage + " damage!");
        health -= _damage;
        //CombatController.instance.UpdateStatus()
    }
    public override void SpendMana(int _mana)
    {
        //Debug.Log(name + " spent " + _mana + " mana.");
    }
}
