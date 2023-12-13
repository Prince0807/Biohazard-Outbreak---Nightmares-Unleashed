using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : NPC
{
    [SerializeField] protected Transform target;
    protected EnemyAIManager enemyAIManager;
    protected Transform currentWayPoint;
    protected ZombieState state = ZombieState.Idle;

    [SerializeField] private int damage;

    protected void Awake()
    {
        aiAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyAIManager = GetComponentInParent<EnemyAIManager>();
    }

    private void Start()
    {
        if (target == null)
            target = FindObjectOfType<FpsController>().transform;
    }

    public override void Damage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            animator.Play("Death");
            state = ZombieState.Dead;
        }
    }

    void Attack()
    {
        if(Mathf.Abs(Vector3.Distance(transform.position, target.position)) < attackRange)
            target.GetComponent<IDamageable>().Damage(Random.Range(damage - 5, damage + 5));
    }

    private void Update()
    {
        if (!isAlive)
            return;

        switch (state)
        {
            case ZombieState.Idle:
                Idle();
                break;
            case ZombieState.Wandering:
                Wandering();
                break;
            case ZombieState.Chasing:
                Chasing();
                break;
            case ZombieState.Attacking:
                Attacking();
                break;
            case ZombieState.Dead:
                Dead();
                break;
        }
    }

    protected void Idle()
    {
        animator.CrossFade("Idle",0.25f);
        if (enemyAIManager.wayPoints.Length > 0)
        {
            state = ZombieState.Wandering;
            currentWayPoint = enemyAIManager.wayPoints[Random.Range(0, enemyAIManager.wayPoints.Length)];
        }
    }
    protected void Wandering()
    {
        if (Mathf.Abs(Vector3.Distance(transform.position, target.position)) < chaseRange)
        {
            state = ZombieState.Chasing;
            return;
        }

        animator.CrossFade("Walk", 0.25f);

        if (Mathf.Abs(Vector3.Distance(transform.position, currentWayPoint.position)) < 2f)
            currentWayPoint = enemyAIManager.wayPoints[Random.Range(0, enemyAIManager.wayPoints.Length)];
        else
            aiAgent.SetDestination(currentWayPoint.position);
    }
    protected void Chasing()
    {
        if (Mathf.Abs(Vector3.Distance(transform.position, currentWayPoint.position)) < attackRange)
        {
            state = ZombieState.Attacking;
            return;
        }
        animator.CrossFade("Run", 0.25f);
        aiAgent.SetDestination(target.position);
    }
    protected void Attacking()
    {
        if (Mathf.Abs(Vector3.Distance(transform.position, currentWayPoint.position)) > attackRange)
        {
            state = ZombieState.Chasing;
            return;
        }
        animator.CrossFade("Attack", 0.25f);
    }
    protected void Dead()
    {
        animator.CrossFade("Death", 0.25f);
        aiAgent.isStopped = true;
        aiAgent.enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        isAlive = false;
        Destroy(gameObject, 5f);
    }
}

public enum ZombieState
{
    Idle,
    Wandering,
    Chasing,
    Attacking,
    Dead
}
