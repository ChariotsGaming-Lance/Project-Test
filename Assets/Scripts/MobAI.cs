using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIStates { patrol, chase, attack }
public class MobAI : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private NavMeshAgent navMeshAgent
    {
        get
        {
            if (null == _navMeshAgent)
                _navMeshAgent = GetComponent<NavMeshAgent>();
            return _navMeshAgent;
        }
    }

    private Mob _mob;
    private Mob mob
    {
        get
        {
            if (null == _mob)
                _mob = GetComponent<Mob>();
            return _mob;
        }
    }

    private FieldOfView _fieldOfView;
    private FieldOfView fieldOfView
    {
        get
        {
            if (null == _fieldOfView)
                _fieldOfView = GetComponent<FieldOfView>();
            return _fieldOfView;
        }
    }

    private HeroSkill _heroSkill;
    private HeroSkill heroSkill
    {
        get
        {
            if (null == _heroSkill)
                _heroSkill = GetComponent<HeroSkill>();
            return _heroSkill;
        }
    }

    private Rigidbody _rb;
    private Rigidbody rb
    {
        get
        {
            if (null == _rb)
                _rb = GetComponent<Rigidbody>();
            return _rb;
        }
    }

    [Header("Monitoring Purpose")]
    [SerializeField]
    private Waypoint currentWaypoint = null;
    [SerializeField]
    private Transform currentTarget = null;
    [SerializeField]
    private Transform previouslyAttackedTarget = null;
    [SerializeField]
    private AIStates state = AIStates.patrol;

    private void Awake()
    {
        rb.isKinematic = true;
    }

    private void Start()
    {
        navMeshAgent.SetDestination(mob.GetWaypointManager().GetRandomWayPoint().transform.position);
    }

    private void FixedUpdate()
    {
        //Debug.Log(rb.velocity.magnitude);
        if (mob.GetStats().isAttacking)
        {
            return;
        }

        currentTarget = fieldOfView.GetClosestTarget();

        AIDecisions();

        if (state == AIStates.patrol)
            Patrol();
        else if (state == AIStates.chase)
            Chase();
        else if (state == AIStates.attack)
            Attack();

        fieldOfView.DrawFieldOfView();       
    }

    private void AIDecisions ()
    {
        

        if (null == currentTarget)
            state = AIStates.patrol;
        else if (null != currentTarget && previouslyAttackedTarget != currentTarget)
        {
            if(navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
                state = AIStates.chase;
            else //if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                state = AIStates.attack;
        }
    }

    private void Patrol()
    {
        if (null == currentWaypoint)
            currentWaypoint = mob.GetWaypointManager().GetRandomWayPoint();

        if (navMeshAgent.remainingDistance < 5F)
        {
            currentWaypoint = mob.GetWaypointManager().GetRandomWayPoint();
            navMeshAgent.SetDestination(currentWaypoint.transform.position);
        }
        navMeshAgent.SetDestination(currentWaypoint.transform.position);
    }

    private void Chase()
    {
        navMeshAgent.SetDestination(currentTarget.transform.position);
        navMeshAgent.stoppingDistance = 2F;
    }

    private void Attack()
    {
        if (!mob.GetStats().isAttacking)
            StartCoroutine(AttackContainer());
    }

    private IEnumerator AttackContainer()
    {
        mob.GetStats().isAttacking = true;
        navMeshAgent.isStopped = true;
        rb.isKinematic = false;
        previouslyAttackedTarget = currentTarget;
        mob.GetStats().chargeStrength = 0;
        float randomChargeTime = Random.Range(0.5f, 3f);
        float chargeDuration = 0;

        while (chargeDuration < randomChargeTime)
        {
            chargeDuration += Time.deltaTime;
            mob.GetStats().chargeStrength += Time.deltaTime;
            yield return new WaitForSeconds(0.01F);
        }

        mob.GetDamager().gameObject.SetActive(true);

        heroSkill.LaunchAttack(mob.GetStats().chargeStrength);

        yield return new WaitForSeconds(randomChargeTime);
            
        mob.GetDamager().gameObject.SetActive(false);
        
        state = AIStates.patrol;
        mob.GetStats().isAttacking = false;
        rb.isKinematic = true;
        navMeshAgent.isStopped = false;
        mob.GetStats().chargeStrength = 0;
    }


}
