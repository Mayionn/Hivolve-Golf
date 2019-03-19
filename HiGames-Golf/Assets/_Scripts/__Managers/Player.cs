using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string Name;
    public int Strikes;
    public float Timer;
    public int WaypointCounter;
    public GameObject[] Waypoints;
    public Ball SelectedBall;
    public Ball Example;
    //public Hand Hand;

    public Player()
    {
        Name = "Lucas";
        Strikes = 0;
        Timer = 0;
        WaypointCounter = 0;
        Example = GameManager.Instance.SkinsManager.DefaultBall;
        SelectedBall = Instantiate(Example);
        SelectedBall.Init();
    }
}
