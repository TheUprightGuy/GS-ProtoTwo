using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    public Animator animator;

    public void AttackAnim()
    {
        animator.SetTrigger("Attack");
    }

    public void MagicAnim()
    {
        animator.SetTrigger("Magic");
    }

    public void FlinchAnim()
    {
        animator.SetTrigger("Flinch");
    }

    public void DeathAnim()
    {
        animator.SetTrigger("Death");
    }
}
