using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobAnimationControl : MonoBehaviour
{
    private Animator _animator;
    private Animator animator
    {
        get
        {
            if (null == _animator)
                _animator = GetComponent<Animator>();
            return _animator;
        }
    }

    private NavMeshAgent _navMeshAgent;
    private NavMeshAgent navMeshAgent
    {
        get
        {
            if (null == _navMeshAgent)
                _navMeshAgent = GetComponentInParent<NavMeshAgent>();
            return _navMeshAgent;
        }
    }

    private Mob _mob;
    private Mob mob {
        get
        {
            if (null == _mob)
                _mob = GetComponentInParent<Mob>();
            return _mob;
        }
    }

    private void FixedUpdate()
    {
        Animate();
        FlipSprite();
    }

    private void FlipSprite()
    {
        if (navMeshAgent.velocity.x > 0.01F)
            transform.localScale = Vector3.one;
        else if (navMeshAgent.velocity.x < -0.01F)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void Animate()
    {
        animator.SetFloat("velocityX", Mathf.Abs(navMeshAgent.velocity.x));
        animator.SetFloat("chargeLevel", mob.GetStats().chargeStrength);
    }
}
