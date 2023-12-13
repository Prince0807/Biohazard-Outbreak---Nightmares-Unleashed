using Obscure.SDC;
using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RangedWeapon : Weapon
{
    [SerializeField] Transform muzzle;
    [SerializeField] ParticleSystem muzzleFlash;

    private Crosshair crosshair;
    private Vector2 crosshairDefaultSize = new Vector2(40f, 40f);

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
        crosshair.SetSize(crosshairDefaultSize);
    }

    protected override void Awake()
    {
        base.Awake();
        crosshair = GameUIController.Instance.crosshair;
    }

    protected override void Update()
    {
        base.Update();
        crosshair.SetSize(crosshairDefaultSize);
        if (crosshair.GetTarget() == null)
            crosshair.SetColor(Color.gray, 0.5f, 1f);
        else if (crosshair.GetTarget().GetComponent<IDamageable>() != null)
            crosshair.SetColor(Color.red, 1f, 0.5f);
        else
            crosshair.SetColor(Color.white, 1f, 1f);
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
                animator.Play("Fire", 0, 0);
                audioSource.clip = weaponData.fire_Audio;
                audioSource.Play();
                muzzleFlash.Play();
                crosshair.MultiplySize(1.5f, 0.5f);

                weaponData.magAmmo--;
                timeSinceLastShot = 0;

                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, weaponData.maxDistance))
                    if (hitInfo.collider.TryGetComponent(out IDamageable hit))
                        hit.Damage(weaponData.damage);
            }
            else
            {
                audioSource.clip = weaponData.emptyMag_Audio;
                audioSource.Play();
            }
        }
    }
}
