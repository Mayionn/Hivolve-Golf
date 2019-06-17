using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SaveData
{
    [System.Serializable]
    public class SaveData
    {
        //----- CURRENCY
        public int Gold;
        public int Diamonds; // NOT BEING USED ATM

        //----- SKINS
        public int CurrentSkin_Hat_Index;
        public int CurrentSkin_Ball_Index;
        public int CurrentSkin_Arrow_Index;
        public int CurrentSkin_ForceBar_Index;
        public int[] UnlockedSkins_Hats;
        public int[] UnlockedSkins_Balls;
        public int[] UnlockedSkins_Arrows;
        public int[] UnlockedSkins_ForceBars;

        //----- MAP PROGRESS
        public float[][] Chapter_Strikes;
        public float[][] Chapter_Timer;

        public SaveData()
        {
            Gold = 0;
            Diamonds = 0;
            CurrentSkin_Hat_Index = 0;
            CurrentSkin_Ball_Index = 0;
            CurrentSkin_Arrow_Index = 0;
            UnlockedSkins_Hats = new int[0];
            UnlockedSkins_Balls = new int[0];
            UnlockedSkins_Arrows = new int[0];
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
        public void SetupCurrentSkin_Arrow(int index)
        {
            CurrentSkin_Arrow_Index = index;
        }
        public void SetupCurrentSkin_ForceBar(int index)
        {
            CurrentSkin_ForceBar_Index = index;
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
        public void SetupSkins_Arrows(int count, int[] indexes)
        {
            UnlockedSkins_Arrows = new int[count];
            for (int i = 0; i < count; i++)
            {
                UnlockedSkins_Arrows[i] = indexes[i];
            }
        }
        public void SetupSkins_ForceBars(int count, int[] indexes)
        {
            UnlockedSkins_ForceBars = new int[count];
            for (int i = 0; i < count; i++)
            {
                UnlockedSkins_ForceBars[i] = indexes[i];
            }
        }
        #endregion

        #region ---Map Progress
        public void SetupMapProgressScore(float[][] scoreStrikes, float[][] scoreTimer)
        {
            Chapter_Strikes = scoreStrikes;
            Chapter_Timer = scoreTimer;
        }
        //public void SetupMapProgressScore_Strikes(float[][] score)
        //{
        //    Chapter_Strikes = score;
        //    //switch (chapter)
        //    //{
        //    //    case 0:
        //    //        Score_Strikes = score;
        //    //        break;
        //    //    case 1:
        //    //        Chapter02_Score_Strikes = score;
        //    //        break;
        //    //    default:
        //    //        break;
        //    //}
        //}
        //public void SetupMapProgressScore_Timer(float[,] score)
        //{
        //    Score_Timer = score;
        //    //switch (chapter)
        //    //{
        //    //    case 0:
        //    //        Chapter01_Score_Timer = score;
        //    //        break;
        //    //    case 1:
        //    //        Chapter02_Score_Timer = score;
        //    //        break;
        //    //    default:
        //    //        break;
        //    //}
        //}
        #endregion
    }
}
