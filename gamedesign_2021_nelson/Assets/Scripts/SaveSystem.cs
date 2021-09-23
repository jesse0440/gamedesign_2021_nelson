using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    // Saves the player to a binary file
    public static void SavePlayer (PlayerController player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    // Loads the binary file and returns the data from the file
    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.data";

        // If the file path is not null
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            stream.Close();

            return data;
        }

        // If the file path is null
        else
        {
            Debug.LogError("Save file not found in" + path);
            
            return null;
        }
    }
}