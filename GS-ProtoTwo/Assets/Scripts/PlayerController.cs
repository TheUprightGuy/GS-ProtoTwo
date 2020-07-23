using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public EnemyController enemy;
    private Vector3 startPosition;
    public float speed;
    public float damage;
    public bool attacking = false;
    public bool finishedAnim = false;


    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            attacking = true;
        }



        if (attacking)
        {
            MoveToTarget(enemy);
        }
        else if (finishedAnim)
        {
            MoveToStart();
        }
    }


    public void MoveToStart()
    {
        float dist = Vector3.Distance(startPosition, transform.position);

        if (dist > 1.0f)
        {
            Vector3 dir = Vector3.Normalize(startPosition - transform.position);

            transform.Translate(dir * speed, Space.World);
        }
        else
        {
            finishedAnim = false;
        }
    }


    public void MoveToTarget(EnemyController _enemy)
    {
        float dist = Vector3.Distance(transform.position, _enemy.transform.position);

        if (dist > 1.0f)
        {
            Vector3 dir = Vector3.Normalize(_enemy.transform.position - transform.position);

            transform.Translate(dir * speed, Space.World);
        }
        else
        {
            // Play Anim
            attacking = false;
            DamageEnemy(_enemy);
        }
    }

    public void DamageEnemy(EnemyController _enemy)
    {
        // Take in Enemy
        // Damage Enemy
        _enemy.TakeDamage(damage);
        finishedAnim = true;
    }
    
}
