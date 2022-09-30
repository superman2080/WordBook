using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip click;
    private void Start()
    {
        audio.volume = PlayerPrefs.GetFloat("Volume");
    }
    public void OpenMainScene()
    {
        audio.PlayOneShot(click);
        SceneManager.LoadScene("MainScene");
    }
    
    public void OpenWordLearning()
    {
        audio.PlayOneShot(click);
        SceneManager.LoadScene("WordLearning");
    }

    public void OpenWordSetting()
    {
        audio.PlayOneShot(click);
        SceneManager.LoadScene("WordSetting");
    }
}
