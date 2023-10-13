using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Weapon : MonoBehaviour
{
    [Header("References")]
    public WeaponData weaponData;

    protected CharacterController characterController;
    protected Animator animator;
    protected AudioSource audioSource;
    
    protected float timeSinceLastShot;
    
    protected virtual void Awake()
    {
        characterController = GetComponentInParent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        weaponData.currentAmmo = weaponData.magSize;
    }

    protected virtual void Update()
    {
        animator.SetFloat("Speed", characterController.velocity.sqrMagnitude);
        timeSinceLastShot += Time.deltaTime;
    }

    protected virtual bool CanShoot()
    {
        return timeSinceLastShot > 1f / (weaponData.fireRate / 60f);
    }
    protected abstract void Shoot();
}
