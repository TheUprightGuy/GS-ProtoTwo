﻿using System.Collections;
using System.Collections.Generic;
using System;
using Audio;
using UnityEngine;
using UnityEngine.AI;

public enum CharacterType
{
    Enemy,
    Player,
    NPC,
}

public delegate void ActionDelegate(BaseCharacterClass _user, BaseCharacterClass _tar);

public abstract class BaseCharacterClass : MonoBehaviour
{
    [Header("Character Stats")]
    public Stats stats;
    public Inventory inventory;
    // Debug
    public bool alive = true;
    [HideInInspector] public int id;
    [HideInInspector] public bool activeTurn = false;
    [HideInInspector] public BaseCharacterClass target;


    public ActionDelegate actionDelegate;

    // TEMPORARY SHIT
    public Transform targetPoint;
    public Transform navToPoint;
    public Quaternion baseRot;

    // Public is necessary due to Inheritance
    [HideInInspector] public TurnBillboard turnIndicator;
    [HideInInspector] public TargetBillboard targetIndicator;
    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public NavMeshAgent navAgent;
    [HideInInspector] public Animator animator;

    public AnimController animController;
    public GameObject hitFX;
    public GameObject critFX;

    #region Setup
    private void Awake()
    {
        myQueue = new List<Action>();
        turnIndicator = GetComponentInChildren<TurnBillboard>();
        targetIndicator = GetComponentInChildren<TargetBillboard>();

        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        startPosition = transform.position;
        baseRot = transform.rotation;
    }
    #endregion
    #region ActionQueuing
    public List<Action> myQueue;
    public bool inAction = false;
    public void DoWork()
    {
        myQueue[0]();
    }

    public void NextTask()
    {
        if (myQueue.Count > 0)
        {
            myQueue.RemoveAt(0);
        }
    }

    public void FinishedTask()
    {
        inAction = false;
        NextTask();
    }
    public void EndTurn()
    {
        // temp
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;
        transform.rotation = baseRot;

        FinishedTask();
        CombatController.instance.NextTurn();
    }

    public void ActionsList(params Action[] _actions)
    {
        foreach (Action n in _actions)
        {
            myQueue.Add(n);
        }

        myQueue.Add(() => EndTurn());
    }
    #endregion ActionQueuing
    #region Callbacks
    private void Start()
    {
        if (GetComponent<EnemyController>())
        {
            stats.Setup();
            stats.SetupHPMP();
        }

        animController = GetComponentInChildren<AnimController>();

        CombatController.instance.toggleTurn += TurnOffTurn;
        CombatController.instance.setTarget += ToggleTarget;
        CombatController.instance.setTurn += SetTurn;
        CombatController.instance.passPriority += PassPriority;
    }

    private void OnDestroy()
    {
        CombatController.instance.toggleTurn -= TurnOffTurn;
        CombatController.instance.setTarget -= ToggleTarget;
        CombatController.instance.setTurn -= SetTurn;
        CombatController.instance.passPriority -= PassPriority;
    }
    #endregion Callbacks
    #region Navigation
    public void MoveToStart()
    {
        inAction = true;

        navAgent.SetDestination(startPosition);
    }

    public void MoveToTarget(BaseCharacterClass _enemy)
    {
        inAction = true;
        // BADBADBADBABD
        navAgent.isStopped = false;

        if (_enemy.navToPoint)
        {
            navAgent.SetDestination(_enemy.navToPoint.position);
        }
        else
        {
            navAgent.SetDestination(_enemy.transform.position);
        }
    }
    #endregion Navigation

    private void Update()
    {
        if (activeTurn)
        {
            // Check Action Queue
            if (myQueue.Count != 0 && !inAction)
            {
                DoWork();
            }

            // Path Completed -> Move to Next Task
            if (navAgent.hasPath && navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                inAction = false;

                navAgent.ResetPath();

                NextTask();
            }
            if (animController)
            {
                // HORRID
                if (GetComponent<TreeBossController>())
                {
                    animController.animator.SetBool("Forward", navAgent.hasPath);
                }
                else if (GetComponent<EnemyController>())
                {
                    animController.animator.SetBool("Moving", navAgent.hasPath);
                }
            }
        }
    }

    public void SetTurn(int _id)
    {
        activeTurn = (id == _id) ? true : false;
        turnIndicator.Toggle(activeTurn);
        if (activeTurn && stats.characterType == CharacterType.Enemy)
        {
            // temp
            CombatController.instance.ChangeState(CombatState.ENEMYTURN);
            turnIndicator.Toggle(false);

            if (!CombatController.instance.CheckPlayers())
            {
                if (GetComponent<TreeBossController>())
                {
                    GetComponent<TreeBossController>().TakeTurn(CombatController.instance.GetTarget());
                }
                else
                {
                    ChooseCommand();
                    // Attack(this, CombatController.instance.GetTarget());
                }
            }
            else
            {
                Debug.Log("No Players Left to Target");
            }
        }
        else if (activeTurn && stats.characterType == CharacterType.Player)
        {
            navAgent.isStopped = false;
            CombatController.instance.ChangeState(CombatState.PLAYERTURN);
        }
    }

