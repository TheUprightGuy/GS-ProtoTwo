using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "Player/Stats")]
public class Stats : ScriptableObject
{
    [Header("Name & Type")]
    new public string name;
    public CharacterType characterType;

    [Header("Attributes")]
    public int attack;
    public int defense;
    public int speed;
    public int magic;
    public int hp;
    public int mp;

    public int maxHealth;  
    public int maxMana;
    public int damage;

    public int baseHealth;
    public int baseMana;

    [Header("Skills List")]
    public List<Magic> spells;
    public List<Ability> abilities;

    //[HideInInspector]
    public int mana;
    //[HideInInspector] 
    public int health;

    public bool linked;

    public void Setup()
    {
        maxHealth = baseHealth;
        health = maxHealth;
        maxMana = baseMana;
        mana = maxMana;
    }

    public void AddStats(Stats _stats)
    {
        if (_stats.linked)
        {
            attack += _stats.attack;
            defense += _stats.defense;
            speed += _stats.speed;
            magic += _stats.magic;
            hp += _stats.hp;
            mp += _stats.mp;

            foreach (Magic n in _stats.spells)
            {
                if (!spells.Contains(n))
                {
                    spells.Add(n);
                }
            }

            foreach (Ability n in _stats.abilities)
            {
                if (!abilities.Contains(n))
                {
                    abilities.Add(n);
                }
            }
        }
    }

    public void Multiply(Stats _stats, int _links)
    {
        attack  = _stats.attack * _links;
        magic   = _stats.magic * _links;
        hp      = _stats.hp * _links;
        mp      = _stats.mp * _links;
        defense = _stats.defense * _links;
        speed   = _stats.speed * _links;

        if (_links > 0)
        {
            linked = true;
        }
    }

    public void Clear()
    {
        attack = 0;
        magic = 0;
        hp = 0;
        mp = 0;
        defense = 0;
        speed = 0;

        spells.Clear();
        abilities.Clear();
    }

    public void SetupHPMP()
    {
        if (maxHealth == 0 || maxMana == 0)
        {
            Debug.LogError("MaxHealth/Mana should never be Zero!");
        }

        float hpPercent = ((float)health / (float)maxHealth);
        float mpPercent = ((float)mana / (float)maxMana);

        // Probably be careful where we call this.
        // Currently adds a base of 100hp/mp + stats * 100
        maxHealth = baseHealth + (hp * 100);
        maxMana = baseMana + (mp * 100);

        health = (int)(maxHealth * hpPercent);
        mana = (int)(maxMana * mpPercent);
        damage = attack * 10;
    }
}
