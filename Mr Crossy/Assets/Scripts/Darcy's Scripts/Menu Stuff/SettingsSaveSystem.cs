using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SettingsSaveSystem //this script handles the conversion of settings data to binary for saving, and the conversion in the opposite direction for loading
{
    public static void SaveSettings(AudioSettings audioSettings, Player_Controller player)
    {      
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/settings.s"; //making the path. the file type can be whatever you want

        FileStream stream = new FileStream(path, FileMode.Create); //creating the stream

        SettingsData data = new SettingsData(audioSettings, player); //finding the data from the settings data script

        formatter.Serialize(stream, data); //converting the data
        stream.Close(); //closing so no further changes can be made
    }

    public static SettingsData LoadSettings() 
    {
        string path = Application.persistentDataPath + "/settings.s"; //finding the path that we saved the data to

        if (File.Exists(path)) //if it found one, starts loading
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open); //opening the stream back up

            SettingsData data = formatter.Deserialize(stream) as SettingsData; //loading, and converting back to values that unity can use

            stream.Close(); //closing the stream
            return data; //returning the data to the caller
        }
        else //if it can't find one, returns an error.
        {
            Debug.LogError("Settings file not found in " + path);
            return null;
        }
    }
}
