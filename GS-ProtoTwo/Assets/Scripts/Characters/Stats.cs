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

    [Header("Leveling")]
    public int level;
    public int currentXP;
    public int nextLevelXP;
    public int pointsToSpend;

    [Header("Enemies Only")]
    public int xpReward;

    [HideInInspector] public int maxHealth;
    [HideInInspector] public int maxMana;
    //[HideInInspector] 
    public int damage;
    //[HideInInspector] 
    public int baseHealth;
    //[HideInInspector] 
    public int baseMana;
    //[HideInInspector] 
    public int mana;
    //[HideInInspector] 
    public int health;

    [Header("Skills List")]
    public List<Magic> spells;
    public List<Ability> abilities;

    [Header("Resistances")]
    public Element weakness;
    public Element resistance;

    [HideInInspector] public bool linked;

    public void Setup()
    {
        level = 1;
        currentXP = 0;
        nextLevelXP = 100;

        maxHealth = baseHealth;
        health = maxHealth;
        maxMana = baseMana;
        mana = maxMana;

        damage = 10;
        pointsToSpend = 3;
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
        damage = 10 + (attack * 10);
    }

    public void GainXP(int _xp)
    {
        currentXP += _xp;
        if (currentXP >= nextLevelXP)
        {
            level++;
            pointsToSpend++;
            currentXP %= nextLevelXP;
            //Play level up sfx
            //Display levelUPOverlay
        }

        nextLevelXP = (int)(nextLevelXP * 1.5f);
    }
}
