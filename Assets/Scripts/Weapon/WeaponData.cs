using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/Data")]
public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public new string name;
    public WeaponType type;

    [Header("Shooting")]
    public int damage;
    public float maxDistance;
    public float fireRate;

    [Header("Reloading")]
    public int magAmmo;
    public int magCapacity;
    public int carryingAmmo;
    public int maxCarryAmmo;

    [Header("Audio")]
    public AudioClip draw_Audio;
    public AudioClip fire_Audio;
    public AudioClip reload_Audio;
    public AudioClip emptyMag_Audio;

    

    public void AddAmmo(int ammo)
    {
        carryingAmmo += ammo;

        if(carryingAmmo > maxCarryAmmo)
            carryingAmmo = maxCarryAmmo;
    }
}

public enum WeaponType
{
    Pistol,
    Rifle,
    Melee
}
