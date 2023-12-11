using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RangedWeapon : Weapon
{
    [SerializeField] Transform muzzle;
    [SerializeField] ParticleSystem muzzleFlash;
    private bool reloading;

    private void OnEnable()
    {
        if (weaponData.type == WeaponType.Pistol)
            PlayerInput.oneShootInput += Shoot;
        else if (weaponData.type == WeaponType.Rifle)
            PlayerInput.shootInput += Shoot;

        PlayerInput.reloadInput += StartReload;

        audioSource.clip = weaponData.draw_Audio;
        audioSource.Play();
        reloading = false;
    }
    private void OnDisable()
    {
        if (weaponData.type == WeaponType.Pistol)
            PlayerInput.oneShootInput -= Shoot;
        else if (weaponData.type == WeaponType.Rifle)
            PlayerInput.shootInput -= Shoot;

        PlayerInput.reloadInput -= StartReload;
    }

    private void StartReload()
    {
        if (reloading || weaponData.magAmmo >= weaponData.magCapacity)
            return;

        reloading = true;
        animator.Play("Reload");
        audioSource.clip = weaponData.reload_Audio;
        audioSource.Play();
    }
    public void FinishReload()
    {
        weaponData.carryingAmmo -= (weaponData.magCapacity - weaponData.magAmmo);
        weaponData.magAmmo = weaponData.magCapacity;
        reloading = false;
    }

    override protected bool CanShoot()
    {
        return !reloading && timeSinceLastShot > 1f / (weaponData.fireRate / 60f);
    }

    override protected void Shoot()
    {
        if (CanShoot())
        {
            if (weaponData.magAmmo > 0)
            {
                if (Physics.Raycast(Camera.main.transform.position, transform.forward, out RaycastHit hitInfo, weaponData.maxDistance))
                {
                    Debug.Log(hitInfo.collider.gameObject.name);
                    if(hitInfo.collider.TryGetComponent(out IDamageable hit))
                    {
                        Debug.Log("Hit");
                        hit.Damage(weaponData.damage);
                    }
                    animator.SetTrigger("Fire");
                    audioSource.clip = weaponData.fire_Audio;
                    audioSource.Play();
                    muzzleFlash.Play();
                }

                weaponData.magAmmo--;
                timeSinceLastShot = 0;
            }
            else
            {
                audioSource.clip = weaponData.emptyMag_Audio;
                audioSource.Play();
            }
        }
    }
}
