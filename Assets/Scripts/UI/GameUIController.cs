using Obscure.SDC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    public static GameUIController Instance;

    private WeaponsManager weaponsManager;
    private FpsController fpsController;

    [Header("Weapon UI")]
    public Crosshair crosshair;
    [SerializeField] private TMP_Text magazineAmmo;
    [SerializeField] private TMP_Text carryingAmmo;

    [Header("Health UI")]
    [SerializeField] private TMP_Text health;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(Instance);
        Instance = this;

        if(weaponsManager == null)
            weaponsManager = FindObjectOfType<WeaponsManager>();
        if(fpsController == null)
            fpsController = FindObjectOfType<FpsController>();
    }

    private void Update()
    {
        UpdateAmmoUI();
        UpdateHealthUI();
    }

    public void UpdateAmmoUI()
    {
        magazineAmmo.text = weaponsManager.GetActiveWeapon().weaponData.magAmmo.ToString();
        carryingAmmo.text = weaponsManager.GetActiveWeapon().weaponData.carryingAmmo.ToString();
    }

    public void UpdateHealthUI()
    {
        health.text = fpsController.health.ToString();
    }
}
