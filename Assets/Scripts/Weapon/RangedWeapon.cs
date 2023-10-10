using System;
using UnityEngine;

public class RangedWeapon : Weapon
{
    [SerializeField] Transform muzzle;
    

    private void OnEnable()
    {
        if (weaponData.type == WeaponType.Pistol)
            PlayerInput.oneShootInput += Shoot;
        else if (weaponData.type == WeaponType.Rifle)
            PlayerInput.shootInput += Shoot;

        PlayerInput.reloadInput += Reload;
    }
    private void OnDisable()
    {
        if (weaponData.type == WeaponType.Pistol)
            PlayerInput.oneShootInput -= Shoot;
        else if (weaponData.type == WeaponType.Rifle)
            PlayerInput.shootInput -= Shoot;

        PlayerInput.reloadInput -= Reload;
    }

    private void Reload()
    {
        if (weaponData.reloading)
            return;

        animator.Play("Reload");
        weaponData.reloading = true;
    }
    public void FinishedReloading()
    {
        weaponData.totalAmmo -= (weaponData.magSize - weaponData.currentAmmo);
        weaponData.currentAmmo = weaponData.magSize;
        weaponData.reloading = false;
    }

    override protected bool CanShoot()
    {
        return !weaponData.reloading && timeSinceLastShot > 1f / (weaponData.fireRate / 60f);
    }

    override protected void Shoot()
    {
        if(weaponData.currentAmmo > 0 && CanShoot())
        {
            if(Physics.Raycast(muzzle.position, transform.forward, out RaycastHit hitInfo, weaponData.maxDistance))
            {
                Debug.Log(hitInfo.transform.name);

                animator.SetTrigger("Fire");
            }

            weaponData.currentAmmo--;
            timeSinceLastShot = 0;
        }
    }
}
