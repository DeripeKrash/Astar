using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScript : MonoBehaviour
{
    public float speed = 5.0f;
    public float bottom = 0;

    private void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        transform.Translate(new Vector3(0,-1,0)  * speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.y) > bottom  || Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("OpenInGameMenu"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
