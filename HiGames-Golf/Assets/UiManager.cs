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
        int count = 0;
        for (int i = 0; i < map.Waypoints.Length; i++)
        {
            if (map.Waypoints[i]._Reached)
            {
                count++;
            }
        }
        UI_InGame.Waypoint.text = map.Waypoints.Length + " \\ " + count;
    }
    public void UpdateMapInfoCurrentStrikes()
    {
        UI_InGame.CurrentStrikes.text = "Strikes: " + map.CurrentStrikes;
    }

    public void TimerStart()
    {
        map.CurrentTime = 0;
        GameManager.Instance.ActUpdate += TimerCount;
    }
    public void TimerCount()
    {
        map.CurrentTime += Time.deltaTime;
        //UI_InGame.Time.text = timer.ToString();
        UI_InGame.CurrentTime.text = string.Format("{0:#0} : {1:00} : {2:00}", 
                                Mathf.Floor(map.CurrentTime / 60),
                                Mathf.Floor(map.CurrentTime) % 60,
                                Mathf.Floor((map.CurrentTime * 100) % 100));
    }
    public void TimerReset()
    {
        map.CurrentTime = 0;
    }
    public void TimerStop()
    {
        GameManager.Instance.ActUpdate -= TimerCount;
    }
}