using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseCharacterClass : MonoBehaviour
{
    [Header("Character Stats")]
    public int speed;
    public float health;
    // temp
    public float damage;
    [Header("Debug")]
    public int id;
    public bool activeTurn = false;
    public BaseCharacterClass target;

    // Public is necessary due to Inheritance
    [HideInInspector] public Billboard turnIndicator;
    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public NavMeshAgent navAgent;
    [HideInInspector] public Animator animator;

    #region Setup
    private void Awake()
    {
        myQueue = new List<Action>();
        turnIndicator = GetComponentInChildren<Billboard>();

        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        startPosition = transform.position;
    }
    #endregion
    #region ActionQueuing
    public List<Action> myQueue;
    public bool inAction = false;
    public bool moving = false;
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
        turnIndicator.ToggleTurn(false);
    }
    #endregion ActionQueuing
    #region Callbacks
    private void Start()
    {
        
        CombatController.instance.setTurn += SetTurn;
    }

    private void OnDestroy()
    {
        CombatController.instance.setTurn -= SetTurn;
    }
    #endregion Callbacks
    #region Navigation
    public void MoveToStart()
    {
        inAction = true;
        moving = true;

        navAgent.SetDestination(startPosition);
    }

    public void MoveToTarget(EnemyController _enemy)
    {
        inAction = true;
        moving = true;

        Vector3 dist = (_enemy.transform.position - transform.position) * 0.2f;
        navAgent.SetDestination(_enemy.transform.position - dist);
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
            if (!navAgent.pathPending && !navAgent.hasPath && moving)
            {
                moving = false;
                inAction = false;

                NextTask();
            }
        }
    }

    public void SetTurn(int _id)
    {
        activeTurn = (id == _id) ? true : false;
        turnIndicator.ToggleTurn(activeTurn);
    }

    public void DamageEnemy()
    {
        // This will be switched w/ ability damage or a range or something idk
        target.TakeDamage(damage);
    }

    public void TakeDamage(float _damage)
    {
        Debug.Log("Took " + _damage + " damage!");
        health -= _damage;
    }

    public virtual void Attack(EnemyController _enemy)
    {
        Debug.Log("using virtual");
    }
}
