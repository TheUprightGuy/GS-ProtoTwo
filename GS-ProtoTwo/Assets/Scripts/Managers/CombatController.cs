using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

public enum CombatState
{ 
    PLAYERTURN,
    ACTION,
    ENEMYTURN,
    BATTLEEND,
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
    public Inventory inventory;
    [Header("Scene Stuff")]
    public GameObject victoryScreenBlur;
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
        victoryScreenBlur.transform.position = Camera.main.transform.position;
        victoryScreenBlur.SetActive(false);


        // For Debug purposes make clones of everything
        //inventory.Setup();
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
            case CombatState.BATTLEEND:
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

    public void DisplayVictory()
    {
        // Probably add action bars to this cleanup function.
        StartCoroutine(PlayVictoryMusicAfterLevelUp());
        GameplayUIScript.instance.ToggleVictory(true);
        CleanUpUI();
        victoryScreenBlur.SetActive(true);
    }

    IEnumerator PlayVictoryMusicAfterLevelUp()
    {
        //Stop music to give level up sound room
        AudioManager.instance.StopMusic();
        yield return new WaitForSeconds(1f);    //To let the sound start playing
        yield return new WaitUntil(() => !AudioManager.instance.IsSoundPlaying("levelUp"));
        AudioManager.instance.SwitchMusicTrack("victory");
        yield return null;
    }

    public void DisplayGameOver()
    {
        // Probably add action bars to this cleanup function.
        GameplayUIScript.instance.ToggleGameOver(true);
        CleanUpUI();
        victoryScreenBlur.SetActive(true);
    }

    // Instantiate Enemies
    public void SetupBattle()
    {
        foreach (GameObject n in encounter.enemyPrefabs)
        {
            if (n.GetComponent<TreeBossController>())
            {
                TreeBossController temp = Instantiate(n, transform.position, transform.rotation).GetComponent<TreeBossController>();
                enemies.Add(temp);
            }
            else
            {
                EnemyController temp = Instantiate(n, transform.position, transform.rotation).GetComponent<EnemyController>();
                enemies.Add(temp);
            }
        }

        // temp
        enemies[0].transform.Translate(new Vector3(2, 0, 0));

        players.Add(player);
        players.Add(player2);

        player2.stats.Setup();
        player2.stats.SetupHPMP();

        foreach (BaseCharacterClass n in players)
        {
            turnOrder.Add(n);
            SetupCanvas((PlayerController)n);
            n.inventory = inventory;
        }
        foreach (BaseCharacterClass n in enemies)
        {
            // temp
            if (!CheckPlayers())
            {
                n.target = GetTarget();
            }
            turnOrder.Add(n);
        }
    }

    public BaseCharacterClass GetTarget()
    {
        int rand = UnityEngine.Random.Range(0, players.Count);
        while (!players[rand].alive)
        {
            rand = UnityEngine.Random.Range(0, players.Count);
        }
        return players[rand];
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
            if (n.GetComponent<TreeBossController>())
            {
                AddEnemyButton((TreeBossController)n);
            }
            else
            {
                AddEnemyButton((EnemyController)n);
            }
        }
        foreach (BaseCharacterClass n in players)
        {
            AddAllyButton((PlayerController)n);
        }
    }

    public void StartBattle()
    {
        activeTurn = 0;
        SetTurn(activeTurn);
    }

    public void NextTurn()
    {
        if (combatState != CombatState.BATTLEEND)
        {
            activeTurn = (activeTurn < turnOrder.Count - 1) ? (activeTurn + 1) : 0;

            SetTurn(activeTurn);

            if (!turnOrder[activeTurn].alive)
            {
                NextTurn();
            }
        }
    }

    public bool CheckPlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].alive)
            {
                return false;
            }
        }

        return true;
    }

    public bool CheckEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].alive)
            {
                return false;
            }
        }

        return true;
    }

    public void CheckRemainingCharacters()
    {
        if (CheckPlayers())
        {
            // GameOver Sequence
            ChangeState(CombatState.BATTLEEND);
            DisplayGameOver();
            encounter.EndEncounter();
            Debug.Log("All players dead");
        }
        if (CheckEnemies())
        {
            // Move to Win Battle
            ChangeState(CombatState.BATTLEEND);
            GetRewards();
            DisplayVictory();
            encounter.EndEncounter();
            Debug.Log("All enemies dead");
        }
    }

    public void GetRewards()
    {
        Inventory rewards = ScriptableObject.CreateInstance<Inventory>();
        rewards.Setup();

        foreach (GameObject n in encounter.enemyPrefabs)
        {
            inventory.AddItem(n.GetComponent<EnemyController>().GiveReward());
            rewards.AddItem(n.GetComponent<EnemyController>().GiveReward());
        }

        foreach (PlayerController n in players)
        {
            foreach (EnemyController o in enemies)
            {
                n.stats.GainXP((int)((float)o.stats.xpReward * Mathf.Sqrt(o.stats.level)));
            }
        }
            
        GameplayUIScript.instance.SetRewardText(rewards);
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

    public event Action<EnemyController> addEnemyButton;
    public void AddEnemyButton(EnemyController _enemy)
    {
        if (addEnemyButton != null)
        {
            addEnemyButton(_enemy);
        }
    }

    public event Action<PlayerController> addAllyButton;
    public void AddAllyButton(PlayerController _player)
    {
        if (addAllyButton != null)
        {
            addAllyButton(_player);
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

    public event Action<PlayerController, ActionDelegate, bool> chooseTarget;
    public void ChooseTarget(PlayerController _player, ActionDelegate _action, bool _offensive)
    {
        if (chooseTarget != null)
        {
            chooseTarget(_player, _action, _offensive);
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

    public event Action<int> turnOffTarget;
    public void TurnOffTarget(int _id)
    {
        if (turnOffTarget != null)
        {
            turnOffTarget(_id);
        }
    }

    public event Action cleanUp;
    public void CleanUpUI()
    {
        if (cleanUp != null)
        {
            cleanUp();
        }
    }

    #endregion Callbacks
}
