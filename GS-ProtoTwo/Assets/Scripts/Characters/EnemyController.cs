using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseCharacterClass
{
    public void Punch(BaseCharacterClass _enemy)
    {
        inAction = true;
        target = _enemy;

        DamageEnemy();
        FinishedTask();
    }

    public override void Attack(BaseCharacterClass _enemy)
    {
        ActionsList(() => MoveToTarget(_enemy), () => Punch(_enemy), () => MoveToStart());
    }
}
