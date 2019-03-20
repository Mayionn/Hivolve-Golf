using Assets.Generics;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Managers;
using UnityEngine;

public class UiManager : Singleton<UiManager>
{
    public UI_InGame UI_InGame;
    public UI_LocalMultiplayer UI_LocalMultiplayer;
    private Map map;

    public void Init()
    {
        UI_LocalMultiplayer.Init();
    }

    public void SetupInGameUI()
    {
        map = GameManager.Instance.CurrentMap;
        switch (GameManager.Instance._GameState)
        {
            case GameManager.GameState.Menu:
                    TimerStart();
                    SetCurrentPlayerInfo();
                    SetMapInfo();
                    SetMapInfoMedals();
                    SetMapInfoWaypoints();
                    SetMapInfoCurrentStrikes();
                break;
            case GameManager.GameState.Singleplayer:
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
        if(map.Waypoints.Length > 0)
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
                if(rp[o] == p.PlayerNum)
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

    public void TimerStart()
    {
        GameManager.Instance.ActUpdate += TimerCount;
    }
    public void TimerCount()
    {
        GameManager.Instance.CurrentPlayer.Timer += Time.deltaTime;
        float t = GameManager.Instance.CurrentPlayer.Timer;
        //UI_InGame.Time.text = timer.ToString();
        UI_InGame.CurrentTime.text = string.Format("{0:#0} : {1:00} : {2:00}", 
                                Mathf.Floor(t / 60),
                                Mathf.Floor(t) % 60,
                                Mathf.Floor((t * 100) % 100));
    }
    public void TimerReset()
    {
        GameManager.Instance.CurrentPlayer.Timer = 0;
    }
    public void TimerStop()
    {
        GameManager.Instance.ActUpdate -= TimerCount;
    }
}