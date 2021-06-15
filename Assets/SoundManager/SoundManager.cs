using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource efxSource = null;
    public AudioSource musicSource = null;
    public static SoundManager instance = null;

    private bool isMenu;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        isMenu = true;
        musicSource.volume = 0.25f;
        PlayMusic(SoundAssets.instance.menuMusic);
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingle(AudioClip clip)
    {
        if (efxSource != null)
            efxSource.PlayOneShot(clip);
    }

    public void PlaySingle(AudioClip clip, float volume)
    {
        if (efxSource != null)
            efxSource.PlayOneShot(clip, volume);
    }

    public void PlayMenuNavigation()
    {
        instance.PlaySingle(SoundAssets.instance.menuNavigation);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource != null)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level > 2)
        {
            PlayMusic(SoundAssets.instance.levelMusic1);

            isMenu = false;
        }
        else if (level == 2)
        { 
            PlayMusic(SoundAssets.instance.levelMusic2);
        }
        else if (level <= 1)
        {
            if (!isMenu)
            {
                PlayMusic(SoundAssets.instance.menuMusic);
                isMenu = true;
            }
        }
    }
}
