using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Map CurrentMap;
    public GameObject Ball;
    public GameObject Camera;
    public Vector3 CameraOffSet;
    public Action ActUpdate;
    public GameObject States;
    public State CurrentState;

    void Start()
    {
        //this.transform.position = CurrentMap.StartingPosition;
        //this.transform.position = Vector3.zero;
        CameraOffSet = new Vector3(Ball.transform.position.x + CameraOffSet.x, CameraOffSet.y, Ball.transform.position.z + CameraOffSet.z);

        BuildStateMachine();
    }

    void Update() 
    {
        ActUpdate?.Invoke();
	}
    void LateUpdate()
    {
        Camera.transform.position = Ball.transform.position + CameraOffSet;
        Camera.transform.LookAt(Ball.transform.position);
    }

    private void BuildStateMachine()
    {
        State State_ThrowBall = States.GetComponent<State_ThrowBall>();
        State State_BallMoving = States.GetComponent<State_BallMoving>();

        State_ThrowBall.GameManager = this;
        State_BallMoving.GameManager = this;
        
        State_ThrowBall.ConnectedStates = new List<State>() {State_BallMoving};
        State_BallMoving.ConnectedStates = new List<State>() {State_ThrowBall};

        CurrentState = States.GetComponent<State_BallMoving>();
        CurrentState.StartState();
    }
}
