using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_BallMoving : State
{
    public override void CheckState()
    {
        if(GameManager.Ball.GetComponent<Rigidbody>().IsSleeping())
        {
            //Não necessario agora
            //ResetBallRotation();
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
        GameManager.ActUpdate += OnState;
    }

    private void ResetBallRotation()
    {
        GameManager.Ball.transform.rotation = Quaternion.Euler(0,0,0);
    }
}
