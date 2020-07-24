using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseCharacterClass
{
    public void Punch(BaseCharacterClass _enemy)
    {
        inAction = true;

        target = _enemy;
        animator.SetTrigger("Punch");
    }

    public override void Attack(BaseCharacterClass _enemy)
    {
        ActionsList(()=>ChooseTarget(), ()=>MoveToTarget(_enemy), ()=>Punch(_enemy), ()=>MoveToStart());
    }
}
