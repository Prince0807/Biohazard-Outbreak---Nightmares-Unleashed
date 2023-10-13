using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/Data")]
public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public new string name;
    public WeaponType type;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;

    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    public int totalAmmo;
    public int maxCarryAmmo;
    public float fireRate;

    [Header("Audio")]
    public AudioClip draw_Audio;
    public AudioClip fire_Audio;
    public AudioClip reload_Audio;
    public AudioClip emptyMag_Audio;

    

    public void AddAmmo(int ammo)
    {
        totalAmmo += ammo;

        if(totalAmmo > maxCarryAmmo)
            totalAmmo = maxCarryAmmo;
    }
}

public enum WeaponType
{
    Pistol,
    Rifle,
    Melee
}
