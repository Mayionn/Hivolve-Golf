using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Managers;

public static class SaveSystem
{

    public static void SaveCurrency(int gold, int diamonds)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveData.sd";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameManager.Instance.Data.Setup_Currency(gold, diamonds);

        formatter.Serialize(stream, GameManager.Instance.Data);
        stream.Close();
    }

    public static void SaveCurrentSkin_Ball(int index)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveData.sd";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameManager.Instance.Data.SetupCurrentSkin_Ball(index);

        formatter.Serialize(stream, GameManager.Instance.Data);
        stream.Close();
    }
    public static void SaveCurrentSkin_Hat(int index)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveData.sd";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameManager.Instance.Data.SetupCurrentSkin_Hat(index);

        formatter.Serialize(stream, GameManager.Instance.Data);
        stream.Close();
    }

    public static void SaveSkins_Ball(int count, int[] indexes)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveData.sd";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameManager.Instance.Data.SetupSkins_Balls(count, indexes);

        formatter.Serialize(stream, GameManager.Instance.Data);
        stream.Close();
    }
    public static void SaveSkins_Hats(int count, int[] indexes)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveData.sd";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameManager.Instance.Data.SetupSkins_Hats(count, indexes);

        formatter.Serialize(stream, GameManager.Instance.Data);
        stream.Close();
    }

    public static SaveData LoadData()
    {
        string path = Application.persistentDataPath + "/SaveData.sd";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            StreamWriter stream = File.CreateText(Application.persistentDataPath + "/SaveData.sd");
            stream.Close();
            Debug.LogError("Save File Not Found " + path);
            return null;
        }
    }

    public static void ClearData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveData.sd";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData();

        formatter.Serialize(stream, data);
        stream.Close();
    }
}
