using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Managers;
using System;

public class Player : MonoBehaviour
{
    public string Name;
    public int PlayerNum;
    public int Strikes;
    public float Timer;
    public int WaypointCounter;
    public GameObject[] Waypoints;
    public Ball SelectedBall;
    public Ball Example;
    //public Hand Hand;

    public Player(int num)
    {
        SetPlayerNumber(num);
        SetRandomGeneratedName();
        SetScore();
        SetChoosenBall();
    }

    private void SetPlayerNumber(int num)
    {
        PlayerNum = num;
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
    }
    private void SetChoosenBall()
    {
        Example = GameManager.Instance.SkinsManager.DefaultBall;
        SelectedBall = Instantiate(Example);
        SelectedBall.Init();
    }
}
