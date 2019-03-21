using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Managers;

public class Map : MonoBehaviour
{
    public GameObject Prefab;
    [HideInInspector] public GameObject SpawnedPrefab;
    public GameObject StartingPosition;

    public enum GameType { Menu, OneShot, Waypoint, FreeForm};
    public GameType _GameType;

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
        SetupBalls();
        GameManager.Instance.ChooseCurrentPlayer(0);
        //Prepare Waypoints
        SetupWaypoints();
        HideWaypointPositions();
        //Prepare Strikes and Time
        ResetScore();
        //Prepare UI
        UiManager.Instance.SetupInGameUI();
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
        Waypoints = new GameObject[WaypointsPosition.Length];
        GameObject wp = GameManager.Instance.MapManager.Waypoint;

        if (WaypointsPosition.Length != 0)
        {
            for (int i = 0; i < WaypointsPosition.Length; i++)
            {
                Waypoints[i] = Instantiate(wp);

                Waypoints[i].GetComponent<Waypoint>().ReachedPlayers = new List<int>(GameManager.Instance.Players.Count);

                Waypoints[i].GetComponent<Waypoint>().Position = WaypointsPosition[i].position;
                Waypoints[i].GetComponent<Waypoint>().Scale = WaypointsPosition[i].localScale;

                Waypoints[i].transform.position = WaypointsPosition[i].position;
                Waypoints[i].transform.localScale = Waypoints[i].GetComponent<Waypoint>().Scale;
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
    //--
    private void ResetScore()
    {
        foreach (Player p in GameManager.Instance.Players)
        {
            p.Strikes = 0;
            p.Timer = 0;
        }
    }
}