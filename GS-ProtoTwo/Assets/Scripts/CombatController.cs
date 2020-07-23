using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject enemyPrefab;

    private int activeTurn = 0;
    [HideInInspector] public List<BaseCharacterClass> turnOrder;

    private void Start()
    {
        SetupBattle();
        GetTurnOrder();

        Invoke("StartBattle", 0.1f);
    }

    // Instantiate Enemies
    public void SetupBattle()
    {
        EnemyController temp = Instantiate(enemyPrefab).GetComponent<EnemyController>();
        enemy = temp;

        turnOrder.Add(temp);
        turnOrder.Add(player);
    }

    public void GetTurnOrder()
    {
        // Sort by Speed
        turnOrder.OrderBy(f => f.speed);
        turnOrder.Reverse();

        // Check Turn IDs
        for (int i = 0; i < turnOrder.Count; i++)
        {
            Debug.Log(turnOrder[i].name + " turn is #" + i);
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

    public event Action<int> setTurn;
    public void SetTurn(int _id)
    {
        if (setTurn != null)
        {
            setTurn(_id);
        }
    }

}
