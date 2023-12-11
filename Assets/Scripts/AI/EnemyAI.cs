using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class EnemyAI : MonoBehaviour, IDamageable

{
    [SerializeField] Transform target;
    NavMeshAgent navMeshAgent;
    [SerializeField] float chaseRange = 5f;
    private Animator animator;
    
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;

    public int health { get; set; }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Debug.Log("EnemyAI : Death");
            animator.Play("Death");
            navMeshAgent.isStopped = true;
            navMeshAgent.enabled = false;
            this.enabled = false;
        }

        distanceToTarget = Vector3.Distance(target.position, transform.position);

		if (isProvoked)
		{
            EngageTarget();
		}
        else if (distanceToTarget<=chaseRange)
		{
            isProvoked = true;
            
        }
    }
     private void EngageTarget()
	{
        if(distanceToTarget >= navMeshAgent.stoppingDistance)
		{
            ChaseTarget();

		}
		if (distanceToTarget <=navMeshAgent.stoppingDistance)
		{
            AttackTarget();
            animator.SetFloat("IsAttacking", 1f);
        }
	}
    private void ChaseTarget() 
    {
        navMeshAgent.SetDestination(target.position);
        animator.SetFloat("IsWalking", 1f);
        animator.SetFloat("IsRunning", 1f);
        animator.SetFloat("IsAttacking", 0f);
    }
    private void AttackTarget()
	{
        
    }

    //gizomos around the enemy circle
    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

    public void Damage(int damage)
    {
        health -= damage;
    }
}
