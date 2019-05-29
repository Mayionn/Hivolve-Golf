using Assets.Generics;
using Assets.Managers;
using Assets.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

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
        public Sprite Gold;
        public Sprite Diamonds;
        public Sprite LockedSkin;
        public Sprite StarIncomplete;
        public Sprite StarComplete;
    }
    [Serializable] public struct UIBackgroundImages
    {
        public Sprite DefaultBackground;
    }
    
    //Struct Variables
    public UIImages UI_Images;
    public UIBackgroundImages UI_BackgroundImages;

    private UI_LocalMultiplayer UI_LocalMultiplayer;
    private UI_MapSelector UI_MapSelector;
    private UI_InGameHud UI_InGameHud;
    private UI_ReadyCheck UI_ReadyCheck;
    private UI_LocalScoreboard UI_LocalScoreboard;
    private UI_CompletedMap UI_CompletedMap;
    private UI_SkinMenu UI_SkinMenu;
    
    public void Init()
    {
        UI_LocalMultiplayer = GetComponent<UI_LocalMultiplayer>();
        UI_MapSelector = GetComponent<UI_MapSelector>();
        UI_InGameHud = GetComponent<UI_InGameHud>();
        UI_ReadyCheck = GetComponent<UI_ReadyCheck>();
        UI_LocalScoreboard = GetComponent<UI_LocalScoreboard>();
        UI_CompletedMap = GetComponent<UI_CompletedMap>();
        UI_SkinMenu = GetComponent<UI_SkinMenu>();
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
        UI_CompletedMap.Init();
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
    public void OpenInterface_LocalResults()
    {
        GameManager.Instance.TimeScaleStop();
        UI_LocalScoreboard.Init_Results();
    }
    public void OpenInterface_SkinsMenu()
    {
        GameManager.Instance.TimeScaleStop();
        CloseInterface_InGameHud();
        UI_SkinMenu.Init();
    }

    public void CloseInterface_InGameHud()
    {
        UI_InGameHud.Terminate();
    }
    public void CloseInterface_InGameReadyCheck()
    {
        UI_ReadyCheck.Terminate();
        GameManager.Instance.TimeScaleResume();
    }
    public void CloseInterface_MapSelector()
    {
        GameManager.Instance.TimeScaleResume();
        UI_MapSelector.Terminate();
    }
    public void CloseInterface_CompletedMap()
    {
        GameManager.Instance.TimeScaleResume();
        UI_CompletedMap.Terminate();
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
    public void CloseInterface_LocalResults()
    {
        GameManager.Instance.TimeScaleResume();
        UI_LocalScoreboard.Terminate_Results();
    }
    public void CloseInterface_SkinsMenu()
    {
        GameManager.Instance.TimeScaleResume();
        OpenInterface_InGameHud();
        UI_SkinMenu.Terminate();
    }

    //Update Info Methods
    public void Update_ScoreBoard_Rows()
    {
        UI_LocalScoreboard.SetupRows();
    }
    public void Update_ScoreBoard_SaveScore(Player player)
    {
        UI_LocalScoreboard.Setup_Score(player);
    }
    public void Update_MapSelector_UnlockNextLevel(int level)
    {
        UI_MapSelector.UnlockNextLevel(level);
    }
    public void UpdateCurrentPlayerName()
    {
        UI_InGameHud.SetCurrentPlayerInfo();
    }
    public void UpdateMapInfoWaypoints()
    {
        Player p = GameManager.Instance.CurrentPlayer;
        Map m = GameManager.Instance.CurrentMap;
        if (m._GameType == GameType.Waypoint)
        {
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
        else
        {
            UI_InGameHud.UI_InGame.Waypoint.text = " ";
        }
    }
    public void UpdateMapInfoCurrentStrikes()
    {
        if(GameManager.Instance._GameMode != GameMode.Menu)
        {
            if(GameManager.Instance.CurrentMap._GameType != GameType.OneShot)
            {
                UI_InGameHud.UI_InGame.CurrentStrikes.text = "Strikes: " + GameManager.Instance.CurrentPlayer.Strikes;
            }
        }
    }
}