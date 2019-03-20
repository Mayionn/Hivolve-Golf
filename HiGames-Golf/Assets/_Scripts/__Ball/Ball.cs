using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Managers;

public class Ball : MonoBehaviour
{
    public Player Player;
    public Vector3 StartingPosition;
    public Vector3 LastPosition;
    [HideInInspector] public Rigidbody RigBody;

    public void Init()
    {
        RigBody = this.GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Waypoint")
        {
            List<int> rp = other.GetComponent<Waypoint>().ReachedPlayers;
            if(!rp.Contains(Player.PlayerNum))
            {
                rp.Add(Player.PlayerNum);
                SetupWaypoint(other.GetComponent<Waypoint>());
            }
        }
        else if(other.tag == "Hole")
        {
            switch (GameManager.Instance._GameState)
            {
                case GameManager.GameState.Menu:
                    switch (other.name)
                    {
                        case "Hole-Singleplayer":
                            {
                                SetupSingleplayer();
                            }
                            break;
                        case "Hole-Multiplayer":
                            {

                            }
                            break;
                        case "Hole-LocalGame":
                            {
                                SetupLocalMultiplayer();
                            }
                            break;
                        default:
                            Debug.Log("Hole not defined!");
                            break;
                    }
                    break;
                case GameManager.GameState.Singleplayer:
                    {
                        Debug.Log("Entrou");
                        //TODO: STOP TIME
                        //TODO: SAVE SCORE
                        //TODO: TRY AGAIN / NEXT LEVEL?
                    }
                    break;
                case GameManager.GameState.Multiplayer:
                    break;
                case GameManager.GameState.Localgame:
                    {
                        Debug.Log("entrou em local");
                    }
                    break;
                default:
                    break;
            }
        }
        else if(other.tag == "OOB")
        {
            GoLastPosition();
        }
    }

    public void GoLastPosition()
    {
        StopBall();
        transform.position = LastPosition;
    }
    public void GoStartingPosition()
    {
        StopBall();
        transform.position = StartingPosition;
    }
    public void SaveLastPosition()
    {
        LastPosition = StartingPosition;
    }
    public void StopAtPosition(Vector3 position)
    {
        StopBall();
        transform.position = position;
    }

    private void StopBall()
    {
        RigBody.Sleep();
        RigBody.velocity = Vector3.zero;
        RigBody.Sleep();
    }
    private static void SetupLocalMultiplayer()
    {
        GameManager.Instance._GameState = GameManager.GameState.Localgame;
        GameManager.Instance.BuildLocalMap();
    }
    private static void SetupSingleplayer()
    {
        GameManager.Instance._GameState = GameManager.GameState.Singleplayer;
        GameManager.Instance.BuildSelectedMap();
    }
    private void SetupWaypoint(Waypoint wp)
    {
         //Move Ball
         StopAtPosition(wp.Position);
         LastPosition = wp.Position;
         //Leave to State_LaunchBall
         GameManager.Instance.CurrentState.LeaveState(GameManager.Instance.CurrentState.ConnectedStates[0]);
         //Update Waypoint - UI
         UiManager.Instance.UpdateMapInfoWaypoints();
    }
}
