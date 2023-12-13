using Obscure.SDC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    public static GameUIController Instance;

    private WeaponsManager weaponsManager;
    private FpsController fpsController;

    [Header("Weapon UI")]
    public Crosshair crosshair;
    [SerializeField] private Image weaponLogo;
    [SerializeField] private Sprite pistol;
    [SerializeField] private Sprite ak74m;
    [SerializeField] private Sprite smg;
    [SerializeField] private TMP_Text magazineAmmo;
    [SerializeField] private TMP_Text carryingAmmo;

    [Header("Health UI")]
    [SerializeField] private TMP_Text health;
    [SerializeField] private Image bloodEffect;


    [SerializeField] private TMP_Text scoreText;

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

    public void SetBloodImageAlpha(float alpha)
    {
        alpha = (100 - alpha) / 100;
        Color color = bloodEffect.color;
        color.a = alpha;
        bloodEffect.color = color;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void UpdateWeaponLogo(int index)
    {
        switch (index)
        {
            case 0:
                weaponLogo.sprite = pistol;
                break;
            case 1:
                weaponLogo.sprite = ak74m;
                break;
            case 2:
                weaponLogo.sprite = smg;
                break;
        }
    }
}
