using Assets.Generics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SaveData;
using Assets.Managers;

public class SaveManager : Singleton<SaveManager>
{
    public SaveData Data;

    public void Init()
    {
        Data = SaveSystem.LoadData();
    }

    public void SaveCurrency(int gold, int diamonds)
    {
        SaveSystem.SaveCurrency(gold, diamonds);
    }
    public void LoadCurrency()
    {
        SaveData data = SaveSystem.LoadData();
        ProfileManager.Instance.Gold = data.Gold;
        ProfileManager.Instance.Diamonds = data.Diamonds;
    }

    public void SaveMapProgress()
    {
        int unlockedChaptersCount = 0;
        List<MapChapter> cm = MapManager.Instance.ChapterMaps;
        for (int i = 0; i < cm.Count; i++)
        {
            if(!cm[i].Maps[0].IsLocked)
            {
                unlockedChaptersCount++;
            }
        }

        int unlockedMapCount = 0;
        for (int i = 0; i < unlockedChaptersCount; i++)
        {
            for (int o = 0; o < cm[i].Maps.Length; o++)
            {
                if(!cm[i].Maps[o].IsLocked)
                {
                    unlockedMapCount++;
                }
            }

            float[,] scoreStrikes = new float[unlockedMapCount,1];
            float[,] scoreTimer = new float[unlockedMapCount, 1];
            for (int p = 0; p < unlockedMapCount; p++)
            {
                scoreStrikes[p, 0] = cm[i].Maps[p].PB.Strikes;
                scoreTimer[p, 0] = cm[i].Maps[p].PB.Time;

                SaveSystem.SaveMapProgressScore_Strikes(i, scoreStrikes);
                SaveSystem.SaveMapProgressScore_Timer(i, scoreTimer);
            }
            unlockedMapCount = 0;
        }
    }
    public void LoadMapProgress()
    {
        SaveData data = SaveSystem.LoadData();
        if(data.Chapter01_Score_Strikes != null)
        {
            for (int i = 0; i < MapManager.Instance.ChapterMaps.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        for (int o = 0; o < data.Chapter01_Score_Strikes.Length; o++)
                        {
                            MapManager.Instance.ChapterMaps[i].Maps[o].PB.Strikes = (int)data.Chapter01_Score_Strikes[o,0];
                            MapManager.Instance.ChapterMaps[i].Maps[o].PB.Time = data.Chapter01_Score_Timer[o, 0];
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            Debug.LogWarning("No Score Saved");
        }
    }

    //Current Skin ---- BALL
    public void SaveCurrentSkin_Ball(int index)
    {
        SaveSystem.SaveCurrentSkin_Ball(index);
    }
    public void LoadCurrentSkin_Ball()
    {
        SaveData data = SaveSystem.LoadData();
        GameManager.Instance.CurrentPlayer.Skin_Ball = SkinsManager.Instance.List_Skins_Balls[data.CurrentSkin_Ball_Index];
        Material material = SkinsManager.Instance.List_Skins_Balls[data.CurrentSkin_Ball_Index].Material;
        GameManager.Instance.CurrentPlayer.SelectedBall.GetComponent<Renderer>().material = material;
    }
    //Current Skin ---- HAT
    public void SaveCurrentSkin_Hat(int index)
    {
        SaveSystem.SaveCurrentSkin_Hat(index);
    }
    public void LoadCurrentSkin_Hat()
    {
        SaveData data = SaveSystem.LoadData();
        GameManager.Instance.CurrentPlayer.Skin_Hat = SkinsManager.Instance.List_Skins_Hats[data.CurrentSkin_Hat_Index];
        GameManager.Instance.CurrentPlayer.Skin_Hat.Load_Skin(GameManager.Instance.CurrentPlayer);
    }
    //Unlocked Skins ---- BALLS
    public void SaveUnlockedSkins_Balls()
    {
        int count = 0;
        List<int> indexesList = new List<int>();
        //GetIndexes of Unlocked Ball Skins
        for (int i = 0; i < SkinsManager.Instance.List_Skins_Balls.Count; i++)
        {
            if (SkinsManager.Instance.List_Skins_Balls[i].IsUnlocked)
            {
                count++;
                indexesList.Add(i);
            }
        }
        //transform list to array
        int[] indexes = new int[count];
        for (int i = 0; i < count; i++)
        {
            indexes[i] = indexesList[i];
        }
        //Send array to SaveSystem to save
        SaveSystem.SaveSkins_Ball(count, indexes);
    }
    public void LoadUnlockedSkins_Balls()
    {
        SaveData data = SaveSystem.LoadData();

        if (data.UnlockedSkins_Balls != null)
        {
            for (int i = 0; i < data.UnlockedSkins_Balls.Length; i++)
            {
                SkinsManager.Instance.List_Skins_Balls[data.UnlockedSkins_Balls[i]].IsUnlocked = true;
            }
        }
    }
    //Unlocked Skins ---- HATS
    public void SaveUnlockedSkins_Hats()
    {
        int count = 0;
        List<int> indexesList = new List<int>();
        //GetIndexes of Unlocked Ball Skins
        for (int i = 0; i < SkinsManager.Instance.List_Skins_Hats.Count; i++)
        {
            if (SkinsManager.Instance.List_Skins_Hats[i].IsUnlocked)
            {
                count++;
                indexesList.Add(i);
            }
        }
        //transform list to array
        int[] indexes = new int[count];
        for (int i = 0; i < count; i++)
        {
            indexes[i] = indexesList[i];
        }
        //Send array to SaveSystem to save
        SaveSystem.SaveSkins_Hats(count, indexes);
    }
    public void LoadUnlockedSkins_Hats()
    {
        SaveData data = SaveSystem.LoadData();

        if (data.UnlockedSkins_Hats != null)
        {
            for (int i = 0; i < data.UnlockedSkins_Hats.Length; i++)
            {
                SkinsManager.Instance.List_Skins_Hats[data.UnlockedSkins_Hats[i]].IsUnlocked = true;
            }
        }
    }
}
