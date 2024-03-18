using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public GameObject soundOff, soundOn;

    public AudioSource bgMusic, soundEffects, whileSpinSound;

    [SerializeField]private AudioClip buttonClickSound, spinSound, stopSound, winSound;

    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if (!PlayerPrefs.HasKey("Sound"))
        {
            PlayerPrefs.SetInt("Sound", 1);
            AudioListener.volume = 1f;
        }
        else
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                SoundOff();
            }
            else
            {
                SoundOn();
            }
        }

        bgMusic.Play();
        soundOn.GetComponent<Button>().onClick.AddListener(SoundOff);
        soundOff.GetComponent<Button>().onClick.AddListener(SoundOn);

    }

    void SoundOff()
    {
        soundOff.SetActive(true);
        PlayerPrefs.SetInt("Sound", 0);
        AudioListener.volume = 0f;
    }
    void SoundOn()
    {
        PlayerPrefs.SetInt("Sound", 1);
        soundOff.SetActive(false);
        AudioListener.volume = 1f;
        ButtonClickSound();
    }

    public void ButtonClickSound()
    {
        soundEffects.PlayOneShot(buttonClickSound);
    }
    public void SpinSound()
    {
        whileSpinSound.PlayOneShot(buttonClickSound);
    }
    public void StopSound()
    {
        soundEffects.PlayOneShot(stopSound);
    }
    public void WinSound()
    {
        soundEffects.PlayOneShot(spinSound);
    }
}
