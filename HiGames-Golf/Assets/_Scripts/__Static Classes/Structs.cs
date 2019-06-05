using System;
using UnityEngine;
using UnityEngine.UI;

public static class Struct
{
    public struct PersonalBest
    {
        public int Strikes;
        public float Time;
    } //Player best Score

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
    [Serializable]
    public struct Info_SkinMenu
    {
        public Text Title;
        public Image Img_Coin;
        public Image Img_Diamond;
        public Text Text_Coin;
        public Text Text_Diamond;
        public Button Button_Back;
    }
    [Serializable]
    public struct InfoCompletedMap
    {
        public Image Image_MapGoldMedal;
        public Image Image_MapSilverMedal;
        public Image Image_MapBronzeMedal;
        public Text Txt_GoldMedal;
        public Text Txt_SilverMedal;
        public Text Txt_BronzeMedal;

        public Image Img_PBStrikes;
        public Image Image_PBTimer;
        public Text Txt_PBStrikes;
        public Text Txt_PBTimer;

        public Image Img_CurrStrikes;
        public Image Image_CurrentTimer;
        public Text Txt_CurrStrikes;
        public Text Txt_CurrTimer;
    } //UI_CompletedMap
    [Serializable]
    public struct InfoLocalGrid
    {
        public int PlayerNum;
        public string PlayerName;
        public Image Image;
        public Ball SelectedBall;
        public Text Txt_PlayerNum;
        public Text Txt_PlayerName;
    } //UI_Local Mutliplayer
    [Serializable]
    public struct InfoScoreboard
    {
        public Text PlayerName;
        public Text PlayerNumber;
        public Text PlayerIndexNumber;
        public Text PlayerTimer;
        public Image ImgTimer;
        public Text PlayerStrikes;
        public Image ImgStrikes;
        public Text TotalPoints;
        public Image Medal;
    } //UI_Local Scoreboard
    [Serializable]
    public struct InfoResults
    {
        public Text Text_PlayerName;
        public Text Text_PlayerScore;
        public Image Image_Medal;
    } //UI_LocalScoreBoard
    [Serializable]
    public struct UILocalReadyCheck
    {
        public Image Background;
        public Image Image_Button;
        public Text Text_CurrentPlayer;
    } //UI_Ready Check

    
}
