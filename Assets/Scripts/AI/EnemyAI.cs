using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class EnemyAI : MonoBehaviour

{
    [SerializeField] Transform target;
    NavMeshAgent navMeshAgent;
    [SerializeField] float chaseRange = 5f;
    private Animator animator;
    
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
		}
	}
    private void ChaseTarget() 
    {
        navMeshAgent.SetDestination(target.position);
        animator.SetFloat("IsWalking", 1f);
        animator.SetFloat("IsRunning", 1f);
    }
    private void AttackTarget()
	{
        Debug.Log("attacking");
        animator.SetFloat("IsAttacking", 1f);
    }

    //gizomos around the enemy circle
    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
