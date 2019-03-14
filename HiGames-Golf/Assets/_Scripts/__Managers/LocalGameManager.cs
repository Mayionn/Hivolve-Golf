using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalGameManager : MonoBehaviour
{
    public Ball[] Players;
    public Ball CurrentPlayer;

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetNextPlayer()
    {
        GameManager.Instance.CurrentBall = CurrentPlayer;
    }
}
