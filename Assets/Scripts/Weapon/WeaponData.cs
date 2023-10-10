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
    public float fireRate;
    public bool reloading;
}

public enum WeaponType
{
    Pistol,
    Rifle,
    Melee
}
