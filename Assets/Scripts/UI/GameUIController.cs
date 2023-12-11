using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    public static GameUIController Instance;

    private WeaponsManager weaponsManager;

    [SerializeField] private TMP_Text magazineAmmo;
    [SerializeField] private TMP_Text carryingAmmo;

    [SerializeField] private TMP_Text health;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance);
        Instance = this;

        if(weaponsManager == null)
            weaponsManager = FindObjectOfType<WeaponsManager>();
    }

    public void UpdateAmmoUI()
    {
        magazineAmmo.text = weaponsManager.GetActiveWeapon().weaponData.magAmmo.ToString();
        carryingAmmo.text = weaponsManager.GetActiveWeapon().weaponData.carryingAmmo.ToString();
    }

    public void UpdateHealthUI()
    {

    }
}
