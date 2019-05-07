using Assets.Generics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public void SaveMapProgress()
    {
        int unlockedChaptersCount = 0;
        List<MapChapter> cm = MapManager.Instance.ChapterMaps;
        for (int i = 0; i < cm.Count; i++)
        {
            if(!cm[i].Maps[0].Display.Locked)
            {
                unlockedChaptersCount++;
            }
        }

        int unlockedMapCount = 0;
        for (int i = 0; i < unlockedChaptersCount; i++)
        {
            for (int o = 0; o < cm[i].Maps.Length; o++)
            {
                if(!cm[i].Maps[o].Display.Locked)
                {
                    unlockedMapCount++;
                }
            }
            for (int p = 0; p < unlockedMapCount; p++)
            {
                float[,] scoreStrikes = new float[unlockedMapCount,1];
                float[,] scoreTimer = new float[unlockedMapCount, 1];

                scoreStrikes[p, 0] = cm[i].Maps[p].Display.Map.PB.Strikes;
                scoreTimer[p, 0] = cm[i].Maps[p].Display.Map.PB.Time;

                SaveSystem.SaveMapProgressScore_Strikes(i, scoreStrikes);
                SaveSystem.SaveMapProgressScore_Timer(i, scoreTimer);
            }
            unlockedMapCount = 0;
        }
    }

    public void LoadMapProgress()
    {
        SaveData data = SaveSystem.LoadData();
        for (int i = 0; i < MapManager.Instance.ChapterMaps.Count; i++)
        {
            switch (i)
            {
                case 0:
                    for (int o = 0; o < data.Chapter01_Score_Strikes.Length; o++)
                    {
                        MapManager.Instance.ChapterMaps[i].Maps[o].PB.Strikes = (int)data.Chapter01_Score_Strikes[o,0];
                    }
                    break;
                default:
                    break;
            }
        }
    }

}
