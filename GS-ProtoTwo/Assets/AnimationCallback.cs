using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallback : MonoBehaviour
{
    // SLOPPY AF BUT UGH
    public TreeBossController boss;
    public EnemyController enemy;

    public void DealDamage()
    {
        if (boss)
        {
            boss.DEALDAMAGE();
        }
        else if (enemy)
        {
            enemy.DamageEnemy();
        }
    }

    public void FinishedTask()
    {
        if (boss)
        {
            boss.FinishedTask();
        }
        else if (enemy)
        {
            enemy.FinishedTask();
        }
    }

    public void Die()
    {
        boss.Finish();
    }

}
