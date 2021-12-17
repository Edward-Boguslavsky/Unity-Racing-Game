using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    static string filePath = Application.persistentDataPath + "/progress.data";

    public static void SaveProgress(GameManager player)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream fileStream = new FileStream(filePath, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    public static PlayerData LoadProgress()
    {
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open);

            PlayerData data = formatter.Deserialize(fileStream) as PlayerData;
            fileStream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Progress data not found");
            return null;
        }
    }

    public static bool SaveFileExists()
    {
        return File.Exists(filePath);
    }
}
