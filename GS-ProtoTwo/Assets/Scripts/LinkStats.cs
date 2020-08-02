using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stats2
{
    public int attack;
    public int magic;
    public int health;
    public int mana;
    public int defense;
    public int speed;

    public void Multiply(int _links)
    {
        attack *= _links;
        magic *= _links;
        health *= _links;
        mana *= _links;
        defense *= _links;
        speed *= _links;
    }

    public void AddStats(Stats2 _stats)
    {
        attack += _stats.attack;
        magic += _stats.magic;
        health += _stats.health;
        mana += _stats.mana;
        defense += _stats.defense;
        speed += _stats.speed;
    }

    public void Clear()
    {
        attack = 0;
        magic = 0;
        health = 0;
        mana = 0;
        defense = 0;
        speed = 0;
    }
}


public class LinkStats : MonoBehaviour
{
    public Stats2 stats;
    [HideInInspector] public Stats2 returnStats;

    private void Start()
    {
        returnStats = stats;

        SkillTreeManager.instance.updateStats += UpdateStats;
    }

    private void OnDestroy()
    {
        SkillTreeManager.instance.updateStats -= UpdateStats;
    }

    public void GetVal(int _links)
    {
        returnStats = stats;
        returnStats.Multiply(_links);
    }

    public void UpdateStats()
    {
        SkillTreeManager.instance.stats.AddStats(returnStats);
    }
}
