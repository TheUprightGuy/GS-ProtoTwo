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
    public int maxHealth;  
    public int maxMana;
    public int speed;
    // temp
    public int damage;
    [Header("Skills List")]
    public List<Magic> spells;
    public List<Ability> abilities;

    [HideInInspector] public int mana;
    [HideInInspector] public int health;
}
