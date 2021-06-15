using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AstarManager : MonoBehaviour
{
    [SerializeField] private MenuManager menuManager = null;
    [SerializeField] private CristalScript cristal = null;
    [SerializeField] private CircleMouvement playerMovement = null;
    [SerializeField] private AttackAnimation playerAttack = null;
    [SerializeField] private FollowPlayer cam = null;
    [SerializeField] private Text score = null;

    public float savedScore = 0;
    
    private EnemyManager[] enemyManager = null;
    private EndLevel endLevelTransition = null;

    private void Start()
    {
        menuManager.TurnOffCanvas();
        enemyManager = FindObjectsOfType<EnemyManager>();
        endLevelTransition = GetComponent<EndLevel>();
    }

    void Update()
    {
        if (cristal.cristalLife <= 0 || Input.GetKey(KeyCode.U))
        {
            if (!endLevelTransition.isDoingTransition)
                endLevelTransition.PlayerLostLevel(score.transform.parent.GetComponent<Score>().score);

            if (endLevelTransition.IsEndTransition())
            {
                SceneManager.LoadScene("SelectLevel");
            }
        }
        else if (isAllSpawnersDead() || Input.GetKey(KeyCode.R))
        {
            if (!endLevelTransition.isDoingTransition)
            {
                SoundManager.instance.PlaySingle(SoundAssets.instance.winLevel);
                endLevelTransition.PlayerWonLevel(score.transform.parent.GetComponent<Score>().score + (cristal.cristalLife * 200f));
            }

            if (endLevelTransition.IsEndTransition())
            {
                SaveScoreAndEndLevel();
            }
        }
        else
        {
            if (menuManager.canvas.isActiveAndEnabled || menuManager.optionsCanvas.isActiveAndEnabled)
            {
                if (playerMovement.enabled == true)
                    playerMovement.enabled = false;
                if (playerAttack.enabled == true)
                    playerAttack.enabled = false;
                if (cam.enabled == true)
                    cam.enabled = false;
                SoundManager.instance.PauseMusic();
            }
            else if (!menuManager.canvas.isActiveAndEnabled && !menuManager.optionsCanvas.isActiveAndEnabled)
            {
                if (playerMovement.enabled == false)
                    playerMovement.enabled = true;
                if (playerAttack.enabled == false)
                    playerAttack.enabled = true;
                if (cam.enabled == false)
                    cam.enabled = true;
                if (SoundManager.instance != null)
                    SoundManager.instance.ResumeMusic();
            }
        }
    }

    private bool isAllSpawnersDead()
    {
        for (uint i = 0; i < enemyManager.Length; ++i)
        {
            if (!enemyManager[i].isAllEnemiesDead())
            {
                return false;
            }
        }
        return true;
    }

    private void SaveScoreAndEndLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        savedScore = score.transform.parent.GetComponent<Score>().score + (cristal.cristalLife * 200f);

        if (sceneName == "FlavioScene")
        {
            if (savedScore > GameData.FlavioScore)
                GameData.FlavioScore = savedScore;
        }
        else if (sceneName == "YacineScene")
        {
            if (savedScore > GameData.YacineScore)
                GameData.YacineScore = savedScore;
        }
        else if (sceneName == "OscarScene")
        {
            if (savedScore > GameData.OscarScore)
                GameData.OscarScore = savedScore;
        }
        else if (sceneName == "RoseanneScene")
        {
            if (savedScore > GameData.RoseanneScore)
                GameData.RoseanneScore = savedScore;
        }
        else if (sceneName == "GaspardScene")
        {
            if (savedScore > GameData.GaspardScore)
                GameData.GaspardScore = savedScore;
        }
        else if (sceneName == "CelienScene")
        {
            if (savedScore > GameData.CelienScore)
                GameData.CelienScore = savedScore;
        }
        SceneManager.LoadScene("SelectLevel");
    }
}
