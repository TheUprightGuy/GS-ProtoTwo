using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBossController : EnemyController
{
    public Element currentElement = Element.Fire;
    public int currentTurn = 0;
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

    public void NormalAttack(BaseCharacterClass _tar)
    {
        // Attack
        Attack(this, _tar);
    }


    public void ChangeElement()
    {
        // Go To Next Element
        currentElement = (Element)(((int)currentElement + 1) % 4);
        // Set Weakness & Resistance
        stats.resistance = currentElement;
        stats.weakness = (Element)(((int)currentElement + 1) % 4);
        // Change Material Color Here

        // Play Animation Here

        ActionsList();
    }

    public void CastSpell(BaseCharacterClass _tar)
    {
        foreach(Magic n in stats.spells)
        {
            if (n.element == currentElement)
            {
                Magic(this, _tar);
                n.Use(this, _tar);
                // Cast this spell
            }
        }

        // Change Material Color Here
        stats.weakness = Element.None;
    }
}
