using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject Prefab;
    [HideInInspector] public GameObject SpawnedPrefab;
    public GameObject StartingPosition;

    public enum GameType { Menu, OneShot, Waypoint, FreeForm};
    public GameType _GameType;

    private UiManager uiManager;
    public int MedalGold;
    public int MedalSilver;
    public int MedalBronze;
    public Transform[] WaypointsPosition;
    [HideInInspector] public GameObject[] Waypoints;
    public string Author;

    public void StartMap()
    {
        //Instantiate Map
        SpawnedPrefab = Instantiate(Prefab);
        //Hide StartingPosition
        HideStartingPosition();
        //SetUp Ball
        //DestroyBalls();
        SetupBalls();
        GameManager.Instance.ChooseCurrentPlayer(0);
        //Prepare Waypoints
        SetupWaypoints();
        HideWaypointPositions();
        //Prepare Strikes and Time
        ResetScore();
        //Prepare UI
        uiManager = GameManager.Instance.UiManager;
        uiManager.SetupInGameUI(this);
    }

    //-----
    private void HideStartingPosition()
    {
        StartingPosition.GetComponent<MeshRenderer>().enabled = false;
        StartingPosition.transform.Find("Direction").GetComponent<MeshRenderer>().enabled = false;
    }
    //---
    private void DestroyBalls()
    {
        foreach (Player p in GameManager.Instance.Players)
        {
            GameManager.Instance.PlayerBall_Destroy(p);
        }
    }
    private void SetupBalls()
    {
        //GameManager.Instance.PlayerBall_Update();
        foreach (Player p in GameManager.Instance.Players)
        {
            GameManager.Instance.PlayerBall_Destroy(p);
            GameManager.Instance.PlayerBall_Instantiate(p);
            p.SelectedBall.Player = p;
            p.SelectedBall.StopAtPosition(StartingPosition.transform.position);
            p.SelectedBall.StartingPosition = StartingPosition.transform.position;
            p.SelectedBall.LastPosition = StartingPosition.transform.position;
        }
    }
    //---
    private void SetupWaypoints()
    {
        if (WaypointsPosition.Length != 0)
        {
            for (int i = 0; i < WaypointsPosition.Length; i++)
            {
                foreach (Player p in GameManager.Instance.Players)
                {
                    p.Waypoints = new GameObject[WaypointsPosition.Length];
                    GameObject wp = GameManager.Instance.MapManager.Waypoint;
                    p.Waypoints[i] = Instantiate(wp);

                    p.Waypoints[i].GetComponent<Waypoint>().Position = WaypointsPosition[i].position;
                    p.Waypoints[i].GetComponent<Waypoint>().Scale = WaypointsPosition[i].localScale;

                    p.Waypoints[i].transform.position = WaypointsPosition[i].position;
                    p.Waypoints[i].transform.localScale = p.Waypoints[i].GetComponent<Waypoint>().Scale;
                }
            }
        }
    }
    private void HideWaypointPositions()
    {
        for (int i = 0; i < WaypointsPosition.Length; i++)
        {
            WaypointsPosition[i].GetComponent<MeshRenderer>().enabled = false;
        }
    }
    //----
    private void ResetScore()
    {
        GameManager.Instance.CurrentPlayer.Strikes = 0;
        GameManager.Instance.CurrentPlayer.Timer = 0;
    }
}