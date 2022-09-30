using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public GameObject settingUI;
    public AudioSource audio;
    public AudioClip click;
    public Slider sfxSlider;
    public static float volume = 0.5f;       //0f ~ 1f
    // Start is called before the first frame update
    void Start()
    {
        volume = PlayerPrefs.GetFloat("Volume");
        sfxSlider.value = volume;
        settingUI.SetActive(false);
    }
    public void OpenSetting()
    {
        audio.PlayOneShot(click);
        settingUI.SetActive(true);
    }
    public void CloseSetting()
    {
        audio.PlayOneShot(click);
        settingUI.SetActive(false);
    }
    public void SFXSetting()
    {
        volume = sfxSlider.value;
        audio.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }
}
