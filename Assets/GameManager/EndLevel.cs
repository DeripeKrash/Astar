using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private Canvas winLevelCanvas = null;
    [SerializeField] private RawImage winLevelBackGround = null;
    [SerializeField] private Text winLevelScore = null;
    [SerializeField] private Canvas gameOverLevelCanvas = null;
    [SerializeField] private Image gameOverLevelBackGround = null;
    [SerializeField] private TextMeshProUGUI gameOverLevelScore = null;
    [SerializeField] private float transitionTime = 5f;
    [SerializeField] private float endLevelFadeOutTime = 3f;

    [System.NonSerialized] public bool isDoingTransition = false;
    private float timerTransition = 0f;


    // Start is called before the first frame update
    void Start()
    {
        winLevelCanvas.gameObject.SetActive(false);
        gameOverLevelCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (winLevelCanvas.isActiveAndEnabled | gameOverLevelCanvas.isActiveAndEnabled)
        {
            timerTransition += Time.unscaledDeltaTime;
            Color fadeTransitionCoeff = new Color(0f, 0f, 0f, (Time.unscaledDeltaTime * (1f / endLevelFadeOutTime)));

            if (winLevelCanvas.isActiveAndEnabled)
                winLevelBackGround.color = winLevelBackGround.color + fadeTransitionCoeff;
            else if (gameOverLevelCanvas.isActiveAndEnabled)
                gameOverLevelBackGround.color = gameOverLevelBackGround.color + fadeTransitionCoeff;
        }
    }

    public void PlayerWonLevel(float score)
    {
        if (!winLevelCanvas.isActiveAndEnabled && !gameOverLevelCanvas.isActiveAndEnabled)
        {
            winLevelCanvas.gameObject.SetActive(true);
            winLevelScore.text = "Score : " + score;
            winLevelBackGround.color = new Color(winLevelBackGround.color.r, winLevelBackGround.color.g, winLevelBackGround.color.b, 0f);
            isDoingTransition = true;
        }
    }

    public void PlayerLostLevel(float score)
    {
        if (!winLevelCanvas.isActiveAndEnabled && !gameOverLevelCanvas.isActiveAndEnabled)
        {
            gameOverLevelCanvas.gameObject.SetActive(true);
            gameOverLevelScore.text = "Score : " + score;
            gameOverLevelBackGround.color = new Color(gameOverLevelBackGround.color.r, gameOverLevelBackGround.color.g, gameOverLevelBackGround.color.b, 0f);
            isDoingTransition = true;
        }
    }

    public bool IsEndTransition()
    {
        if (timerTransition >= transitionTime)
        {
            timerTransition = 0f;
            return true;
        }

        return false;
    }
}
