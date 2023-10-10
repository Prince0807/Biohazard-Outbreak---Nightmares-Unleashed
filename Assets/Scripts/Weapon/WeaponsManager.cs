using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    [SerializeField] GameObject[] weapons;
    
    int currentWeaponIndex = 0;


    void Start()
    {
        PlayerInput.switchWeaponInput += SwitchWeapon;
    }

    public void SwitchWeapon()
    {
        weapons[currentWeaponIndex].SetActive(false);
        weapons[PlayerInput.weaponIndex].SetActive(true);
        currentWeaponIndex = PlayerInput.weaponIndex;
    }
}
