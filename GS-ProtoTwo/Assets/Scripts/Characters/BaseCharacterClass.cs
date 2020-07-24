﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public enum CharacterType
{
    Enemy,
    Player,
    NPC,
}

public abstract class BaseCharacterClass : MonoBehaviour
{
    [Header("Character Stats")]
    public int speed;
    public float health;
    // temp
    public float damage;
    public CharacterType characterType;
    [Header("Debug")]
    public int id;
    public bool activeTurn = false;
    public BaseCharacterClass target;

    // Public is necessary due to Inheritance
    [HideInInspector] public TurnBillboard turnIndicator;
    [HideInInspector] public TargetBillboard targetIndicator;
    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public NavMeshAgent navAgent;
    [HideInInspector] public Animator animator;

    #region Setup
    private void Awake()
    {
        myQueue = new List<Action>();
        turnIndicator = GetComponentInChildren<TurnBillboard>();
        targetIndicator = GetComponentInChildren<TargetBillboard>();

        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        startPosition = transform.position;
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
        myQueue.RemoveAt(0);
    }

    public void FinishedTask()
    {
        inAction = false;
        NextTask();
    }
    public void EndTurn()
    {
        CombatController.instance.NextTurn();
        FinishedTask();
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
        CombatController.instance.setTarget += ToggleTarget;
        CombatController.instance.setTurn += SetTurn;
    }

    private void OnDestroy()
    {
        CombatController.instance.setTarget -= ToggleTarget;
        CombatController.instance.setTurn -= SetTurn;
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

        navAgent.SetDestination(_enemy.transform.position);
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

            // temp
            if (Input.GetKeyDown(KeyCode.Return))
            {
                CombatController.instance.Confirm();
                turnIndicator.Toggle(false);
                FinishedTask();
            }
        }
    }

    public void SetTurn(int _id)
    {
        activeTurn = (id == _id) ? true : false;
        turnIndicator.Toggle(activeTurn);
        if (activeTurn && characterType == CharacterType.Enemy)
        {
            // temp
            turnIndicator.Toggle(false);

            Attack(target);
        }
    }

    public void DamageEnemy()
    {
        // This will be switched w/ ability damage or a range or something idk
        target.TakeDamage(damage);
    }

    public void TakeDamage(float _damage)
    {
        Debug.Log(name + " took " + _damage + " damage!");
        health -= _damage;
    }

    public virtual void Attack(BaseCharacterClass _enemy)
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

    public void ChooseTarget()
    {
        inAction = true;

        CombatController.instance.combatState = CombatState.targetState;
        CombatController.instance.currentTarget = 0;
        CombatController.instance.SetTarget(CombatController.instance.currentTarget);
    }
}
