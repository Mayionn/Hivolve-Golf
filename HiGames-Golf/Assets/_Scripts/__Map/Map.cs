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
    [HideInInspector] public int CurrentStrikes;
    [HideInInspector] public float CurrentTime;
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
        DestroyBalls();
        SetupBall();
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
    private void SetupBall()
    {
        foreach (Player p in GameManager.Instance.Players)
        {
            GameManager.Instance.PlayerBall_Instantiate(p);
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
            Waypoints = new GameObject[WaypointsPosition.Length];
            GameObject wp = GameManager.Instance.MapManager.Waypoint;
            for (int i = 0; i < WaypointsPosition.Length; i++)
            {
                Waypoints[i] = Instantiate(wp);

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
    //---
    private void ResetScore()
    {
        CurrentStrikes = 0;
        CurrentTime = 0;
    }

}