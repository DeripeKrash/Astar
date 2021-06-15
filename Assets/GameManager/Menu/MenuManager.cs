using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Canvas canvas = null;
    public Canvas optionsCanvas = null;

    public bool isOption = true;

    private void Start()
    {
        if (canvas == null)
        {
            Debug.LogError("ERROR : canvas == null");
            return;
        }

        if (!isOption)
        {
            return;
        }
        if (optionsCanvas == null)
        {
            Debug.LogError("ERROR : optionCanvas == null");
            return;
        }
        optionsCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("OpenInGameMenu"))
        {
            if (optionsCanvas != null && canvas.isActiveAndEnabled && !optionsCanvas.isActiveAndEnabled)
            {
                TurnOffCanvas();
            }
            else if (!canvas.isActiveAndEnabled && !optionsCanvas.isActiveAndEnabled)
            {
                TurnOnCanvas();
            }
        }
    }

    public void LoadSelectLevelScene()
    {
        SceneManager.LoadScene("SelectLevel");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ResumeGame()
    {
        if (canvas.isActiveAndEnabled)
            TurnOffCanvas();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void TurnOnCanvas()
    {
        canvas.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void TurnOffCanvas()
    {
        canvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void LoadGameOptionsCanvas()
    {
        canvas.gameObject.SetActive(false);
        optionsCanvas.gameObject.SetActive(true);
    }

    public void ReturnPreviousMenu()
    {
        optionsCanvas.gameObject.SetActive(false);
        canvas.gameObject.SetActive(true);
    }

    public void SetInputPS4True()
    {
        PlayerInputManager.PS4Input = true;
    }

    public void SetInputPS4False()
    {
        PlayerInputManager.PS4Input = false;
    }

    public void SetInputQwertyTrue()
    {
        PlayerInputManager.qwertyInput = true;
    }
    public void SetInputQwertyFalse()
    {
        PlayerInputManager.qwertyInput = false;
    }

    public void LoadCreditScene()
    {
        SceneManager.LoadScene("CreditScene");
    }
}
