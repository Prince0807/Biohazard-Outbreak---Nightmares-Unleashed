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

        WeaponsManager.Instance.PlayAudio(weaponData.draw_Audio);
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
        WeaponsManager.Instance.PlayAudio(weaponData.reload_Audio);
        weaponData.reloading = true;
    }

    override protected bool CanShoot()
    {
        return !weaponData.reloading && timeSinceLastShot > 1f / (weaponData.fireRate / 60f);
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
                    WeaponsManager.Instance.PlayAudio(weaponData.fire_Audio);
                }

                weaponData.currentAmmo--;
                timeSinceLastShot = 0;
            }
            else
                Reload();
        }
    }
}
