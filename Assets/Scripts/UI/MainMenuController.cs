using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    [SerializeField] private GameObject loadingScreen;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayGameBtn()
    {
        loadingScreen.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void SetAnimationIndex(int index)
    {
        animator.SetInteger("Index", index);
    }

    public void ExitGameBtn()
    {
        Application.Quit();
    }

    public void OnMusicSliderChange()
    {
        audioMixer.SetFloat("MusicVol", musicSlider.value);
    }

    public void OnSfxSliderChange()
    {
        audioMixer.SetFloat("SfxVol", soundSlider.value);
    }
}
