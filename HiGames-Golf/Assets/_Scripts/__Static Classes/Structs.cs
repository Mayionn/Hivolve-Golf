using System;
using UnityEngine;
using UnityEngine.UI;

public static class Struct
{
    //|--------------------------------------------------------------------------
    // PLAYER'S BEST SCORE
    //|--------------------------------------------------------------------------
    public struct PersonalBest
    {
        public int Strikes;
        public float Time;
    } //Player best Score

    //|--------------------------------------------------------------------------
    // DISPLAY'S INFO
    //|--------------------------------------------------------------------------
    [Serializable]
    public struct DisplayInfo
    {
        public RectTransform pos;
        public Sprite SpriteLevel;
        public Sprite SpriteLocked;
        public Sprite SpriteUnlocked;
    }

    //|--------------------------------------------------------------------------
    // UI INFO
    //|--------------------------------------------------------------------------
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
        public Image Paper;
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
    } //UI_CompletedMap1
    [Serializable]
    public struct InfoOneShotMap
    {
        public Image Image_CurrentTimer;
        public Text Txt_CurrTimer;
        public Image Image_MoneyIcon;
        public Text Txt_MoneyEarned;
    }
    [Serializable]
    public struct InfoLocalGrid
    {
        public int PlayerNum;
        public Image Image;
        public Image ImageBall;
        public Text Txt_PlayerNum;
        [HideInInspector] public Ball SelectedBall;
    } //UI_Local Mutliplayer
    [Serializable]
    public struct InfoScoreboard
    {
        public Image Image_Background;
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
        public Text Text_PlayerNumber;
        public Text Text_PlayerScore;
        public Image Image_Background;
        public Image Image_Medal;
    } //UI_LocalScoreBoard
    [Serializable]
    public struct UILocalReadyCheck
    {
        public Image Background;
        public Image Image_Button;
        public Text Text_CurrentPlayer;
    } //UI_Ready Check

    //|--------------------------------------------------------------------------
    // COLORS
    //|--------------------------------------------------------------------------
    [Serializable]
    public struct UIColors
    {
        public string OneShotComplete;
        public string OneShotIncomplete;
    }
    [Serializable]
    public struct MapColor
    {
        [SerializeField] private string name;
        public string Hole;
        public string Floor;
        public string MapObjects;
        public string BackgroundObjects;
        public string Background;
        public string Skybox;
    }


    [Serializable]
    public struct ChapterInfo
    {
        public string Name;
        public Sprite BackgroundSprite;
        public Map[] Maps;
        public DisplayInfo[] DisplayInfos;
    }
}
