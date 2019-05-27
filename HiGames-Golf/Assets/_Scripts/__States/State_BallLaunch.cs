using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Managers;
using static Enums;

public class State_BallLaunch : State
{
    private float rotSpeed = 0;
    private readonly float INCROTSPEED = 0.2f;
    private readonly float MAXROTSPEED = 4f;

    private float throwForce;
    private readonly float MAXTHROWFORCE = 20f;
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

    private readonly int ARROWSIZE = 5;

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
        bool canLaunch = false;
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
                        _striking = true;
                    }
                if (_striking)
                {
                    if (t.phase == TouchPhase.Moved)
                    {
                        if (touchPos1.y > t.position.y)
                        {
                            float distance2 = touchPos1.y - t.position.y;
                            if (distance2 > MAXSWIPEDISTANCE) distance2 = MAXSWIPEDISTANCE;
                            distance2 /= MAXSWIPEDISTANCE;

                            Setup_ArrowSize(distance2);
                            Setup_ArrowAim();
                        }
                        //If in the middle of the aim, the finger leaves the zone
                        if (!IsBottomSide(t) || IsRightSide(t) || IsLeftSide(t))
                        {
                            _striking = false;
                            Setup_ArrowSize(0);
                            break;
                        }
                    }
                    if (t.phase == TouchPhase.Ended)
                    {
                        if (t.position.y < touchPos1.y)
                        {
                            if (IsBottomSide(t) && !IsLeftSide(t) && !IsRightSide(t))
                            {
                                canLaunch = true;
                            }
                            touchPos2 = t.position;
                            _striking = false;
                            Setup_ArrowSize(0);
                        }
                    }
                }

                /*DEBUG*/
                //Delete on final version - Or use to make interactive display of strike power
              
            }
            if (canLaunch)
            {
                //Calculate finger distance
                float distance = touchPos1.y - touchPos2.y;
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
            if (Input.GetKey(KeyCode.Space) && GameManager._GameState == GameState.Resumed)
            {
                if (Ball.RigBody.isKinematic)
                {
                    Ball.RigBody.isKinematic = false;
                }

                Vector3 direction = GetThrowDirection();
                Ball.GetComponent<Rigidbody>().AddForce(direction * MAXTHROWFORCE, ForceMode.Impulse);
                _launched = true;

                //-Update Map and UI
                GameManager.Instance.CurrentPlayer.Strikes++;
                UiManager.Instance.UpdateMapInfoCurrentStrikes();
            }
        }
    }

    private void Setup_ArrowSize(float value)
    {
        Ball.Player.Arrow.transform.localScale = new Vector3(0.1f, 1, 0.4f + value);
    }
    private void Setup_ArrowAim()
    {
        var difference = Ball.transform.position - GetThrowDirection();
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

    private Vector3 GetThrowDirection()
    {
        Vector3 throwDirection = CameraManager.Instance.Camera.transform.forward;
        throwDirection.y = 0;
        return throwDirection;
    }
}
