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

    private UI_LocalMultiplayer UI_LocalMultiplayer;
    private UI_MapSelector UI_MapSelector;
    private UI_InGameHud UI_InGameHud;
    private UI_ReadyCheck UI_ReadyCheck;
    private UI_LocalScoreboard UI_LocalScoreboard;
    
    //CurrentMap and Player
    private Map m;
    private Player p;

    public void Init()
    {
        UI_LocalMultiplayer = GetComponent<UI_LocalMultiplayer>();
        UI_MapSelector = GetComponent<UI_MapSelector>();
        UI_InGameHud = GetComponent<UI_InGameHud>();
        UI_ReadyCheck = GetComponent<UI_ReadyCheck>();
        UI_LocalScoreboard = GetComponent<UI_LocalScoreboard>();
    }

    //Open / Close --- Interface
    public void OpenInterface_InGameHud()
    {
        UI_InGameHud.Init();
    }
    public void OpenInterface_InGameReadyCheck()
    {
        GameManager.Instance.TimeScaleStop();
        UI_ReadyCheck.Init();
    }
    public void OpenInterface_MapSelector()
    {
        GameManager.Instance.TimeScaleStop();
        UI_MapSelector.UI.SetActive(true);
        UI_MapSelector.Init();
    }
    public void OpenInterface_CompletedMap()
    {
        GameManager.Instance.TimeScaleStop();
        CloseInterface_InGameHud();
        CM_Init();
    }
    public void OpenInterface_LocalMultiplayer()
    {
        GameManager.Instance.TimeScaleStop();
        UI_LocalMultiplayer.Init();
    }
    public void OpenInterface_LocalScoreboard()
    {
        GameManager.Instance.TimeScaleStop();
        CloseInterface_InGameHud();
        UI_LocalScoreboard.Init();
    }

    public void CloseInterface_InGameHud()
    {
        UI_InGameHud.CloseInterface();
    }
    public void CloseInterface_InGameReadyCheck()
    {
        GameManager.Instance.TimeScaleResume();
        UI_ReadyCheck.Terminate();
    }
    public void CloseInterface_MapSelector()
    {
        GameManager.Instance.TimeScaleResume();
        UI_MapSelector.CloseInterface();
        //UI_MapSelector.UI = UiManager.Instance.GO_MapSelector; //Necessario caso contrario dá um erro que me fez questionar a minha religião
        UI_MapSelector.UI.SetActive(false);
    }
    public void CloseInterface_CompletedMap()
    {
        GameManager.Instance.TimeScaleResume();
        UI_CompletedMap.Go.SetActive(false);
    }
    public void CloseInterface_LocalMultiplayer()
    {
        GameManager.Instance.TimeScaleResume();
        UI_LocalMultiplayer.UI.SetActive(false);
    }
    public void CloseInterface_LocalScoreboard()
    {
        GameManager.Instance.TimeScaleResume();
        UI_LocalScoreboard.Terminate();
    }

    //CM --- Completed Map
    private void CM_Init()
    {
        UI_CompletedMap.Go.SetActive(true);

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

    //Update Info Methods
    public void Update_ScoreBoard_Rows()
    {
        UI_LocalScoreboard.SetupRows();
    }
    public void Update_ScoreBoard_SaveScore(Player player)
    {
        UI_LocalScoreboard.SaveScore(player);
    }
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