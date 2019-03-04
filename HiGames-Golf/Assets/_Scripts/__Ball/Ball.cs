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
        transform.position = LastPosition;
        RigBody.velocity = Vector3.zero;
    }
    public void GoStartingPosition()
    {
        RigBody.velocity = Vector3.zero;
        transform.position = StartingPosition;
    }
    public void SaveLastPosition()
    {
        LastPosition = StartingPosition;
    }
    public void StopAtPosition(Vector3 position)
    {
        transform.position = position;
        RigBody.velocity = Vector3.zero;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Waypoint" && !other.GetComponent<Waypoint>()._Reached)
        {
            SetupWaypoint(other);
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

                            }
                            break;
                        default:
                            Debug.Log("Hole not defined!");
                            break;
                    }
                    break;
                case GameManager.GameState.Singleplayer:
                    {
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

    private static void SetupSingleplayer()
    {
        GameManager.Instance._GameState = GameManager.GameState.Singleplayer;
        GameManager.Instance.BuildSelectedMap();
    }
    private void SetupWaypoint(Collider other)
    {
        Waypoint waypoint = other.GetComponent<Waypoint>();
        waypoint.SetReached();
        StopAtPosition(waypoint.Position);
        LastPosition = waypoint.Position;
        //Leave to State_LaunchBall
        GameManager.Instance.CurrentState.LeaveState(GameManager.Instance.CurrentState.ConnectedStates[0]);
    }
}
