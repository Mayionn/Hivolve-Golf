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
    public bool Phasing;
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
                                UiManager.Instance.OpenInterface_MapSelector();
                            }
                            break;
                        case "Hole-Multiplayer":
                            {

                            }
                            break;
                        case "Hole-LocalGame":
                            {
                                //Open LocalMultiplayer Interface
                                Player.EndedMap = false;
                                UiManager.Instance.OpenInterface_LocalMultiplayer();
                            }
                            break;
                        default:
                            Debug.Log("Hole not defined!");
                            break;
                    }
                    break;
                case GameManager.GameState.Singleplayer:
                    {
                        Player.TruncateTimer();
                        UiManager.Instance.OpenInterface_CompletedMap();
                    }
                    break;
                case GameManager.GameState.Multiplayer:
                    break;
                case GameManager.GameState.Localgame:
                    {
                        Player.EndedMap = true;
                        //this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                        UiManager.Instance.LGS_SaveScore(Player);
                        GameManager.Instance.NextPlayer();

                        //TODO: DONT FORGET TO TURN GAMEOBJECT TO ON 
                        //      AND DELETE OTHER PLAYERS AT THE END

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
    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            Phasing = false;
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
