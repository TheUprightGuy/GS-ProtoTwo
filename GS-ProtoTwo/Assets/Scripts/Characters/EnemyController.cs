using System.Collections;
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

    public override void Attack()
    {
        ActionsList(() => MoveToTarget(target), () => Punch(), () => MoveToStart());
    }
}
