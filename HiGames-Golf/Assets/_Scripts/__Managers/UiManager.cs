using Assets.Generics;
using Assets.Managers;
using Assets.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{
    [Serializable] public struct InfoInGame
    {
        public Text MedalGold;
        public Text MedalSilver;
        public Text MedalBronze;
        public Text CurrentTime;
        public Text CurrentStrikes;
        public Text Waypoint;
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
        public Text Txt_PBMedal;
        public Text Txt_PBTimer;
        public Text Txt_CurrMedal;
        public Text Txt_CurrTimer;
        public Image Img_Medal01;
        public Image Img_Medal02;
        public Image Img_Medal03;
        public Image Img_PBMedal;
        public Image Img_PBTimer;
        public Image Img_CurrMedal;
        public Image Img_CurrTimer;
    }

    public UI_InGame UI_InGame;
    public GameObject UI_InGame_GO;
    public InfoCompletedMap UI_CompletedMap;

    public UI_LocalMultiplayer UI_LocalMultiplayer;
    private Map map;


    public void OpenInterface_LocalMultiplayer()
    {
        GameManager.Instance.TimeScaleStop();
        UI_LocalMultiplayer.gameObject.SetActive(true);
        UI_LocalMultiplayer.Init();
    }
    public void OpenInterface_CompletedMap()
    {
        GameManager.Instance.TimeScaleStop();
        InGameUIClose();
        UI_CompletedMap.Go.SetActive(true);
        CM_Init();
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

    public void InGameUIOpen()
    {
        map = GameManager.Instance.CurrentMap;
        switch (GameManager.Instance._GameState)
        {
            case GameManager.GameState.Singleplayer:
                UI_InGame_GO.SetActive(true);
                TimerStart();
                SetCurrentPlayerInfo();
                SetMapInfo();
                SetMapInfoMedals();
                SetMapInfoWaypoints();
                SetMapInfoCurrentStrikes();
                break;
            case GameManager.GameState.Multiplayer:
                break;
            case GameManager.GameState.Localgame:
                TimerStart();
                SetCurrentPlayerInfo();
                SetMapInfo();
                SetMapInfoMedals();
                SetMapInfoWaypoints();
                SetMapInfoCurrentStrikes();
                break;
            default:
                break;
        }
    }
    public void InGameUIClose()
    {
        UI_InGame_GO.SetActive(false);
        TimerStop();
    }

    private void SetCurrentPlayerInfo()
    {
        Player p = GameManager.Instance.CurrentPlayer;
        UI_InGame.CurrentPlayerInfo.text = "Current Player: " + p.Name
                                                + "\nPlayer Number: " + p.PlayerNum;
    }
    private void SetMapInfo()
    {
        UI_InGame.MapInfo.text = map.gameObject.name + "\n" + map.Author;
    }
    private void SetMapInfoMedals()
    {
        UI_InGame.MedalGold.text = map.MedalGold.ToString();
        UI_InGame.MedalSilver.text = map.MedalSilver.ToString();
        UI_InGame.MedalBronze.text = map.MedalBronze.ToString();
    }
    private void SetMapInfoWaypoints()
    {
        if (map.Waypoints.Length > 0)
        {
            UI_InGame.Waypoint.gameObject.SetActive(true);
            UI_InGame.Waypoint.text = map.Waypoints.Length + " \\ " + 0;
        }
        else
        {
            UI_InGame.Waypoint.gameObject.SetActive(false);
        }
    }
    private void SetMapInfoCurrentStrikes()
    {
        UI_InGame.CurrentStrikes.text = "Strikes: 0";
    }

    public void UpdateCurrentPlayerName()
    {
        SetCurrentPlayerInfo();
    }
    public void UpdateMapInfoWaypoints()
    {
        Player p = GameManager.Instance.CurrentPlayer;
        Map m = GameManager.Instance.CurrentMap;
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
        UI_InGame.Waypoint.text = map.Waypoints.Length + " \\ " + p.WaypointCounter;
    }
    public void UpdateMapInfoCurrentStrikes()
    {
        UI_InGame.CurrentStrikes.text = "Strikes: " + GameManager.Instance.CurrentPlayer.Strikes;
    }

    //CM --- CompletedMap
    private void CM_Init()
    {
        Map m = GameManager.Instance.CurrentMap;
        Player p = GameManager.Instance.CurrentPlayer;
        m.CheckPersonalBest();
        //TODO: UPDATE PB and CURRENT SCORE IMAGES - CREATE STRUCT
        UI_CompletedMap.Txt_Medal01.text = m.MedalGold.ToString();
        UI_CompletedMap.Txt_Medal02.text = m.MedalSilver.ToString();
        UI_CompletedMap.Txt_Medal03.text = m.MedalBronze.ToString();
        UI_CompletedMap.Txt_PBMedal.text = m.PB.Medal.ToString();
        UI_CompletedMap.Txt_PBTimer.text = m.PB.Time.ToString();
        UI_CompletedMap.Txt_CurrMedal.text = p.Strikes.ToString();
        UI_CompletedMap.Txt_CurrTimer.text = p.Timer.ToString();
    }
    public void CM_ButtonReset()
    {
        CloseInterface_CompletedMap();
        InGameUIOpen();
        GameManager.Instance.ResetGame();
    }
    public void CM_ButtonMenu()
    {
        CloseInterface_CompletedMap();
        GameManager.Instance.SetupMenuMap();
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