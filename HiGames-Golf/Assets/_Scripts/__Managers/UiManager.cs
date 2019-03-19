using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public UI_InGame UI_InGame;
    private Map map;

    public void SetupInGameUI(Map map)
    {
        this.map = map;
        switch (GameManager.Instance._GameState)
        {
            case GameManager.GameState.Menu:
                TimerStart();
                    SetMapInfo();
                    SetMapInfoMedals();
                    SetMapInfoWaypoints();
                    SetMapInfoCurrentStrikes();
                break;
            case GameManager.GameState.Singleplayer:
                TimerStart();
                    SetMapInfo();
                    SetMapInfoMedals();
                    SetMapInfoWaypoints();
                    SetMapInfoCurrentStrikes();
                break;
            case GameManager.GameState.Multiplayer:
                break;
            case GameManager.GameState.Localgame:
                break;
            default:
                break;
        }
    }

    private void SetMapInfo()
    {
        UI_InGame.MapInfo.text = map.Prefab.name + "\n" + map.Author;
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

    public void UpdateMapInfoWaypoints()
    {
        Player p = GameManager.Instance.CurrentPlayer;
        for (int i = 0; i < p.Waypoints.Length; i++)
        {
            if(p.Waypoints[i].GetComponent<Waypoint>()._Reached)
            {
                p.WaypointCounter++;
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
        GameManager.Instance.CurrentPlayer.Timer = 0;
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