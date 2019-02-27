using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_BallMoving : State
{
    private readonly int arraySize = 2;
    private readonly float stopDistance = 0.0001f;
    private float distToGround;
    private readonly float distToGroundOffSet = 0.001f;
    private Vector3[] positionArray;

    public override void CheckState()
    {
        //Debug.Log(Vector3.Distance(positionArray[0], positionArray[1]));
        if(positionArray[0] != null)
        {
            if(IsGrounded() && Vector3.Distance(positionArray[0], positionArray[1]) < stopDistance)
            {
                Debug.Log("Woop");
                //GameManager.Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        if(IsGrounded() && Ball.RigBody.IsSleeping())
        {
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
        UpdateArray();

        CheckState();
    }

    public override void StartState()
    {
        positionArray = new Vector3[arraySize];
        distToGround = Ball.GetComponent<SphereCollider>().radius;
        GameManager.ActUpdate += OnState;
    }

    private void UpdateArray() {
        positionArray[0] = positionArray[1];
        positionArray[1] = Ball.transform.position;
    }
    private bool IsGrounded()
    {
        return Physics.Raycast(Ball.transform.position, -Vector3.up, distToGround + distToGroundOffSet);
    }

   
}