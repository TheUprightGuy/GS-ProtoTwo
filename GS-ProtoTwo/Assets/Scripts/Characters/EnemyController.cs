using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseCharacterClass
{
    public Item rewardItem;

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

    public override void SpendMana(int _mana)
    {
        //Debug.Log(name + " spent " + _mana + " mana.");
    }

    public Item GiveReward()
    {
        if (rewardItem)
        {
            Item temp = Instantiate(rewardItem);
            temp.quantity = 1;
            return (temp);
        }

        return (null);
    }
}
