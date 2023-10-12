using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("References")]
    public WeaponData weaponData;

    protected CharacterController characterController;
    protected Animator animator;
    
    protected float timeSinceLastShot;
    
    protected virtual void Awake()
    {
        characterController = GetComponentInParent<CharacterController>();
        animator = GetComponent<Animator>();
        weaponData.reloading = false;
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
