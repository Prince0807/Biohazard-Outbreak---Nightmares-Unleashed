using UnityEngine;
using UnityEngine.AI;

public abstract class NPC : MonoBehaviour, IDamageable
{
    protected NavMeshAgent aiAgent;
    protected Animator animator;

    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runSpeed;
    [SerializeField] protected float chaseRange;
    [SerializeField] protected float attackRange;

    protected bool isAlive = true;

    public int health { get; set; }

    public abstract void Damage(int damage);
}
