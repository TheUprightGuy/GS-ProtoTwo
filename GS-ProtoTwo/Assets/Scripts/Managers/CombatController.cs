using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatState
{ 
    menuState,
    targetingState,
    actionState,
    enemyState,
}


public class CombatController : MonoBehaviour
{
    #region Singleton
    public static CombatController instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one CombatController exists!");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion Singleton

    [Header("Debug Fields")]
    public PlayerController player;
    private EnemyController enemy;
    public CombatState combatState;
    public GameObject enemyPrefab;

    private int activeTurn = 0;
    public int currentTarget = 0;
    public int prevTarget = 0;

    //[HideInInspector] 
    public List<EnemyController> enemies;
    //[HideInInspector] 
    public List<PlayerController> players;

    //[HideInInspector] 
    public List<BaseCharacterClass> turnOrder;

    private void Start()
    {
        SetupBattle();
        GetTurnOrder();

        Invoke("StartBattle", 0.1f);
    }

    private void Update()
    {
        switch (combatState)
        {
            case CombatState.targetingState:
            {
                TargetNavigation();
                break;
            }
            default:
            {
                break;
            }       
        }
    }

    public void ChangeState(CombatState _state)
    {
        combatState = _state;

        switch (combatState)
        {
            case CombatState.menuState:
            {
                ToggleActionCanvas(true);
                break;
            }
            case CombatState.targetingState:
            {
                break;
            }
            case CombatState.actionState:
            {
                ToggleActionCanvas(false);
                break;
            }
            default:
            {
                ToggleActionCanvas(false);
                break;
            }
        }
    }

    // Instantiate Enemies
    public void SetupBattle()
    {
        // temp
        EnemyController temp = Instantiate(enemyPrefab,transform.position, transform.rotation).GetComponent<EnemyController>();
        temp.speed = 3;
        enemies.Add(temp);
        EnemyController temp2 = Instantiate(enemyPrefab, transform.position, transform.rotation).GetComponent<EnemyController>();
        enemies.Add(temp2);

        players.Add(player);

        foreach (BaseCharacterClass n in players)
        {
            turnOrder.Add(n);
        }
        foreach (BaseCharacterClass n in enemies)
        {
            // temp
            n.target = players[0];
            turnOrder.Add(n);
        }
        
        // temp
        enemy = enemies[0];
    }

    public void GetTurnOrder()
    {
        // Sort by Speed
        turnOrder.Sort((a, b) => { return a.speed.CompareTo(b.speed); });
        turnOrder.Reverse();

        // Check Turn IDs
        for (int i = 0; i < turnOrder.Count; i++)
        {
            //Debug.Log(turnOrder[i].name + " turn is #" + i + " with " + turnOrder[i].speed + " speed.");
            turnOrder[i].id = i;
        }
    }

    public void StartBattle()
    {
        activeTurn = 0;
        SetTurn(activeTurn);
    }

    public void Attack()
    {      
        turnOrder[activeTurn].Attack(enemy);       
    }

    // This will be used to manage turn order
    public void NextTurn()
    {
        activeTurn = (activeTurn < turnOrder.Count - 1) ? (activeTurn + 1) : 0;

        SetTurn(activeTurn);
    }

    public void Confirm()
    {
        ChangeState(CombatState.actionState);
        // temp
        SetTarget(-1);
    }

    public void TargetNavigation()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            currentTarget = (currentTarget > 0 ? currentTarget - 1 : enemies.Count - 1);
            SetTarget(enemies[currentTarget].id);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            currentTarget = (currentTarget < enemies.Count - 1 ? currentTarget + 1 : 0);
            SetTarget(enemies[currentTarget].id);
        }
    }

    #region Callbacks
    public event Action<int> setTurn;
    public void SetTurn(int _id)
    {
        if (setTurn != null)
        {
            setTurn(_id);
        }
    }

    public event Action<int> setTarget;
    public void SetTarget(int _id)
    {
        if (setTarget != null)
        {
            setTarget(_id);
        }
    }

    public event Action<bool> toggleActionCanvas;
    public void ToggleActionCanvas(bool _toggle)
    {
        if (toggleActionCanvas != null)
        {
            toggleActionCanvas(_toggle);
        }
    }
    #endregion Callbacks
}
