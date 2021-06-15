using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Globalization;
using UnityEngine.UI;
using TMPro;

public class SelectLevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI FlavioLevelScore = null;
    [SerializeField] private TextMeshProUGUI YacineLevelScore = null;
    [SerializeField] private TextMeshProUGUI OscarLevelScore = null;
    [SerializeField] private TextMeshProUGUI RoseanneLevelScore = null;
    [SerializeField] private TextMeshProUGUI GaspardLevelScore = null;
    [SerializeField] private TextMeshProUGUI CelienLevelScore = null;

    private void Start()
    {
        GameData.LoadGameData();
        SaveSystem.SaveData();

        FlavioLevelScore.text = GameData.FlavioScore.ToString();
        YacineLevelScore.text = GameData.YacineScore.ToString();
        OscarLevelScore.text = GameData.OscarScore.ToString();
        RoseanneLevelScore.text = GameData.RoseanneScore.ToString();
        GaspardLevelScore.text = GameData.GaspardScore.ToString();
        CelienLevelScore.text = GameData.CelienScore.ToString();

    }

    public void LoadTestScene()
    { 
        if (Input.GetKey(KeyCode.Joystick1Button8) || Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("MainScene");
            return;
        }
        SceneManager.LoadScene("TutoScene");
    }

    public void LoadFlavioScene()
    {
        SceneManager.LoadScene("FlavioScene");
    }

    public void LoadYacineScene()
    {
        SceneManager.LoadScene("YacineScene");
    }

    public void LoadRoseanneScene()
    {
        SceneManager.LoadScene("RoseanneScene");
    }

    public void LoadOscarScene()
    {
        SceneManager.LoadScene("OscarScene");
    }

    public void LoadCelienScene()
    {
        SceneManager.LoadScene("CelienScene");
    }

    public void LoadGaspardScene()
    {
        SceneManager.LoadScene("GaspardScene");
    }

}
