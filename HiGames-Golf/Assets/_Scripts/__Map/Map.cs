using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Managers;
using static Struct;
using static Enums;
using UnityEditor;

//[CustomEditor(typeof(Map))]
//public class MyScriptEditor : Editor
//{
//    override public void OnInspectorGUI()
//    {
//        var myScript = target as Map;


//        if (myScript._GameType == GameType.Waypoint)
//        {
//            EditorGUI.indentLevel++;
//            EditorGUILayout.PrefixLabel("Gold Medal");
//            myScript.MedalGold = EditorGUILayout.IntField(myScript.MedalGold);
//            EditorGUILayout.PrefixLabel("Silver Medal");
//            myScript.MedalSilver = EditorGUILayout.IntField(myScript.MedalSilver);
//            EditorGUILayout.PrefixLabel("Number");
//            myScript.MedalBronze = EditorGUILayout.IntField(myScript.MedalBronze);
//            EditorGUI.indentLevel--;
//        }
//    }
//}


[Serializable]
public class Map : MonoBehaviour
{
    public string Name;
    public string Author;
    public int Chapter;
    public int MedalGold;
    public int MedalSilver;
    public int MedalBronze;
    public int GoldForCompletion;
    public GameType _GameType;
    public SkyboxType Skybox;
    public CameraDirection CameraDirection;
    public ColorPalette MapColors;
    public PersonalBest PB;
    public Transform[] WaypointsPosition;
    public GameObject Prefab;
    public GameObject StartingPosition;
    [HideInInspector] public Display Display;
    [HideInInspector] public GameObject SpawnedPrefab;
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
        //Set Map colors
        SetMapColors();
    }
    //-----
    private void HideStartingPosition()
    {
        StartingPosition.GetComponent<MeshRenderer>().enabled = false;
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
            GameManager.Instance.Player_Ball_Instantiate(p);
            GameManager.Instance.Player_Arrow_Instantiate(p);
            GameManager.Instance.Player_ForceBar_Instantiate(p);
            GameManager.Instance.Player_Hat_Instantiate(p);
            p.EndedMap = false;
            p.SelectedBall.StopAtPosition(true, StartingPosition.transform.position);
            p.SelectedBall.StartingPosition = StartingPosition.transform.position;
            p.SelectedBall.LastPosition = StartingPosition.transform.position;
        }
    }
    //---
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
   
    private void SetMapColors()
    {
        ColorPaletteManager.Instance.SetMapColors(SpawnedPrefab, (int)MapColors);
    }
}