using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_BallMoving : State
{
    private readonly float distToGroundOffSet = 0.001f;
    private float distToGround;

    //nova maneira de parar a bola - usar o velocity e somar xyz e ver a velocidade a partir.
    public override void CheckState()
    {
        if(IsGrounded() && Ball.RigBody.IsSleeping())
        {
            switch (GameManager.Instance.CurrentMap._GameType)
            {
                case Map.GameType.Menu:
                    Ball.GoStartingPosition();
                    //Save this position as last position
                    break;
                case Map.GameType.OneShot:
                    Ball.GoStartingPosition();
                    //UpdateCounter
                    break;
                case Map.GameType.Waypoint:
                    Ball.GoLastPosition();
                    break;
                case Map.GameType.FreeForm:
                    Ball.SaveLastPosition();
                    //save this as last position
                    break;
                default:
                    break;
            }
            LeaveState(ConnectedStates[0]);
        }
    }

    public override void LeaveState(State state)
    {
        GameManager.ActUpdate -= OnState;
        GameManager.CurrentState = state;
        GameManager.CurrentState.StartState();
    }

    public override void OnState()
    {
        CheckState();
    }

    public override void StartState()
    {
        distToGround = Ball.GetComponent<SphereCollider>().radius;
        GameManager.ActUpdate += OnState;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(Ball.transform.position, -Vector3.up, distToGround + distToGroundOffSet);
    }
}