    public void ChooseCommand()
    {
        bool chosen = false;

        while (!chosen)
        {
            int action = UnityEngine.Random.Range(0, 3);

            switch (action)
            {
                // Attack
                case 0:
                {
                    chosen = true;
                    Attack(this, CombatController.instance.GetTarget());
                    break;
                }
                // Ability
                case 1:
                {
                    if (stats.abilities.Count > 0)
                    {
                        chosen = true;
                        int rand = UnityEngine.Random.Range(0, stats.abilities.Count);
                        Ability(this, CombatController.instance.GetTarget());
                        stats.abilities[rand].Use(this, CombatController.instance.GetTarget());
                    }
                    break;
                }
                // Spells
                case 2:
                {
                    if (stats.spells.Count > 0)
                    {
                        chosen = true;
                        int rand = UnityEngine.Random.Range(0, stats.spells.Count);
                        Magic(this, CombatController.instance.GetTarget());
                        stats.spells[rand].Use(this, CombatController.instance.GetTarget());
                    }
                    break;
                }
            }
        }
    }

    public void TurnOffTurn()
    {
        turnIndicator.Toggle(false);
    }

    public void DamageEnemy()
    {
        AudioManager.instance.PlaySound("growl");
        // This will be switched w/ ability damage or a range or something idk
        target.TakeDamage(stats.damage, Element.None);
        CreateHitFX();
    }

    public virtual void TakeDamage(int _damage, Element _element)
    {
        if(_damage>0) AudioManager.instance.PlaySound("enemyDamage2");
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

        //_damage -= stats.defense * 3;

        // Change this to percentage later
        stats.health -= _damage;

        if (stats.characterType == CharacterType.Player)
        {
            CombatController.instance.UpdateStatus((PlayerController)this);
        }
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

    public void Die()
    {
        alive = false;
        // Do death stuff here
        // TEMP PLEASE REPLACE THIS
        if (this.GetComponent<EnemyController>() || this.GetComponent<TreeBossController>())
        {
            Destroy(gameObject);
        }
        else
        {
            navAgent.enabled = false;
            transform.Translate(new Vector3(0, -1000, 0));
        }

        CombatController.instance.TurnOffTarget(id);
        CombatController.instance.CheckRemainingCharacters();
    }


    public virtual void SpendMana(int _mana)
    {
        //Debug.Log(name + " spent " + _mana + " mana.");
    }

    public void HoldPriority(BaseCharacterClass _character)
    {
        if (this == _character)
        {
            inAction = true;
        }
    }

    public void PassPriority(BaseCharacterClass _character)
    {
        if (this == _character)
        {
            FinishedTask();
        }
    }

    public virtual void Attack(BaseCharacterClass _user, BaseCharacterClass _tar)
    {
        Debug.Log("using virtual for some reason");
    }

    public virtual void Magic(BaseCharacterClass _user, BaseCharacterClass _tar)
    {
        Debug.Log("using virtual for some reason");
    }
    public virtual void Ability(BaseCharacterClass _user, BaseCharacterClass _tar)
    {
        Debug.Log("using virtual for some reason");
    }
    public virtual void Item(BaseCharacterClass _user, BaseCharacterClass _tar)
    {
        Debug.Log("using virtual for some reason");
    }

    public void ToggleTarget(int _id)
    {
        if (id == _id)
        {
            targetIndicator.Toggle(true);
        }
        else
        {
            targetIndicator.Toggle(false);
        }
    }

    public void RestoreHealth(int _health)
    {
        stats.health = (stats.health + _health > stats.maxHealth) ? stats.maxHealth : stats.health + _health;
        if (stats.characterType == CharacterType.Player)
        {
            CombatController.instance.UpdateStatus((PlayerController)this);
        }
    }
    public void RestoreMana(int _mana)
    {
        stats.mana = (stats.mana + _mana > stats.maxMana) ? stats.maxMana : stats.mana + _mana;
        if (stats.characterType == CharacterType.Player)
        {
            CombatController.instance.UpdateStatus((PlayerController)this);
        }
    }

    public void CreateHitFX()
    {
        if (hitFX)
        {
            if (target.targetPoint)
            {
                // If crit use crit
                Instantiate(hitFX, target.targetPoint);
            }
            else
            {
                // If crit use crit
                Instantiate(hitFX, target.transform);
            }
        }
    }
}
