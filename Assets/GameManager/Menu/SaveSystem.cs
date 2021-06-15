using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine.SceneManagement;

public static class SaveSystem
{
    public static void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Astar.save";
        FileStream stream = new FileStream(path, FileMode.Create);


        SaveData data = new SaveData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void LoadData()
    {
        string path = Application.persistentDataPath + "/Astar.save";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            SaveData data =  formatter.Deserialize(stream) as SaveData;
            data.ApplyData();
            stream.Close();
        }
        else
        {
            SceneManager.LoadScene("TutoScene");
            return;
        }
    }
}
