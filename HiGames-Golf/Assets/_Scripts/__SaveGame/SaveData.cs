﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SaveData
{
    [System.Serializable]
    public class SaveData
    {
        //-Currency
        public int Gold;
        public int Diamonds;

        //--Skins
        //TODO: ADD MORE SKIN TYPES
        public int CurrentSkin_Hat_Index;
        public int CurrentSkin_Ball_Index;
        public int[] UnlockedSkins_Hats;
        public int[] UnlockedSkins_Balls;

        //Map Progress
        public float[,] Chapter01_Score_Strikes;
        public float[,] Chapter01_Score_Timer;

        public SaveData()
        {
            Gold = 0;
            Diamonds = 0;
            CurrentSkin_Hat_Index = 0;
            CurrentSkin_Ball_Index = 0;
            UnlockedSkins_Hats = new int[0];
            UnlockedSkins_Balls = new int[0];
        }

        #region -Currency
        public void Setup_Currency(int gold, int diamonds)
        {
            Gold = gold;
            Diamonds = diamonds;
        }
        #endregion

        #region --Skins
        public void SetupCurrentSkin_Ball(int index)
        {
            CurrentSkin_Ball_Index = index;
        }
        public void SetupCurrentSkin_Hat(int index)
        {
            CurrentSkin_Hat_Index = index;
        }
        public void SetupSkins_Balls(int count, int[] indexes)
        {
            UnlockedSkins_Balls = new int[count];
            for (int i = 0; i < count; i++)
            {
                UnlockedSkins_Balls[i] = indexes[i];
            }
        }
        public void SetupSkins_Hats(int count, int[] indexes)
        {
            UnlockedSkins_Hats = new int[count];
            for (int i = 0; i < count; i++)
            {
                UnlockedSkins_Hats[i] = indexes[i];
            }
        }
        #endregion

        #region ---Map Progress
        public void SetupMapProgressScore_Strikes(int chapter, float[,] score)
        {
            switch (chapter)
            {
                case 0:
                    Chapter01_Score_Strikes = score;
                    break;
                default:
                    break;
            }
        }
        public void SetupMapProgressScore_Timer(int chapter, float[,] score)
        {
            switch (chapter)
            {
                case 0:
                    Chapter01_Score_Timer = score;
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
