using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {

    /// <summary>
    /// function untuk save data
    /// </summary>
    /// <param name="saveSlotNumber">untuk penamaan slot file</param>
    public static void SavePlayer(string saveSlotNumber) {
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            //nama file yang dibuat
            string path = Application.persistentDataPath + "/player" + saveSlotNumber + ".savegame";
            FileStream stream = File.Create(path);
            Debug.Log(path);

            PlayerSaveData data = new PlayerSaveData();
            formatter.Serialize(stream, data);
            stream.Close();
        }
        catch
        {
            Debug.Log("Save Failed");
        }
    }
    
    /// <summary>
    /// function untuk load data
    /// </summary>
    /// <param name="saveSlotNumber">untuk pilih nama savefilenya(load slotnya)</param>
    /// <returns></returns>
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

    /// <summary>
    /// function untuk delete data
    /// </summary>
    /// <param name="saveSlotNumber">untuk pilih nomor savean yang ingin di delete</param>
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
