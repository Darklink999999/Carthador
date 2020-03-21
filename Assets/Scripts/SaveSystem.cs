using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void Save (MainCharacter player, Game game){

        BinaryFormatter f = new BinaryFormatter ();
        string path = Application.persistentDataPath + "/save.carthador";
        FileStream s = new FileStream (path, FileMode.Create);

        SaveData data = new SaveData (player, game);

        f.Serialize(s, data);
        s.Close ();
    }
    

    public static SaveData Load () {

        string path = Application.persistentDataPath + "/save.carthador"; 
        if (File.Exists (path)) {

        BinaryFormatter f = new BinaryFormatter ();
        FileStream s = new FileStream (path, FileMode.Open);

        SaveData data = f.Deserialize (s) as SaveData;
        s.Close();

        return data;



        }
        else {

            Debug.LogError("Save file not found in " + path);
            return null;
        }


    }



}
