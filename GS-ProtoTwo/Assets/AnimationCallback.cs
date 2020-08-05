using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallback : MonoBehaviour
{
    public TreeBossController boss;

    public void DealDamage()
    {
        boss.DEALDAMAGE();
    }

    public void FinishedTask()
    {
        boss.FinishedTask();
    }

    public void Die()
    {
        boss.Finish();
    }
}
