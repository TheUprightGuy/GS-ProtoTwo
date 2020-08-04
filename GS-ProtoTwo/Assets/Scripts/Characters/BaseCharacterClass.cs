using System.Collections;
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

    public Transform targetPoint;

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
        stats.mana = stats.maxMana;
        stats.health = stats.maxHealth;

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
                Attack(this, CombatController.instance.GetTarget());
            }
            else
            {
                Debug.Log("No Players Left to Target");
            }
        }
        else if (activeTurn && stats.characterType == CharacterType.Player)
        {
            CombatController.instance.ChangeState(CombatState.PLAYERTURN);
        }
    }

    public void TurnOffTurn()
    {
        turnIndicator.Toggle(false);
    }

    public void DamageEnemy()
    {
        // This will be switched w/ ability damage or a range or something idk
        target.TakeDamage(stats.damage);
    }

    public void TakeDamage(int _damage)
    {
        //Debug.Log(name + " took " + _damage + " damage!");
        stats.health -= _damage;

        if (stats.characterType == CharacterType.Player)
        {
            CombatController.instance.UpdateStatus((PlayerController)this);
        }
        if (stats.health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        alive = false;
        // Do death stuff here
        // TEMP PLEASE REPLACE THIS
        navAgent.enabled = false;
        transform.Translate(new Vector3(0, -1000, 0));

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
}
