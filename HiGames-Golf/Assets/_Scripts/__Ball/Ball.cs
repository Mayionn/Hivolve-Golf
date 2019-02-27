using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
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
    public void StopAtPosition(Vector3 position)
    {
        transform.position = position;
        RigBody.velocity = Vector3.zero;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Waypoint" && !other.GetComponent<Waypoint>()._Reached)
        {
            Waypoint waypoint = other.GetComponent<Waypoint>();
            waypoint.SetReached();
            StopAtPosition(waypoint.Position);
            LastPosition = waypoint.Position;
            //Leave to State_LaunchBall
            GameManager.Instance.CurrentState.LeaveState(GameManager.Instance.CurrentState.ConnectedStates[0]);
        }
        else if(other.tag == "Hole")
        {
            switch (other.GetComponent<TypeOfHole>().typeOfHole)
            {
                case TypeOfHole.HoleType.Menu_Singleplayer:
                    GameManager.Instance._GameState = GameManager.GameState.SinglePlayer;
                    GameManager.Instance.BuildSelectedMap();
                    break;
                case TypeOfHole.HoleType.Menu_Multiplayer:
                    break;
                case TypeOfHole.HoleType.Menu_Local:
                    break;
                case TypeOfHole.HoleType.Game_FinalHole:
                    switch (GameManager.Instance._GameState)
                    {
                        case GameManager.GameState.SinglePlayer:
                            //TODO: STOP TIME
                            //TODO: SAVE SCORE
                            //TODO: TRY AGAIN / NEXT LEVEL?
                            break;
                        default:
                            break;
                    }
                    break;
                case TypeOfHole.HoleType.Game_TrickHole:
                    break;
                default:
                    break;
            }
        }
        else if(other.tag == "OOB")
        {
            //TODO: GO LAST POSITION STOP
            //UPDATE: SCORE
        }
    }
}
