using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Managers;

namespace Assets.SaveData
{
    public static class SaveSystem
    {
        //----- CURRENCY
        public static void SaveCurrency(int gold, int diamonds)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/SaveData.sd";
            FileStream stream = new FileStream(path, FileMode.Create);

            SaveManager.Instance.Data.Setup_Currency(gold, diamonds);

            formatter.Serialize(stream, SaveManager.Instance.Data);
            stream.Close();
        }

        //----- SKINS
        public static void SaveCurrentSkin_Ball(int index)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/SaveData.sd";
            FileStream stream = new FileStream(path, FileMode.Create);

            SaveManager.Instance.Data.SetupCurrentSkin_Ball(index);

            formatter.Serialize(stream, SaveManager.Instance.Data);
            stream.Close();
        }
        public static void SaveCurrentSkin_Hat(int index)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/SaveData.sd";
            FileStream stream = new FileStream(path, FileMode.Create);

            SaveManager.Instance.Data.SetupCurrentSkin_Hat(index);

            formatter.Serialize(stream, SaveManager.Instance.Data);
            stream.Close();
        }
        public static void SaveCurrentSkin_Arrow(int index)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/SaveData.sd";
            FileStream stream = new FileStream(path, FileMode.Create);

            SaveManager.Instance.Data.SetupCurrentSkin_Arrow(index);

            formatter.Serialize(stream, SaveManager.Instance.Data);
            stream.Close();
        }
        public static void SaveCurrentSkin_ForceBar(int index)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/SaveData.sd";
            FileStream stream = new FileStream(path, FileMode.Create);

            SaveManager.Instance.Data.SetupCurrentSkin_ForceBar(index);

            formatter.Serialize(stream, SaveManager.Instance.Data);
            stream.Close();
        }
        public static void SaveSkins_Ball(int count, int[] indexes)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/SaveData.sd";
            FileStream stream = new FileStream(path, FileMode.Create);

            SaveManager.Instance.Data.SetupSkins_Balls(count, indexes);

            formatter.Serialize(stream, SaveManager.Instance.Data);
            stream.Close();
        }
        public static void SaveSkins_Hats(int count, int[] indexes)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/SaveData.sd";
            FileStream stream = new FileStream(path, FileMode.Create);

            SaveManager.Instance.Data.SetupSkins_Hats(count, indexes);

            formatter.Serialize(stream, SaveManager.Instance.Data);
            stream.Close();
        }
        public static void SaveSkins_Arrows(int count, int[] indexes)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/SaveData.sd";
            FileStream stream = new FileStream(path, FileMode.Create);

            SaveManager.Instance.Data.SetupSkins_Arrows(count, indexes);

            formatter.Serialize(stream, SaveManager.Instance.Data);
            stream.Close();
        }
        public static void SaveSkins_ForceBars(int count, int[] indexes)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/SaveData.sd";
            FileStream stream = new FileStream(path, FileMode.Create);

            SaveManager.Instance.Data.SetupSkins_ForceBars(count, indexes);

            formatter.Serialize(stream, SaveManager.Instance.Data);
            stream.Close();
        }

        //----- MAP PROGRESS
        public static void SaveMapProgressScore(float[][] scoreStrikes, float[][] scoreTimer) {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/SaveData.sd";
            FileStream stream = new FileStream(path, FileMode.Create);

            SaveManager.Instance.Data.SetupMapProgressScore(scoreStrikes, scoreTimer);

            formatter.Serialize(stream, SaveManager.Instance.Data);
            stream.Close();
        }

        //----- LOAD DATA
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
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Create);

                SaveData data = new SaveData();

                formatter.Serialize(stream, data);
                stream.Close();

                return data;
            }
        }

        //----- CLEAR DATA
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
}
