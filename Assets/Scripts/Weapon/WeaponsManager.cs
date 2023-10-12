using System.Collections;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    public static WeaponsManager Instance;
    
    private AudioSource weaponAudioSource;
    
    [SerializeField] Weapon[] weapons;
    private int currentWeaponIndex = 0;

    private void Awake()
    {
        Instance = this;
        weaponAudioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        PlayerInput.switchWeaponInput += StartSwitchingWeapon;
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

    public void PlayAudio(AudioClip clip)
    {
        weaponAudioSource.PlayOneShot(clip);
    }
}
