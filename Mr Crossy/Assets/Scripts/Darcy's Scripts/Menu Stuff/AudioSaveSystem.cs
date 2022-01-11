using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class AudioSaveSystem
{
    public static void SaveAudio(AudioSettings audioSettings)
    {      
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/audiosettings.s";

        FileStream stream = new FileStream(path, FileMode.Create);

        AudioData data = new AudioData(audioSettings);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static AudioData LoadAudio()
    {
        string path = Application.persistentDataPath + "/audiosettings.s";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            AudioData data = formatter.Deserialize(stream) as AudioData;

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Audio Settings file not found in " + path);
            return null;
        }
    }
}
