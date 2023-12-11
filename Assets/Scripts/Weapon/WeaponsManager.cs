using System.Collections;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{   
    [SerializeField] private Weapon[] weapons;
    private int currentWeaponIndex = 0;

    void Start()
    {
        PlayerInput.switchWeaponInput += StartSwitchingWeapon;
        weapons[currentWeaponIndex].gameObject.SetActive(true);
    }

    public void StartSwitchingWeapon()
    {
        if (currentWeaponIndex == PlayerInput.weaponIndex)
            return;
        StartCoroutine(Switch());
    }

    IEnumerator Switch()
    {
        weapons[currentWeaponIndex].GetComponent<Animator>().SetTrigger("Hide");
        yield return new WaitForSeconds(0.5f);
        weapons[currentWeaponIndex].gameObject.SetActive(false);
        weapons[PlayerInput.weaponIndex].gameObject.SetActive(true);
        currentWeaponIndex = PlayerInput.weaponIndex;
    }

    public Weapon GetActiveWeapon()
    {
        return weapons[currentWeaponIndex];
    }
}
