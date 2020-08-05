using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBossController : EnemyController
{
    public Element currentElement = Element.Fire;
    public int currentTurn = 0;
    public PulsingLight pulsingLight;


    public void TakeTurn(BaseCharacterClass _tar)
    {
        switch(currentTurn)
        {
            case 0:
            {
                NormalAttack(_tar);
                break;
            }
            case 1:
            {
                ChangeElement();
                break;
            }
            case 2:
            {
                CastSpell(_tar);
                break;
            }
        }
        currentTurn = (currentTurn + 1) % 3;
    }

    public void BigPunch(BaseCharacterClass _tar)
    {
        inAction = true;
        target = _tar;

        animController.AttackAnim();
    }

    public void DEALDAMAGE()
    {
        // This will be switched w/ ability damage or a range or something idk
        target.TakeDamage(stats.damage, Element.None);
    }

    public void NormalAttack(BaseCharacterClass _tar)
    {
        // Attack
        target = _tar;
        ActionsList(() => BigPunch(_tar));
    }


    public void ChangeElement()
    {
        // Go To Next Element
        currentElement = (Element)(((int)currentElement + 1) % 4);
        // Set Weakness & Resistance
        stats.resistance = currentElement;
        stats.weakness = (Element)(((int)currentElement + 1) % 4);
        // Change Material Color Here
        pulsingLight.ChangeColor(currentElement);
        // Play Animation Here
        animController.MagicAnim();
        ActionsList(()=>HoldPriority(this));
    }

    public void CastSpell(BaseCharacterClass _tar)
    {
        foreach(Magic n in stats.spells)
        {
            if (n.element == currentElement)
            {
                //Magic(this, _tar);
                n.Use(this, _tar);
                // Cast this spell
            }
        }

        // Change Material Color Here
        stats.weakness = Element.Holy;
        pulsingLight.ChangeColor(Element.Holy);
    }

    public void Die()
    {
        alive = false;
        // Do death stuff here
        // TEMP PLEASE REPLACE THIS
        animController.DeathAnim();

        CombatController.instance.TurnOffTarget(id);
        //CombatController.instance.CheckRemainingCharacters();
    }

    public void Finish()
    {
        Destroy(gameObject);
        CombatController.instance.CheckRemainingCharacters();
    }

    public override void TakeDamage(int _damage, Element _element)
    {
        // Double/Halve Damage based on Resistance/Weakness
        if (_element == stats.weakness)
        {
            _damage *= 2;
        }
        if (_element == stats.resistance)
        {
            _damage /= 2;
        }

        float armor = 100 / (100 + ((float)stats.defense * 3));
        _damage = Mathf.FloorToInt((float)_damage * armor);

        // Change this to percentage later
        stats.health -= _damage;

        animController.FlinchAnim();

        // Change this to percentage later
        stats.health -= _damage;

        if (stats.health <= 0)
        {
            Die();
        }
        if (stats.health > stats.maxHealth)
        {
            stats.health = stats.maxHealth;
        }

        Debug.Log(this.name + " took " + _damage + " damage!");
    }
}
