using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [HideInInspector] public Rigidbody RigBody;

    // Start is called before the first frame update
    void Start()
    {
        RigBody = this.GetComponent<Rigidbody>();
    }

    private void StopAtPosition(Vector3 position)
    {
        transform.position = position;
        RigBody.velocity = Vector3.zero;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Waypoint" && !other.GetComponent<Waypoint>()._Reached)
        {
            Debug.Log("Entered Waypoint");
            Waypoint waypoint = other.GetComponent<Waypoint>();
            waypoint.SetReached();
            StopAtPosition(waypoint.Position);
            //Leave to State_LaunchBall
            GameManager.Instance.CurrentState.LeaveState(GameManager.Instance.CurrentState.ConnectedStates[0]);
        }
        if(other.tag == "Hole")
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
                    break;
                case TypeOfHole.HoleType.Game_TrickHole:
                    break;
                default:
                    break;
            }
        }
    }
}
