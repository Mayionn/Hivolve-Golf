using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_BallLaunch : State
{
    private readonly float ballRotationSpeed = 2f;
    private readonly float throwForce = 10f;
    private bool _launched;

    public override void CheckState()
    {
        if (_launched)
        {
            LeaveState(ConnectedStates[0]);
        }
        else Debug.Log("nao lançou");
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
        _launched = false;
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
            Vector3 direction = GetThrowDirection();
            Ball.GetComponent<Rigidbody>().AddForce(direction * throwForce,ForceMode.Impulse);
            _launched = true;
        }
    }
    private Vector3 GetThrowDirection()
    {
        Vector3 throwDirection = GameManager.Camera.transform.forward;
        throwDirection.y = 0;
        return throwDirection;
    }
}
