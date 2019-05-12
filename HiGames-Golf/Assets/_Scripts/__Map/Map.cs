﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Managers;

[Serializable]
public class Map : MonoBehaviour
{
    public enum GameType { Menu, OneShot, Waypoint, FreeForm};
    public enum SkyboxType { Garage, LivingRoom, Outdoor};
    public struct PersonalBest
    {
        public int Strikes;
        public float Time;
    }

    public string Name;
    public string Author;
    public int Chapter;
    public int MedalGold;
    public int MedalSilver;
    public int MedalBronze;
    public GameType _GameType;
    public SkyboxType Skybox;
    public PersonalBest PB;
    [HideInInspector] public Display Display;

    public CameraManager.CameraDirection CameraDirection;

    public GameObject Prefab;
    [HideInInspector] public GameObject SpawnedPrefab;

    public GameObject StartingPosition;
    public Transform[] WaypointsPosition;
    [HideInInspector] public GameObject[] Waypoints;


    public void StartMap()
    {
        //Instantiate Map
        SpawnedPrefab = Instantiate(Prefab);
        //Hide StartingPosition
        HideStartingPosition();
        //SetUp Ball
        SetupBalls();
        GameManager.Instance.ChoosePlayer(0);
        //Set Starting look Direction
        CameraManager.Instance.LookDirection(CameraDirection); 
        //Prepare Waypoints
        SetupWaypoints();
        HideWaypointPositions();
        //Prepare Strikes and Time
        ResetPlayerScore();
        //Setup Skybox
        SkinsManager.Instance.SetSkybox(Skybox);
        //Prepare UI
        UiManager.Instance.OpenInterface_InGameHud();
    }

    //-----
    private void HideStartingPosition()
    {
        StartingPosition.GetComponent<MeshRenderer>().enabled = false;
        StartingPosition.transform.Find("Direction").GetComponent<MeshRenderer>().enabled = false;
    }
    private void HideWaypointPositions()
    {
        for (int i = 0; i < WaypointsPosition.Length; i++)
        {
            WaypointsPosition[i].GetComponent<MeshRenderer>().enabled = false;
        }
    }
    //---
    private void SetupBalls()
    {
        foreach (Player p in GameManager.Instance.Players)
        {
            //TODO: PROBABLY REVIEW THIS
            GameManager.Instance.PlayerBall_Destroy(p);
            GameManager.Instance.PlayerBall_Instantiate(p);
            p.EndedMap = false;
            p.SelectedBall.Player = p;
            p.SelectedBall.StopAtPosition(true, StartingPosition.transform.position);
            p.SelectedBall.StartingPosition = StartingPosition.transform.position;
            p.SelectedBall.LastPosition = StartingPosition.transform.position;
        }
    }
    //---
    private void SetupWaypoints()
    {
        Waypoints = new GameObject[WaypointsPosition.Length];
        GameObject wp = MapManager.Instance.Waypoint;

        if (WaypointsPosition.Length != 0)
        {
            for (int i = 0; i < WaypointsPosition.Length; i++)
            {
                Waypoints[i] = Instantiate(wp);

                Waypoints[i].GetComponent<Waypoint>().ReachedPlayers = new List<int>(GameManager.Instance.Players.Count);

                Waypoints[i].GetComponent<Waypoint>().Position = WaypointsPosition[i].position;
                Waypoints[i].GetComponent<Waypoint>().Scale = WaypointsPosition[i].localScale;

                Waypoints[i].transform.position = Waypoints[i].GetComponent<Waypoint>().Position;
                Waypoints[i].transform.localScale = Waypoints[i].GetComponent<Waypoint>().Scale;
            }
        }
    }
    public void WaypointsReset()
    {
        if (WaypointsPosition.Length != 0)
        {
            for (int i = 0; i < WaypointsPosition.Length; i++)
            {
                Waypoints[i].GetComponent<Waypoint>().ReachedPlayers.Clear();
            }
        }
    }
    public void WaypointsDestroy()
    {
        foreach (GameObject g in Waypoints)
        {
            Destroy(g);
        }
    }
    //--
    public void CheckPersonalBest()
    {
        Player p = GameManager.Instance.CurrentPlayer;

        if (PB.Strikes == 0)
        {
            PB.Strikes = p.Strikes;
            PB.Time = p.Timer;
        }
        else if(p.Strikes == PB.Strikes)
        {
            if(p.Timer < PB.Time)
            {
                PB.Time = p.Timer;
            }
        }
        else if(p.Strikes < PB.Strikes)
        {
            PB.Strikes = p.Strikes;
            PB.Time = p.Timer;
        }
    }
    private void ResetPlayerScore()
    {
        foreach (Player p in GameManager.Instance.Players)
        {
            p.ResetScore();
        }
    }
}