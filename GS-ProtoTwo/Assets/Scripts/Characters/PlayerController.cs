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

    public override void Attack()
    {
        ActionsList(()=>ChooseTarget(), ()=>MoveToTarget(target), ()=>Punch(), ()=>MoveToStart());
    }
}
