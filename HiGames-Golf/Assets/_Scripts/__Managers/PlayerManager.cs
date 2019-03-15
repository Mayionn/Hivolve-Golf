using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Ball SelectedBall;
    //public Hand Hand;

    public Player()
    {
        SelectedBall = GameManager.Instance.SkinsManager.DefaultBall;
        SelectedBall.Init();
    }
    public float timer;
    public float strikes;
}
