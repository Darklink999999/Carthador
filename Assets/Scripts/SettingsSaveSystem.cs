using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SettingsSaveSystem
{
    public static void Save(Settings s)
    {

        BinaryFormatter f = new BinaryFormatter();
        string path = Application.persistentDataPath + "/settings.carthador";
        FileStream stream = new FileStream(path, FileMode.Create);

        SettingsSaveData data = new SettingsSaveData(s);

        f.Serialize(stream, data);
        stream.Close();
    }


    public static SettingsSaveData Load()
    {

        string path = Application.persistentDataPath + "/settings.carthador";
        if (File.Exists(path))
        {

            BinaryFormatter f = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SettingsSaveData data = f.Deserialize(stream) as SettingsSaveData;
            stream.Close();

            return data;



        }
        else
        {

            Debug.LogError("Save file not found in " + path);
            return null;
        }


    }
}
