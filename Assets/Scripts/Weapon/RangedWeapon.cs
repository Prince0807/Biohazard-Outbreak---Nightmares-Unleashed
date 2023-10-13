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
        if (reloading || weaponData.currentAmmo >= weaponData.magSize)
            return;

        reloading = true;
        animator.Play("Reload");
        audioSource.clip = weaponData.reload_Audio;
        audioSource.Play();
    }
    public void FinishReload()
    {
        weaponData.totalAmmo -= (weaponData.magSize - weaponData.currentAmmo);
        weaponData.currentAmmo = weaponData.magSize;
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
            if (weaponData.currentAmmo > 0)
            {
                if (Physics.Raycast(muzzle.position, transform.forward, out RaycastHit hitInfo, weaponData.maxDistance))
                {
                    animator.SetTrigger("Fire");
                    audioSource.clip = weaponData.fire_Audio;
                    audioSource.Play();
                    muzzleFlash.Play();
                }

                weaponData.currentAmmo--;
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
