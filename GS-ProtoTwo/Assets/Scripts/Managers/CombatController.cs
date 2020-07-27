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
    public PlayerController player2;

    public Encounter encounter;
    // Turn Tracking
    public CombatState combatState;
    [HideInInspector] public int activeTurn = 0;
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
        Invoke("GetTurnOrder", 0.1f);
        // temp - REQUIRED CURRENTLY
        Invoke("StartBattle", 0.2f);
    }

    public void ChangeState(CombatState _state)
    {
        combatState = _state;

        switch (combatState)
        {
            case CombatState.PLAYERTURN:
            {
                ToggleActionCanvas(turnOrder[activeTurn], true);
                StartTurn((PlayerController)turnOrder[activeTurn]);
                break;
            }
            case CombatState.ACTION:
            {
                ToggleTurn();
                ToggleActionCanvas(turnOrder[activeTurn], false);
                break;
            }
            default:
            {
                ToggleActionCanvas(turnOrder[activeTurn], false);
                break;
            }
        }
    }

    // Instantiate Enemies
    public void SetupBattle()
    {
        foreach(GameObject n in encounter.enemyPrefabs)
        {
            EnemyController temp = Instantiate(n, transform.position, transform.rotation).GetComponent<EnemyController>();           
            enemies.Add(temp);
        }

        // temp
        enemies[0].transform.Translate(new Vector3(2, 0, 0));

        players.Add(player);
        players.Add(player2);

        foreach (BaseCharacterClass n in players)
        {
            turnOrder.Add(n);
            SetupCanvas((PlayerController)n);
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
        turnOrder.Sort((a, b) => { return a.stats.speed.CompareTo(b.stats.speed); });
        turnOrder.Reverse();

        // Check Turn IDs
        for (int i = 0; i < turnOrder.Count; i++)
        {
            turnOrder[i].id = i;
        }
        // Add a Button for each Target
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
        turnOrder[activeTurn].actionDelegate(turnOrder[activeTurn], turnOrder[_id]);
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

    public event Action<BaseCharacterClass, bool> toggleActionCanvas;
    public void ToggleActionCanvas(BaseCharacterClass _player, bool _toggle)
    {
        if (toggleActionCanvas != null)
        {
            toggleActionCanvas(_player, _toggle);
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

    public event Action<PlayerController> startTurn;
    public void StartTurn(PlayerController _player)
    {
        if (startTurn != null)
        {
            startTurn(_player);
        }
    }

    public event Action<PlayerController, ActionDelegate> chooseTarget;
    public void ChooseTarget(PlayerController _player, ActionDelegate _action)
    {
        if (chooseTarget != null)
        {
            chooseTarget(_player, _action);
        }
    }

    public event Action<BaseCharacterClass> playerRef;
    public void PlayerRef(BaseCharacterClass _player)
    {
        if (playerRef != null)
        {
            playerRef(_player);
        }
    }

    public event Action<PlayerController> setupCanvas;
    public void SetupCanvas(PlayerController _player)
    {
        if (setupCanvas != null)
        {
            setupCanvas(_player);
        }
    }

    public event Action<PlayerController> updateStatus;
    public void UpdateStatus(PlayerController _player)
    {
        if (updateStatus != null)
        {
            updateStatus(_player);
        }
    }

    public event Action<BaseCharacterClass> passPriority;
    public void PassPriority()
    {
        if (passPriority != null)
        {
            passPriority(turnOrder[activeTurn]);
        }
    }
    #endregion Callbacks
}
