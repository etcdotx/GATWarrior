using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {
    //public static string saveFileName;
    public static void SavePlayer(string saveSlotNumber) {
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/player" + saveSlotNumber + ".savegame";
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerSaveData data = new PlayerSaveData();

            formatter.Serialize(stream, data);
            stream.Close();
            Debug.Log("Save ok");
        } catch {
            Debug.Log("Save Failed");
        }
    }
    
    public static PlayerSaveData LoadPlayer(string saveSlotNumber)
    {
        string path = Application.persistentDataPath + "/player"+ saveSlotNumber + ".savegame";
        Debug.Log(path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerSaveData data = formatter.Deserialize(stream) as PlayerSaveData;

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void DeletePlayer(string saveSlotNumber)
    {
        string path = Application.persistentDataPath + "/player" + saveSlotNumber + ".savegame";
        Debug.Log(path);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else
        {
            Debug.LogError("There is no file" + path);
        }
    }
}
