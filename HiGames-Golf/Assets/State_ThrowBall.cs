using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_ThrowBall : State
{
    private float ballRotationSpeed = 2f;
    private float throwForce = 100f;
    private Vector3 throwDirection;
    private bool ballThrown = false;

    public override void CheckState()
    {
        if(ballThrown)
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
        CheckCameraMovement();

        CheckBallThrow();

        CheckState();
    }

    public override void StartState()
    {
        GameManager.ActUpdate += OnState;
    }
    
    private void CheckCameraMovement()
    {
        if(Input.GetKey(KeyCode.A))
        {
            GameManager.CameraOffSet = Quaternion.AngleAxis(+ballRotationSpeed, Vector3.up) * GameManager.CameraOffSet;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            GameManager.CameraOffSet = Quaternion.AngleAxis(-ballRotationSpeed, Vector3.up) * GameManager.CameraOffSet;
        }
    }

    private void CheckBallThrow()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            SetThrowDirection();
            GameManager.Ball.GetComponent<Rigidbody>().AddForce(throwDirection * throwForce);
            ballThrown = true;
        }
    }
    private void SetThrowDirection()
    {
        throwDirection = GameManager.Camera.transform.forward;
        throwDirection.y = 0;
    }
}
