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

    public override void Attack(BaseCharacterClass _tar)
    {
        target = _tar;
        ActionsList(()=>MoveToTarget(target), ()=>Punch(), ()=>MoveToStart());
    }
}
