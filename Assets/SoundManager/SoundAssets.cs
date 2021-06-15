using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAssets : MonoBehaviour
{
    public AudioClip playerAttack = null;
    public AudioClip enemyDestroyed = null;
    public AudioClip menuNavigation = null;
    public AudioClip teleportation = null;
    public AudioClip firstPlayerDash = null;
    public AudioClip secondPlayerDash = null;
    public AudioClip playerJump = null;
    public AudioClip cristalTakeDamage = null;
    public AudioClip winLevel = null;

    public AudioClip levelMusic1 = null;
    public AudioClip levelMusic2 = null;
    public AudioClip menuMusic = null;

    public static SoundAssets instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }
}
