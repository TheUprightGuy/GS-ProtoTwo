using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseCharacterClass
{
    public Item rewardItem;

    public void Punch()
    {
        inAction = true;
        if (animController)
        {
            animController.AttackAnim();
        }


        //DamageEnemy();
        //FinishedTask();
    }

    public override void Attack(BaseCharacterClass _user, BaseCharacterClass _tar)
    {
        target = _tar;
        ActionsList(() => MoveToTarget(target), () => Punch(), () => MoveToStart());
    }
    public override void Magic(BaseCharacterClass _user, BaseCharacterClass _tar)
    {
        //Debug.Log("used magic on " + _tar.name);

        target = _tar;
        ActionsList(() => HoldPriority(this));
    }
    public override void Ability(BaseCharacterClass _user, BaseCharacterClass _tar)
    {
        //Debug.Log("used ability on " + _tar.name);

        target = _tar;
        ActionsList(() => HoldPriority(this));
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
