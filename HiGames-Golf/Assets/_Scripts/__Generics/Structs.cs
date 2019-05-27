using System;
using UnityEngine;
using UnityEngine.UI;

public static class Struct
{
    public struct PersonalBest
    {
        public int Strikes;
        public float Time;
    }

    [Serializable]
    public struct DisplayInfo
    {
        public RectTransform pos;
        public Sprite SpriteLevel;
        public Sprite SpriteLocked;
        public Sprite SpriteUnlocked;
    }
    [Serializable]
    public struct InfoInGame
    {
        public Text MedalGold;
        public Text MedalSilver;
        public Text MedalBronze;
        public Image ImgMedalGold;
        public Image ImgMedalSilver;
        public Image ImgMedalBronze;
        public Text CurrentTime;
        public Text CurrentStrikes;
        public Image ImgCurrentStrikes;
        public Text Waypoint;
        public Image ImgWaypoint;
        public Button ResetGame;
        public Text MapInfo;
        public Text CurrentPlayerInfo;
        public Button SkinMenu;
        public Button MapSelector;
    }


}
