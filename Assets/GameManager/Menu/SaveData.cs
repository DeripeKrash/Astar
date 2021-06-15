using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float CelienScore;
    public float FlavioScore;
    public float OscarScore;
    public float RoseanneScore;
    public float GaspardScore;
    public float YacineScore;

    /*public static bool PS4Input = false;
    public static bool Azerty = true;*/

    public SaveData()
    {
        CelienScore   = GameData.CelienScore;
        FlavioScore   = GameData.FlavioScore;
        OscarScore    = GameData.OscarScore;
        RoseanneScore = GameData.RoseanneScore;
        GaspardScore  = GameData.GaspardScore;
        YacineScore   = GameData.YacineScore;
    }

    public void ApplyData()
    {
        GameData.CelienScore = CelienScore;
        GameData.FlavioScore = FlavioScore;
        GameData.OscarScore = OscarScore;
        GameData.RoseanneScore = RoseanneScore;
        GameData.GaspardScore = GaspardScore;
        GameData.YacineScore = YacineScore;
    }
}
