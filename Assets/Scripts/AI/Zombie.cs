using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : NPC
{
    [SerializeField] protected Transform target;
    [SerializeField] protected Transform lookAt;

    [SerializeField] protected ParticleSystem bloodVFX;
    [SerializeField] protected ParticleSystem chunksVFX;

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
        health = 100;
        if (target == null)
            target = FindObjectOfType<FpsController>().transform;
    }

    public override void Damage(int damage)
    {
        Debug.Log(damage);
        health -= damage;
        bloodVFX.Play();
        bloodVFX.GetComponent<AudioSource>().Play();
        if (health <= 0)
        {
            chunksVFX.Play();
            chunksVFX.GetComponent<AudioSource>().Play();
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

        Debug.Log(state.ToString());

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
        animator.Play("Idle");
        if (enemyAIManager.wayPoints.Length > 0)
        {
            state = ZombieState.Wandering;
            currentWayPoint = enemyAIManager.wayPoints[Random.Range(0, enemyAIManager.wayPoints.Length)];
        }
    }
    protected void Wandering()
    {
        aiAgent.speed = walkSpeed;
        if (Mathf.Abs(Vector3.Distance(transform.position, target.position)) < chaseRange)
        {
            state = ZombieState.Chasing;
            return;
        }

        animator.Play("Walk");

        if (Mathf.Abs(Vector3.Distance(transform.position, currentWayPoint.position)) < 2f)
            currentWayPoint = enemyAIManager.wayPoints[Random.Range(0, enemyAIManager.wayPoints.Length)];
        else
            aiAgent.SetDestination(currentWayPoint.position);
    }
    protected void Chasing()
    {
        aiAgent.speed = runSpeed;
        if (Mathf.Abs(Vector3.Distance(transform.position, target.position)) < attackRange)
        {
            Debug.Log("Attacking state not received");
            state = ZombieState.Attacking;
            return;
        }
        if (Mathf.Abs(Vector3.Distance(transform.position, target.position)) > chaseRange)
        {
            state = ZombieState.Wandering;
            return;
        }
        animator.Play("Run");
        aiAgent.SetDestination(target.position);
    }
    protected void Attacking()
    {
        if (Mathf.Abs(Vector3.Distance(transform.position, target.position)) > (attackRange + 1))
        {
            state = ZombieState.Chasing;
            return;
        }
        animator.Play("Attack");
    }
    protected void Dead()
    {
        animator.Play("Death");
        aiAgent.isStopped = true;
        aiAgent.enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        isAlive = false;
        GameManager.Instance.AddScore(100);
        Destroy(gameObject, 5f);
    }

    private void LookAtTarget()
    {
        lookAt.LookAt(target.position);
        transform.rotation = Quaternion.Euler(0f, lookAt.eulerAngles.y, 0f);
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
