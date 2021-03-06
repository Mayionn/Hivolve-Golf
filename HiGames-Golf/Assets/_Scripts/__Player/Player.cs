﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Managers;
using System;

public class Player
{
    public string Name;
    public int PlayerNum;
    public int Strikes;
    public float Timer;
    public int WaypointCounter;
    public int LocalgamePoints;
    public bool EndedMap = false;
    public Ball SelectedBall;
    public Ball Example; //Used to prevent deleting the original gameobject
    public GameObject Hat;
    public GameObject Hat_Prefab;
    public GameObject Arrow;
    public GameObject Arrow_Prefab;
    public GameObject ForceBar;
    public GameObject ForceBar_Prefab;
    //Skins
    public Skin_Hat Skin_Hat;
    public Skin_Ball Skin_Ball;
    public Skin_Arrow Skin_Arrow;
    public Skin_ForceBar Skin_ForceBar;

    public Player()
    {
        SetPlayerNumber();
        SetRandomGeneratedName();
        SetScore();
        Setup_ChoosenBall();
    }

    private void SetPlayerNumber()
    {
        PlayerNum = GameManager.Instance.Players.Count;
    }
    private void SetRandomGeneratedName()
    {
        int n =  UnityEngine.Random.Range(0, 1000);
        Name = n.ToString(); 
    }
    private void SetScore()
    {
        Strikes = 0;
        Timer = 0;
        WaypointCounter = 0;
        LocalgamePoints = 0;
    }
    private void Setup_ChoosenBall()
    {
        Example = SkinsManager.Instance.DefaultBall;
        Skin_Ball = SkinsManager.Instance.List_Skins_Balls[0]; //default
        Skin_Hat = SkinsManager.Instance.List_Skins_Hats[0];   //default
        Skin_Arrow = SkinsManager.Instance.List_Skins_Arrows[0];

        Arrow_Prefab = SkinsManager.Instance.ArrowPrefab;
        ForceBar_Prefab = SkinsManager.Instance.ForceBar_Prefab;
    }
    public void Setup_HatSkin()
    {
        Hat_Prefab = Skin_Hat.Hat;
    }

    public void TruncateTimer()
    {
        float truncated = (float)(Math.Truncate((double)Timer * 100.0) / 100.0);

        Timer = (float)(Math.Round((double)Timer, 2));
    }
    public void ResetScore()
    {
        Strikes = 0;
        Timer = 0;
        WaypointCounter = 0;
    }
    public void ResetLocalMultiplayerScore()
    {
        LocalgamePoints = 0;
    }
}
