using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPrefab : MonoBehaviour
{
    private SpellType spellType;

    private Element element;
    public float projectileSpeed = 5;
    private int damage;
    public ParticleSystem ps;
    public BaseCharacterClass target;

    public GameObject hitFX;
    public GameObject critFX;

    public void Setup(Magic _magic, BaseCharacterClass _user, BaseCharacterClass _target)
    {
        damage = _magic.damage + _user.stats.magic * 3;
        element = _magic.element;
        spellType = _magic.spellType;
        target = _target;

        switch(spellType)
        {
            case SpellType.Projectile:
            {
                
                break;
            }
            case SpellType.Targeted:
            {
                CastAtTarget(_target);
                break;
            }
            case SpellType.Self:
            {
                // Not Currently Used
                break;
            }
        }

    }

    // Set Target to Seek
    public void CastAtTarget(BaseCharacterClass _target)
    {
        // Bad Pass
        if (_target == null)
        {
            Destroy();
        }
        // Damage the Target
        else
        {
            Damage(_target);
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
        // Destroy Once Finished Emitting
        if (!ps.IsAlive())
        {
            Destroy();
        }

        // Target Dead
        if (target == null)
        {
            // May instead have bullet travel to targets last location
            Destroy();
            return;
        }

        if (spellType == SpellType.Projectile)
        {
            // Target still Alive
            if (target.targetPoint)
            {
                transform.LookAt(target.targetPoint);
            }
            else
            {
                transform.LookAt(target.transform);
            }

            Vector3 dir;
            // Get Direction & Find Movement
            if (target.targetPoint)
            {
                dir = target.targetPoint.position - transform.position;
            }
            else
            {
                dir = target.transform.position - transform.position;
            }
            float distanceThisFrame = projectileSpeed * Time.deltaTime;

            // If distance to target is less than how far we move this frame
            if (dir.magnitude <= distanceThisFrame)
            {
                // Reg Hit
                CreateHitFX();
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

    void CreateHitFX()
    {
        // If crit use crit
        Instantiate(hitFX, target.transform);
    }

    // Hit Reg
    void HitTarget()
    {
        Damage(target);
        // Done w/ Projectile
        Destroy();
    }

    // Damage
    void Damage(BaseCharacterClass _target)
    {

        // Ensure Enemy is Still Alive
        if (_target != null)
        {
            _target.TakeDamage(damage, element);
        }
    }
}
