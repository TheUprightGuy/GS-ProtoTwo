using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Element element;
    public Transform target;
    public float projectileSpeed = 20.0f;
    private int damage;

    public void Setup(Magic _magic)
    {
        damage = _magic.damage;
        element = _magic.element;
    }

    // Set Target to Seek
    public void Seek(BaseCharacterClass _target)
    {
        // Bad Pass
        if (_target == null)
        {
            Destroy();
        }
        // Set Target to Seek
        else
        {
            target = _target.transform;
        }
    }
    // Done with this Projectile
    public void Destroy()
    {
        // Cleanup
        CombatController.instance.PassPriority();
        Destroy(gameObject);
    }

    // Movement + Hit Reg
    void Update()
    {
        // Target Dead
        if (target == null)
        {
            // May instead have bullet travel to targets last location
            Destroy();
            return;
        }
        // Target still Alive
        else
        {
            transform.LookAt(target.transform);

            // Get Direction & Find Movement
            Vector3 dir = target.position - transform.position;
            float distanceThisFrame = projectileSpeed * Time.deltaTime;

            // If distance to target is less than how far we move this frame
            if (dir.magnitude <= distanceThisFrame)
            {
                // Reg Hit
                HitTarget();
                return;
            }
            // Else Move
            else
            {
                transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            }
        }
    }

    // Hit Reg
    void HitTarget()
    {        
        Damage(target);       
        // Done w/ Projectile
        Destroy();
    }

    // Custom Sort by Distance
    public int DistanceSort(Collider _a, Collider _b)
    {
        return (transform.position - _a.transform.position).sqrMagnitude.
            CompareTo((transform.position - _b.transform.position).sqrMagnitude);
    }

    // Damage
    void Damage(Transform _enemy)
    {
        EnemyController enemy = _enemy.GetComponent<EnemyController>();

        // Ensure Enemy is Still Alive
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            // Check if Debuff to Apply
            /*if (debuff)
            {
                Debuff tempDebuff = Instantiate(debuff);
                enemy.ApplyDebuff(tempDebuff);
            }*/
        }
    }
}