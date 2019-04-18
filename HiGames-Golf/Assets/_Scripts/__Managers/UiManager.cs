using Assets.Generics;
using Assets.Managers;
using Assets.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{
    //Structs
    [Serializable] public struct UIImages
    {
        public Sprite GoldMedal;
        public Sprite SilverMedal;
        public Sprite BronzeMedal;
        public Sprite StopWatch;
        public Sprite Waypoint;
        public Sprite Strikes;
        public Sprite Hidden;
        public Sprite Reset;
    }
    [Serializable] public struct UIBackgroundImages
    {
        public Sprite DefaultBackground;
    }
    [Serializable] public struct UILocalReadyCheck
    {
        public Sprite Background;
        public Sprite Sprite_Button;
    }
    [Serializable] public struct InfoCompletedMap
    {
        public GameObject Go;
        public Text Txt_Medal01;
        public Text Txt_Medal02;
        public Text Txt_Medal03;
        public Text Txt_PBStrikes;
        public Text Txt_PBTimer;
        public Text Txt_CurrStrikes;
        public Text Txt_CurrTimer;
        public Image Img_PBStrikes;
        public Image Img_CurrStrikes;
    }
    [Serializable] public struct InfoLocalScoreboard
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
    }
    
    //Struct Variables
    public UIImages UI_Images;
    public UIBackgroundImages UI_BackgroundImages;
    public InfoCompletedMap UI_CompletedMap;
    public List<InfoLocalScoreboard> UI_LocalScoreboard;

    public GameObject GO_LocalScoreboard;

    private UI_LocalMultiplayer UI_LocalMultiplayer;
    private UI_MapSelector UI_MapSelector;
    private UI_InGameHud UI_InGameHud;
    
    //CurrentMap and Player
    private Map m;
    private Player p;

    public void Init()
    {
        UI_LocalMultiplayer = GetComponent<UI_LocalMultiplayer>();
        UI_MapSelector = GetComponent<UI_MapSelector>();
        UI_InGameHud = GetComponent<UI_InGameHud>();
    }

    //Open / Close --- Interface
    public void OpenInterface_InGameHud()
    {
        UI_InGameHud.Init();
    }
    public void OpenIngerface_InGameReadyCheck()
    {

    }
    public void OpenInterface_MapSelector()
    {
        GameManager.Instance.TimeScaleStop();
        UI_MapSelector.UI.SetActive(true);
        UI_MapSelector.Init();
    }
    public void OpenInterface_LocalMultiplayer()
    {
        GameManager.Instance.TimeScaleStop();
        UI_LocalMultiplayer.UI.SetActive(true);
        UI_LocalMultiplayer.Init();
    }
    public void OpenInterface_CompletedMap()
    {
        GameManager.Instance.TimeScaleStop();
        CloseInterface_InGameHud();
        UI_CompletedMap.Go.SetActive(true);
        CM_Init();
    }
    public void OpenInterface_LocalScoreboard()
    {
        GameManager.Instance.TimeScaleStop();
        CloseInterface_InGameHud();
        LGS_CheckBestPlayerOnMap();
        LGS_GivePlayersScores();
        GO_LocalScoreboard.SetActive(true);
    }

    public void CloseInterface_InGameHud()
    {
        UI_InGameHud.CloseInterface();
    }
    public void CloseInterface_MapSelector()
    {
        GameManager.Instance.TimeScaleResume();
        UI_MapSelector.CloseInterface();
        //UI_MapSelector.UI = UiManager.Instance.GO_MapSelector; //Necessario caso contrario dá um erro que me fez questionar a minha religião
        UI_MapSelector.UI.SetActive(false);
    }
    public void CloseInterface_LocalMultiplayer()
    {
        GameManager.Instance.TimeScaleResume();
        UI_LocalMultiplayer.UI.SetActive(false);
    }
    public void CloseInterface_CompletedMap()
    {
        GameManager.Instance.TimeScaleResume();
        UI_CompletedMap.Go.SetActive(false);
    }
    public void CloseInterface_LocalScoreboard()
    {
        GameManager.Instance.TimeScaleResume();
        GO_LocalScoreboard.SetActive(false);
    }

    //CM --- Completed Map
    private void CM_Init()
    {
        m = GameManager.Instance.CurrentMap;
        p = GameManager.Instance.CurrentPlayer;

        m.CheckPersonalBest();
        if(m.PB.Strikes < m.MedalBronze)
        {
           UI_MapSelector.UnlockNextLevel(m.Display.levelNumber);
        }
        //UPDATE TEXT
        //Map Medals
        UI_CompletedMap.Txt_Medal01.text = m.MedalGold.ToString();
        UI_CompletedMap.Txt_Medal02.text = m.MedalSilver.ToString();
        UI_CompletedMap.Txt_Medal03.text = m.MedalBronze.ToString();
        //PB
        UI_CompletedMap.Txt_PBStrikes.text = m.PB.Strikes.ToString();
        UI_CompletedMap.Txt_PBTimer.text = m.PB.Time.ToString();
        //Current Score
        UI_CompletedMap.Txt_CurrStrikes.text = p.Strikes.ToString();
        UI_CompletedMap.Txt_CurrTimer.text = p.Timer.ToString();
        //UPDATE IMAGES
        CM_UpdateScoreImages();
    }
    public void CM_ButtonReset()
    {
        CloseInterface_CompletedMap();
        OpenInterface_InGameHud();
        GameManager.Instance.ResetGame();
    }
    public void CM_ButtonMenu()
    {
        CloseInterface_CompletedMap();
        OpenInterface_MapSelector();
        //GameManager.Instance.SetupMenuMap();
    }
    public void CM_UpdateScoreImages()
    {
        //UPDATE PERSONAL BEST - PB
        if (m.PB.Strikes <= m.MedalGold)
        {
            UI_CompletedMap.Img_PBStrikes.sprite = UI_Images.GoldMedal;
        }
        else if (m.PB.Strikes <= m.MedalSilver)
        {
            UI_CompletedMap.Img_PBStrikes.sprite = UI_Images.SilverMedal;
        }
        else if (m.PB.Strikes <= m.MedalBronze)
        {
            UI_CompletedMap.Img_PBStrikes.sprite = UI_Images.BronzeMedal;
        }

        //UPDATE CURRENT SCORE
        if (p.Strikes <= m.MedalGold)
        {
            UI_CompletedMap.Img_CurrStrikes.sprite = UI_Images.GoldMedal;
        }
        else if (p.Strikes <= m.MedalSilver)
        {
            UI_CompletedMap.Img_CurrStrikes.sprite = UI_Images.SilverMedal;
        }
        else if (p.Strikes <= m.MedalBronze)
        {
            UI_CompletedMap.Img_CurrStrikes.sprite = UI_Images.BronzeMedal;
        }
        else
        {
            UI_CompletedMap.Img_CurrStrikes.sprite = null;
        }
    }

    //LGS --- Local Game Scoreboard
    public void LGS_Init()
    {
        LGS_SetImages();
        LGS_HideRows();


    }
    public void LGS_BUTTON_Menu()
    {
        CloseInterface_LocalScoreboard();

        GameManager.Instance.RemoveLocalgamePlayers();

        GameManager.Instance.SetupMenuMap();
    }
    public void LGS_SaveScore(Player p)
    {
        InfoLocalScoreboard i = UI_LocalScoreboard[p.PlayerNum];
        i.PlayerName.text = p.Name;
        i.PlayerNumber.text = "Player: " + p.PlayerNum.ToString();
        i.PlayerIndexNumber.text = p.PlayerNum.ToString();
        i.PlayerStrikes.text = p.Strikes.ToString();
        p.TruncateTimer();
        i.PlayerTimer.text = p.Timer.ToString();
        i.TotalPoints.text = p.LocalgamePoints.ToString();
    }
    private void LGS_SetImages()
    {
        for (int i = 0; i < UI_LocalScoreboard.Count; i++)
        {
            LGS_ChangeImages(UI_LocalScoreboard[i]);
        }
    }
    private void LGS_HideRows()
    {
        for (int i = 0; i < UI_LocalScoreboard.Count; i++)
        {
            if (i >= GameManager.Instance.Players.Count)
            {
                LGS_Hide(UI_LocalScoreboard[i]);
            }
        }
    }
    private void LGS_ChangeImages(InfoLocalScoreboard i)
    {
        i.ImgTimer.sprite = UI_Images.StopWatch;
        i.ImgStrikes.sprite = UI_Images.Strikes;
    }
    private void LGS_Hide(InfoLocalScoreboard i)
    {
        i.PlayerName.text = "";
        i.PlayerNumber.text = "";
        i.PlayerTimer.text = "";
        i.ImgTimer.sprite = UI_Images.Hidden;
        i.PlayerStrikes.text = "";
        i.ImgStrikes.sprite = UI_Images.Hidden;
        i.TotalPoints.text = "";
        i.Medal.sprite = UI_Images.Hidden;
    }
    private void LGS_CheckBestPlayerOnMap()
    {
        //int count = GameManager.Instance.Players.Count;
        //List<InfoLocalScoreboard> li = new List<InfoLocalScoreboard>(count);
        //for (int i = 0; i < count; i++)
        //{
        //    li[i] = UI_LocalScoreboard[i];
        //}

        UI_LocalScoreboard.Sort(
            delegate (InfoLocalScoreboard p1, InfoLocalScoreboard p2)
            {
                if (p1.PlayerTimer.text != "" && p2.PlayerTimer.text != "")
                {
                    int compareStrikes = p1.PlayerStrikes.text.CompareTo(p2.PlayerStrikes.text);
                    if (compareStrikes == 0)
                    {
                        return p1.PlayerTimer.text.CompareTo(p2.PlayerTimer.text);
                    }
                    return compareStrikes;
                }
                else return 1;
            }
        );
    }
    private void LGS_GivePlayersScores()
    {
        /* Changes the medal icon on the scoreboard depending on the performance of the player on the current map;
         * Also adds points to the players depending on their position;
         * Follows the order of the players --- this happens after ordering the list by numberOfStrikes; */

        for (int i = 0; i < GameManager.Instance.Players.Count; i++)
        {
            int stPlace = 4;
            int ndPlace = 3;
            int rdPlace = 2;
            int thPlace = 1;
            if (UI_LocalScoreboard[i].PlayerTimer.text != "")
            {
                switch (i)
                {
                    case 0:
                        LGS_SetScore(UI_LocalScoreboard[i], UI_Images.GoldMedal, stPlace);
                        break;
                    case 1:
                        LGS_SetScore(UI_LocalScoreboard[i], UI_Images.SilverMedal, ndPlace);
                        break;
                    case 2:
                        LGS_SetScore(UI_LocalScoreboard[i], UI_Images.BronzeMedal, rdPlace);
                        break;
                    default:
                        LGS_SetScore(UI_LocalScoreboard[i], UI_Images.Hidden, thPlace);
                        break;
                }
            }
        }
    }
    private void LGS_SetScore(InfoLocalScoreboard i, Sprite s, int p)
    {
        Player lp = GameManager.Instance.Players[int.Parse(i.PlayerIndexNumber.text)];
        i.TotalPoints.text = lp.LocalgamePoints.ToString() + " + " + p;
        lp.LocalgamePoints += p;
        i.Medal.sprite = s;
    }

    //Update Info Methods
    public void UpdateCurrentPlayerName()
    {
        UI_InGameHud.SetCurrentPlayerInfo();
    }
    public void UpdateMapInfoWaypoints()
    {
        p = GameManager.Instance.CurrentPlayer;
        m = GameManager.Instance.CurrentMap;
        p.WaypointCounter = 0;
        //Percorrer Waypoints!
        for (int i = 0; i < m.Waypoints.Length; i++)
        {
            List<int> rp = m.Waypoints[i].GetComponent<Waypoint>().ReachedPlayers;
            //Percorrer ReachedPlayers em cada Waypoint!
            for (int o = 0; o < rp.Count; o++)
            {
                //Se o Player num estiver na lista,
                //é porque o player já chegou ao determinado waypoint
                if (rp[o] == p.PlayerNum)
                {
                    //Incrementar o contador de strikes, no jogado atual
                    p.WaypointCounter++;
                }
            }
        }
        UI_InGameHud.UI_InGame.Waypoint.text = m.Waypoints.Length + " \\ " + p.WaypointCounter;
    }
    public void UpdateMapInfoCurrentStrikes()
    {
        UI_InGameHud.UI_InGame.CurrentStrikes.text = "Strikes: " + GameManager.Instance.CurrentPlayer.Strikes;
    }
}