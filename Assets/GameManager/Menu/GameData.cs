using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class GameData
{
    public static float CelienScore = 0;
    public static float FlavioScore   = 0;
    public static float OscarScore    = 0;
    public static float RoseanneScore = 0;
    public static float GaspardScore  = 0;
    public static float YacineScore   = 0;

    /*public static bool PS4Input = false;
    public static bool Azerty = true;*/

    private static bool loaded = false;

    public static void LoadGameData()
    {
        if (loaded)
            return;

        loaded = true;
        SaveSystem.LoadData();
    }

}
