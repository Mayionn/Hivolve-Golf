using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Vector3 StartingPosition;
    public Vector3 LastPosition;
    [HideInInspector] public Rigidbody RigBody;

    // Start is called before the first frame update
    void Awake()
    {
        RigBody = this.GetComponent<Rigidbody>();
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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Waypoint" && !other.GetComponent<Waypoint>()._Reached)
        {
            SetupWaypoint(other.GetComponent<Waypoint>());
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
                        case "Hole-Localgame":
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
                    break;
                default:
                    break;
            }
        }
        else if(other.tag == "OOB")
        {
            GoLastPosition();
            //UPDATE: SCORE
        }
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
        wp.SetReached();
        //Move Ball
        StopAtPosition(wp.Position);
        LastPosition = wp.Position;
        //Leave to State_LaunchBall
        GameManager.Instance.CurrentState.LeaveState(GameManager.Instance.CurrentState.ConnectedStates[0]);
        //Update Waypoint - UI
        GameManager.Instance.UiManager.UpdateMapInfoWaypoints();
    }
}
