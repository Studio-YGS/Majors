using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SettingsSaveSystem
{
    public static void SaveSettings(AudioSettings audioSettings, Player_Controller player)
    {      
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/settings.s";

        FileStream stream = new FileStream(path, FileMode.Create);

        SettingsData data = new SettingsData(audioSettings, player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SettingsData LoadSettings()
    {
        string path = Application.persistentDataPath + "/settings.s";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            SettingsData data = formatter.Deserialize(stream) as SettingsData;

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Settings file not found in " + path);
            return null;
        }
    }
}
