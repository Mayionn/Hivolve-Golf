using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Managers;

public class State_BallLaunch : State
{
    private float rotSpeed = 0;
    private readonly float INCROTSPEED = 0.2f;
    private readonly float MAXROTSPEED = 4f;

    private float throwForce;
    private readonly float MAXTHROWFORCE = 15f;
    private Vector2 touchPos1, touchPos2, p;

    private float WIDTH;
    private float HEIGHT;
    private float TOPSIDE;
    private float BOTTOMSIDE;
    private float LEFTSIDE;
    private float RIGHTSIDE;
    private float MAXSWIPEDISTANCE;

    private bool _launched;
    private bool _striking;

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
        _launched = false;
        _striking = false;

        WIDTH = GameManager.Canvas.pixelRect.width;
        HEIGHT = GameManager.Canvas.pixelRect.height;
        TOPSIDE = (HEIGHT / 3) * 2;
        BOTTOMSIDE = HEIGHT / 3;
        LEFTSIDE = WIDTH / 3;
        RIGHTSIDE = (WIDTH / 3) * 2;
        MAXSWIPEDISTANCE = BOTTOMSIDE / 3;

        GameManager.ActUpdate += OnState;
    }
    
    //--- Methods
    private void CheckCameraMovement()
    {
        if (!_striking)
        {
            bool isTouch = false;

            foreach(Touch touch in Input.touches)
            {
                if(IsLeftSide(touch))
                {
                    GameManager.Instance.CameraManager.CameraOffSet 
                        = Quaternion.AngleAxis(-rotSpeed, Vector3.up) * GameManager.Instance.CameraManager.CameraOffSet;
                    isTouch = true;
                }
                else if(IsRightSide(touch))
                {
                    GameManager.Instance.CameraManager.CameraOffSet 
                        = Quaternion.AngleAxis(+rotSpeed, Vector3.up) * GameManager.Instance.CameraManager.CameraOffSet;
                    isTouch = true;
                }
            }

            if (Input.GetKey(KeyCode.A))
            {
                GameManager.Instance.CameraManager.CameraOffSet = Quaternion.AngleAxis(+rotSpeed, Vector3.up) * GameManager.Instance.CameraManager.CameraOffSet;
                isTouch = true;
            }
            else if(Input.GetKey(KeyCode.D))
            {
                GameManager.Instance.CameraManager.CameraOffSet = Quaternion.AngleAxis(-rotSpeed, Vector3.up) * GameManager.Instance.CameraManager.CameraOffSet;
                isTouch = true;
            }

            if (isTouch)
            {
                if (rotSpeed < MAXROTSPEED)
                {
                    rotSpeed += INCROTSPEED;
                }
            }
            else rotSpeed = 0;
        }
    }

    private bool IsLeftSide(Touch t)
    {
        if (t.position.x < LEFTSIDE && t.position.x > 0) return true;
        else return false;
    }
    private bool IsRightSide(Touch t)
    {
        if (t.position.x > RIGHTSIDE && t.position.x < WIDTH) return true;
        else return false;
    }
    private bool IsBottomSide(Touch t)
    {
        if (t.position.y < BOTTOMSIDE && t.position.y > 0) return true;
        else return false;
    }

    private void CheckBallThrow()
    {
        bool canLaunch = false;

        //Get Finger Position
        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began)
                if (IsBottomSide(t) && !IsRightSide(t) && !IsLeftSide(t))
                { 
                    touchPos1 = t.position;
                    _striking = true;
                }
            //for debug only
            if(t.phase == TouchPhase.Moved)
            {
                p = t.position;
            }

            if (t.phase == TouchPhase.Ended)
            {
                if (t.position.y < touchPos1.y)
                {
                    touchPos2 = t.position;
                    _striking = false;
                    canLaunch = true;
                }
            }

            float throwForce2;
            float distance2 = Vector2.Distance(touchPos1, p);
            if (distance2 > MAXSWIPEDISTANCE) distance2 = MAXSWIPEDISTANCE;
            distance2 /= MAXSWIPEDISTANCE;
            //make strike force, depending on distance between fingers;
            throwForce2 = distance2 * MAXTHROWFORCE;
            GameManager.Instance.DebugTxt.text = throwForce2.ToString();

        }
        if (canLaunch)
        {
            //Calculate finger distance
            float distance = Vector2.Distance(touchPos1, touchPos2);
                if (distance > MAXSWIPEDISTANCE) distance = MAXSWIPEDISTANCE;
            distance /= MAXSWIPEDISTANCE;
            //make strike force, depending on distance between fingers;
            throwForce = distance * MAXTHROWFORCE;

            //TODO - CAUCLUATE DISTANCE BETWEN POS = THROWN FORCE
            if (Ball.RigBody.isKinematic)
            {
                Ball.RigBody.isKinematic = false;
            }

            Vector3 direction = GetThrowDirection();
            Ball.GetComponent<Rigidbody>().AddForce(direction * throwForce, ForceMode.Impulse);
            _launched = true;

            //-Update Map and UI
            GameManager.Instance.CurrentPlayer.Strikes++;
            UiManager.Instance.UpdateMapInfoCurrentStrikes();
        }

        //COMPUTER
        if(Input.GetKey(KeyCode.Space) && GameManager._GameState == GameManager.GameState.Resumed)
        {
            if (Ball.RigBody.isKinematic)
            {
                Ball.RigBody.isKinematic = false;
            }

            Vector3 direction = GetThrowDirection();
            Ball.GetComponent<Rigidbody>().AddForce(direction * 10,ForceMode.Impulse);
            _launched = true;

            //-Update Map and UI
            GameManager.Instance.CurrentPlayer.Strikes++;
            UiManager.Instance.UpdateMapInfoCurrentStrikes();
        }
    }
    private Vector3 GetThrowDirection()
    {
        Vector3 throwDirection = GameManager.Instance.CameraManager.Camera.transform.forward;
        throwDirection.y = 0;
        return throwDirection;
    }
}
