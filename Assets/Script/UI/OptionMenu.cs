using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider music;

    public GameObject pauseMenu;
    public GameObject optionMenu;

    public void SetMusicVolume()
    {
        float volume = music.value;
        mixer.SetFloat("Music", Mathf.Log10(volume)*20);
    }

    public void OnFullscreen( bool isfullscreen)
    {
        Screen.fullScreen = isfullscreen;
    }

    public void BacktoPause()
    {
        pauseMenu.SetActive(true);
        optionMenu.SetActive(false);
    }
}
