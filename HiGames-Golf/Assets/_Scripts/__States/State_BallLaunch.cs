using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Managers;
using static Enums;

public class State_BallLaunch : State
{
    private Vector2 touchPos1;
    private Vector2 touchPos2;
    private Vector2 p;
    private float rotSpeed = 0;
    private float WIDTH;
    private float HEIGHT;
    private float TOPSIDE;
    private float BOTTOMSIDE;
    private float LEFTSIDE;
    private float RIGHTSIDE;
    private float MAXSWIPEDISTANCE;
    private readonly int ARROWSIZE = 5;
    private readonly float MAXROTSPEED = 4f;
    private readonly float INCROTSPEED = 0.2f;
    private readonly float MAXTHROWFORCE = 20f;
    private bool _launched;
    private bool _striking;
    private bool _canLaunch;

    //Father class Methods
    public override void CheckState()
    {
        if (_launched)
        {
            LeaveState(ConnectedStates[0]);
        }
    }
    public override void LeaveState(State state)
    {
        Ball.Player.Arrow.GetComponent<MeshRenderer>().enabled = false;
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

        Ball.Player.Arrow.GetComponent<MeshRenderer>().enabled = true;

        GameManager.ActUpdate += OnState;
    }
    
    //--- Methods
    private void CheckCameraMovement()
    {
        if (!_striking && GameManager._GameState == GameState.Resumed)
        {
            bool isTouch = false;

            foreach (Touch touch in Input.touches)
            {
                if (IsLeftSide(touch) && !IsBottomSide(touch))
                {
                    CameraManager.Instance.CameraOffSet
                        = Quaternion.AngleAxis(-rotSpeed, Vector3.up) * CameraManager.Instance.CameraOffSet;
                    isTouch = true;
                }
                else if (IsRightSide(touch) && !IsBottomSide(touch))
                {
                    CameraManager.Instance.CameraOffSet
                        = Quaternion.AngleAxis(+rotSpeed, Vector3.up) * CameraManager.Instance.CameraOffSet;
                    isTouch = true;
                }
            }

            if (Input.GetKey(KeyCode.A))
            {
                CameraManager.Instance.CameraOffSet = Quaternion.AngleAxis(+rotSpeed, Vector3.up) * CameraManager.Instance.CameraOffSet;
                isTouch = true;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                CameraManager.Instance.CameraOffSet = Quaternion.AngleAxis(-rotSpeed, Vector3.up) * CameraManager.Instance.CameraOffSet;
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

            Setup_ArrowAim();
        }
    }
    private void CheckBallThrow()
    {
        //Prevent Launch on Menus
        if (GameManager._GameState == GameState.Resumed)
        {
            //Get Finger Position
            foreach (Touch t in Input.touches)
            {
                if (t.phase == TouchPhase.Began)
                    if (IsBottomSide(t) && !IsRightSide(t) && !IsLeftSide(t))
                    {
                        touchPos1 = t.position;
                        _canLaunch = false;
                        _striking = true;
                    }
                if (_striking)
                {
                    if (t.phase == TouchPhase.Moved)
                    {
                        if (touchPos1.y > t.position.y)
                        {
                            Setup_LaunchEffect(GetTouchForce(touchPos1.y - t.position.y));
                            Setup_ArrowAim();
                        }
                        //If in the middle of the aim, the finger leaves the zone
                        if (!IsBottomSide(t) || IsRightSide(t) || IsLeftSide(t))
                        {
                            _striking = false;
                            Setup_LaunchEffect(0);
                            break;
                        }
                    }
                    else if (t.phase == TouchPhase.Ended)
                    {
                        if (t.position.y < touchPos1.y)
                        {
                            if (IsBottomSide(t) && !IsLeftSide(t) && !IsRightSide(t))
                            {
                                _canLaunch = true;
                            }
                            touchPos2 = t.position;
                            _striking = false;
                            Setup_LaunchEffect(0);
                        }
                    }
                }
            }
            if (_canLaunch)
            {
                //make strike force, depending on distance between fingers;
                float throwForce = GetTouchForce(touchPos1.x - touchPos2.y) * MAXTHROWFORCE; //The max it returns is 1

                if (Ball.RigBody.isKinematic)
                {
                    Ball.RigBody.isKinematic = false;
                }

                Vector3 direction = GetThrowDirection();
                Ball.GetComponent<Rigidbody>().AddForce(direction * throwForce, ForceMode.Impulse);

                _launched = true;
                _canLaunch = false;

                //-Update Map and UI
                GameManager.Instance.CurrentPlayer.Strikes++;
                UiManager.Instance.UpdateMapInfoCurrentStrikes();
            }

            #region Keyboard Launch
            if (Input.GetKey(KeyCode.Space) && GameManager._GameState == GameState.Resumed)
            {
                if (Ball.RigBody.isKinematic) Ball.RigBody.isKinematic = false;

                Vector3 direction = GetThrowDirection();
                Ball.GetComponent<Rigidbody>().AddForce(direction * MAXTHROWFORCE, ForceMode.Impulse);
                _launched = true;

                //-Update Map and UI
                GameManager.Instance.CurrentPlayer.Strikes++;
                UiManager.Instance.UpdateMapInfoCurrentStrikes();
            }
            #endregion
        }
    }
    private void Setup_LaunchEffect(float value)
    {
        Ball.Player.Arrow.transform.localScale = new Vector3(0.1f, 1, 0.4f + value);
        CameraManager.Instance.LaunchEffect(value);
    }
    private void Setup_ArrowAim()
    {
        Ball.Player.Arrow.transform.position = Ball.transform.position + (GetThrowDirection() * Ball.Player.Arrow.transform.localScale.z * ARROWSIZE);
        Ball.Player.Arrow.transform.forward = -GetThrowDirection();
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
    private float GetTouchForce(float value)
    {
        //returns values between 0 and 1
        float force = value;

        if (force > MAXSWIPEDISTANCE) force = MAXSWIPEDISTANCE;

        return force /= MAXSWIPEDISTANCE;
    }
    private Vector3 GetThrowDirection()
    {
        Vector3 throwDirection = CameraManager.Instance.Camera.transform.forward;
        throwDirection.y = 0;
        return throwDirection;
    }
}
