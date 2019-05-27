using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Managers;
using static Struct;
using static Enums;

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
                case GameType.Menu:
                    Ball.GoStartingPosition(true);
                    break;
                case GameType.OneShot:
                    Ball.GoStartingPosition(true);
                    break;
                case GameType.Waypoint:
                    Ball.GoLastPosition(true);
                    break;
                case GameType.FreeForm:
                    Ball.SaveLastPosition();
                    break;
                default:
                    break;
            }
            LeaveState(ConnectedStates[0]);
        }
    }

    public override void LeaveState(State state)
    {
        if(GameManager.Instance._GameMode == GameMode.Localgame)
        {
            GameManager.Instance.NextPlayer();
        }
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
        return Physics.Raycast(Ball.transform.position, -Vector3.up, (distToGround + distToGroundOffSet) * Ball.transform.localScale.x);
    }
}