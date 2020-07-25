using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatState
{ 
    PLAYERTURN,
    ACTION,
    ENEMYTURN,
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
    public GameObject enemyPrefab;
    // Turn Tracking
    public CombatState combatState;
    private int activeTurn = 0;
    // Target Navigation
    [HideInInspector] public int currentTarget = 0;
    [HideInInspector] public List<EnemyController> enemies;
    [HideInInspector] public List<PlayerController> players;
    [HideInInspector] public List<BaseCharacterClass> turnOrder;

    private void Start()
    {
        // temp - REQUIRED CURRENTLY
        Invoke("Setup", 0.01f);
    }

    public void Setup()
    {
        SetupBattle();
        GetTurnOrder();

        Invoke("StartBattle", 0.1f);
    }

    // temp
    public void ChangeState(CombatState _state)
    {
        combatState = _state;

        switch (combatState)
        {
            case CombatState.PLAYERTURN:
            {
                ToggleActionCanvas(true);
                StartTurn();
                break;
            }
            case CombatState.ACTION:
            {
                ToggleTurn();
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
        //temp.speed = 3;
        temp.transform.Translate(new Vector3(2, 0, 0));
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
            // Change this to take in EnemyController once we have more info on there (eg. name)
        }
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
        foreach (BaseCharacterClass n in enemies)
        {
            AddEnemyButton(n.id);
        }
    }

    public void StartBattle()
    {
        activeTurn = 0;
        SetTurn(activeTurn);
    }

    public void NextTurn()
    {
        activeTurn = (activeTurn < turnOrder.Count - 1) ? (activeTurn + 1) : 0;

        SetTurn(activeTurn);
    }

    // Need to Swap the Queued Action instead of Attack
    public void Submit(int _id)
    {
        turnOrder[activeTurn].Attack(turnOrder[_id]);
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

    public event Action toggleTurn;
    public void ToggleTurn()
    {
        if (toggleTurn != null)
        {
            toggleTurn();
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

    public event Action<int> addEnemyButton;
    public void AddEnemyButton(int _id)
    {
        if (addEnemyButton != null)
        {
            addEnemyButton(_id);
        }
    }

    public event Action startTurn;
    public void StartTurn()
    {
        if (startTurn != null)
        {
            startTurn();
        }
    }
    #endregion Callbacks
}
