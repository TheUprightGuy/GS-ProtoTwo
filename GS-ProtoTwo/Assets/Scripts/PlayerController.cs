using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum PlayerAction
{
    wait,
    goToStart,
    goToEnemy,
}

public class PlayerController : MonoBehaviour
{
    [Header("PlayerStats")]
    public float damage;
    public int id;
    [Header("Debug")]
    public EnemyController enemy;

    // Private Variables
    private Vector3 startPosition;
    private NavMeshAgent navAgent;
    private Animator animator;
    private PlayerAction currentAction;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        startPosition = transform.position;
    }

    private void Update()
    {
        // Test Attack
        if (Input.GetKeyDown(KeyCode.F))
        {
            MoveToTarget(enemy);
        }

        // Test Turns
        if (Input.GetKeyDown(KeyCode.Z))
        {
            EventHandler.instance.ToggleTurn(id);
        }
        // Test Turns
        if (Input.GetKeyDown(KeyCode.X))
        {
            EventHandler.instance.ToggleTurn(10);
        }

        // Path Completed && Not Waiting for Turn/Instruction
        if (!navAgent.pathPending && !navAgent.hasPath && currentAction != PlayerAction.wait)
        {
            CheckAction(currentAction);
        }
    }

    public void CheckAction(PlayerAction _action)
    {
        switch (_action)
        {
            case PlayerAction.goToEnemy:
            {
                Punch();
                currentAction = PlayerAction.wait;
                break;
            }
            case PlayerAction.goToStart:
            {
                currentAction = PlayerAction.wait;
                break;
            }

            default:
            {
                break;
            }
        }
    }

    public void MoveToStart()
    {
        currentAction = PlayerAction.goToStart;
        navAgent.SetDestination(startPosition);
    }


    public void MoveToTarget(EnemyController _enemy)
    {
        Vector3 dist = (_enemy.transform.position - transform.position) * 0.2f;
        navAgent.SetDestination(_enemy.transform.position - dist);

        currentAction = PlayerAction.goToEnemy;
    }

    public void Punch()
    {
        animator.SetTrigger("Punch");
    }

    public void DamageEnemy()
    {
        enemy.TakeDamage(damage);
    }
    
}
