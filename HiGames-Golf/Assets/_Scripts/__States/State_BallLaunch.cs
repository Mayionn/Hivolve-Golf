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
        Debug.Log("On State_BallLaunch");
        _launched = false;
        GameManager.ActUpdate += OnState;
    }
    
    private void CheckCameraMovement()
    {
        if(Input.GetKey(KeyCode.A))
        {
            GameManager.Instance.CameraManager.CameraOffSet = Quaternion.AngleAxis(+ballRotationSpeed, Vector3.up) * GameManager.Instance.CameraManager.CameraOffSet;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            GameManager.Instance.CameraManager.CameraOffSet = Quaternion.AngleAxis(-ballRotationSpeed, Vector3.up) * GameManager.Instance.CameraManager.CameraOffSet;
        }
    }
    private void CheckBallThrow()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            if (Ball.RigBody.isKinematic)
            {
                Ball.RigBody.isKinematic = false;
            }

            Vector3 direction = GetThrowDirection();
            Ball.GetComponent<Rigidbody>().AddForce(direction * throwForce,ForceMode.Impulse);
            _launched = true;

            //-Update Map and UI
            GameManager.Instance.CurrentMap.CurrentStrikes++;
            GameManager.Instance.UiManager.UpdateMapInfoCurrentStrikes();
        }
    }
    private Vector3 GetThrowDirection()
    {
        Vector3 throwDirection = GameManager.Instance.CameraManager.Camera.transform.forward;
        throwDirection.y = 0;
        return throwDirection;
    }
}
