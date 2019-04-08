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
        public Sprite Reset;
    }
    [Serializable] public struct InfoInGame
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
        public Text BestTime;
        public Text BestStrikes;
        public Button ResetGame;
        public Text MapInfo;
        public Text CurrentPlayerInfo;
    }
    [Serializable] public struct InfoLocalGrid
    {
        public int PlayerNum;
        public string PlayerName;
        public Image Image;
        public Ball SelectedBall;
        public Text Txt_PlayerNum;
        public Text Txt_PlayerName;
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

    //Struct Variables
    public UIImages UI_Images;
    public InfoInGame UI_InGame;
    public InfoCompletedMap UI_CompletedMap;
    
    public GameObject UI_InGame_GO;
    public UI_LocalMultiplayer UI_LocalMultiplayer;
    //CurrentMap and Player
    private Map m;
    private Player p;

    //Open / Close --- Interface
    public void OpenInterface_InGameHud()
    {
        m = GameManager.Instance.CurrentMap;
        switch (GameManager.Instance._GameState)
        {
            case GameManager.GameState.Singleplayer:
                UI_InGame_GO.SetActive(true);
                TimerStart();
                IGH_SetCurrentPlayerInfo();
                IGH_SetMapInfo();
                IGH_SetMapInfoMedals();
                IGH_SetMapInfoWaypoints();
                IGH_SetMapInfoCurrentStrikes();
                break;
            case GameManager.GameState.Multiplayer:
                break;
            case GameManager.GameState.Localgame:
                UI_InGame_GO.SetActive(true);
                TimerStart();
                IGH_SetCurrentPlayerInfo();
                IGH_SetMapInfo();
                IGH_SetMapInfoMedals();
                IGH_SetMapInfoWaypoints();
                IGH_SetMapInfoCurrentStrikes();
                break;
            default:
                break;
        }
    }
    public void OpenInterface_LocalMultiplayer()
    {
        GameManager.Instance.TimeScaleStop();
        UI_LocalMultiplayer.gameObject.SetActive(true);
        UI_LocalMultiplayer.Init();
    }
    public void OpenInterface_CompletedMap()
    {
        GameManager.Instance.TimeScaleStop();
        CloseInterface_InGameHud();
        UI_CompletedMap.Go.SetActive(true);
        CM_Init();
    }
    public void CloseInterface_InGameHud()
    {
        UI_InGame_GO.SetActive(false);
        TimerStop();
    }
    public void CloseInterface_LocalMultiplayer()
    {
        GameManager.Instance.TimeScaleResume();
        UI_LocalMultiplayer.gameObject.SetActive(false);
    }
    public void CloseInterface_CompletedMap()
    {
        GameManager.Instance.TimeScaleResume();
        UI_CompletedMap.Go.SetActive(false);
    }

    //IGH --- In Game Hud
    private void IGH_SetCurrentPlayerInfo()
    {
        p = GameManager.Instance.CurrentPlayer;
        UI_InGame.CurrentPlayerInfo.text = "Current Player: " + p.Name
                                                + "\nPlayer Number: " + p.PlayerNum;
    }
    private void IGH_SetMapInfo()
    {
        UI_InGame.MapInfo.text = m.gameObject.name + "\n" + m.Author;
    }
    private void IGH_SetMapInfoMedals()
    {
        UI_InGame.MedalGold.text = m.MedalGold.ToString();
        UI_InGame.ImgMedalGold.sprite = UI_Images.GoldMedal;
        UI_InGame.MedalSilver.text = m.MedalSilver.ToString();
        UI_InGame.ImgMedalSilver.sprite = UI_Images.SilverMedal;
        UI_InGame.MedalBronze.text = m.MedalBronze.ToString();
        UI_InGame.ImgMedalBronze.sprite = UI_Images.BronzeMedal;
    }
    private void IGH_SetMapInfoWaypoints()
    {
        if (m.Waypoints.Length > 0)
        {
            UI_InGame.Waypoint.gameObject.SetActive(true);
            UI_InGame.ImgWaypoint.sprite = UI_Images.Waypoint;
            UI_InGame.Waypoint.text = m.Waypoints.Length + " \\ " + 0;
        }
        else
        {
            UI_InGame.Waypoint.gameObject.SetActive(false);
        }
    }
    private void IGH_SetMapInfoCurrentStrikes()
    {
        UI_InGame.ImgCurrentStrikes.sprite = UI_Images.Strikes;
        UI_InGame.CurrentStrikes.text = "Strikes: 0";
    }

    //CM --- Completed Map
    private void CM_Init()
    {
        m = GameManager.Instance.CurrentMap;
        p = GameManager.Instance.CurrentPlayer;

        m.CheckPersonalBest();
        //TODO: UPDATE PB and CURRENT SCORE IMAGES - CREATE STRUCT
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
        GameManager.Instance.SetupMenuMap();
    }
    public void CM_UpdateScoreImages()
    {
        //UPDATE PERSONAL BEST - PB
        if(m.PB.Strikes <= m.MedalGold)
        {
            UI_CompletedMap.Img_PBStrikes.sprite = UI_Images.GoldMedal;
        }
        else if(m.PB.Strikes <= m.MedalSilver)
        {
            UI_CompletedMap.Img_PBStrikes.sprite = UI_Images.SilverMedal;
        }
        else if(m.PB.Strikes <= m.MedalBronze)
        {
            UI_CompletedMap.Img_PBStrikes.sprite = UI_Images.BronzeMedal;
        }

        //UPDATE CURRENT SCORE
        if(p.Strikes <= m.MedalGold)
        {
            UI_CompletedMap.Img_CurrStrikes.sprite = UI_Images.GoldMedal;
        }
        else if(p.Strikes <= m.MedalSilver)
        {
            UI_CompletedMap.Img_CurrStrikes.sprite = UI_Images.SilverMedal;
        }
        else if(p.Strikes <= m.MedalBronze)
        {
            UI_CompletedMap.Img_CurrStrikes.sprite = UI_Images.BronzeMedal;
        }
        else 
        {
            UI_CompletedMap.Img_CurrStrikes.sprite = null;
        }
    }
    
    //Update Info Methods
    public void UpdateCurrentPlayerName()
    {
        IGH_SetCurrentPlayerInfo();
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
        UI_InGame.Waypoint.text = m.Waypoints.Length + " \\ " + p.WaypointCounter;
    }
    public void UpdateMapInfoCurrentStrikes()
    {
        UI_InGame.CurrentStrikes.text = "Strikes: " + GameManager.Instance.CurrentPlayer.Strikes;
    }

    //Timer Functions
    private void TimerStart()
    {
        GameManager.Instance.ActUpdate += TimerCount;
    }
    private void TimerCount()
    {
        GameManager.Instance.CurrentPlayer.Timer += Time.deltaTime;
        float t = GameManager.Instance.CurrentPlayer.Timer;
        //UI_InGame.Time.text = timer.ToString();
        UI_InGame.CurrentTime.text = string.Format("{0:#0} : {1:00} : {2:00}", 
                                Mathf.Floor(t / 60),
                                Mathf.Floor(t) % 60,
                                Mathf.Floor((t * 100) % 100));
    }
    private void TimerReset()
    {
        GameManager.Instance.CurrentPlayer.Timer = 0;
    }
    private void TimerStop()
    {
        GameManager.Instance.ActUpdate -= TimerCount;
    }
}