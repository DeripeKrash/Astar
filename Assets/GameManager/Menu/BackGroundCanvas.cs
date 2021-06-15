using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class BackGroundCanvas : MonoBehaviour
{
    private static BackGroundCanvas instance = null;
    private RawImage image = null;
    private VideoPlayer videoPlayer = null;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            image = GetComponentInChildren<RawImage>();
            videoPlayer = GetComponentInChildren<VideoPlayer>();
            instance = this;
        }
        else
            Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name != "MainMenu" && currentScene.name != "SelectLevel")
        {
            image.enabled = false;
            videoPlayer.enabled = false;
        }
        else
        {
            image.enabled = true;
            videoPlayer.enabled = true;
        }
    }
}
