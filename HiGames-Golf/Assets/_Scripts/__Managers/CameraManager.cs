using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Managers;
using Assets.Generics;
using System;
using static Enums;

public class CameraManager : Singleton<CameraManager>
{

    public Camera Camera;
    public Camera SideCamera;
    public Vector3 CameraOffSet;
    public Vector3 CameraHeigthOffSet;
    public float FieldOfView = 70f;
    public float LaunchEffectMultiplier = 30;
    public float OffSetLaunchRange;
    public float OffSetLaunchRecovery;
    public float HeigthOffSetLaunchRange;
    public float HeigthOffSetLaunchRecovery;

    private Vector3 _westOffSet; //Default
    private Vector3 _eastOffSet;
    private Vector3 _northOffSet;
    private Vector3 _southOffSet;
    private float ofY;
    private float ohfY;
    private float defaultFieldOfView;

    void LateUpdate()
    {
        float velocity = GameManager.Instance.CurrentPlayer.SelectedBall.RigBody.velocity.magnitude + (FieldOfView - defaultFieldOfView);

        Vector3 newOffSet = new Vector3(CameraOffSet.x, ofY + (velocity / OffSetLaunchRange), CameraOffSet.z);
        Vector3 newCameraHeightOffSet = new Vector3(CameraHeigthOffSet.x, ohfY - (velocity / HeigthOffSetLaunchRange), CameraHeigthOffSet.z);

        CameraHeigthOffSet = Vector3.Lerp(CameraHeigthOffSet, newCameraHeightOffSet, Time.deltaTime * OffSetLaunchRecovery);
        CameraOffSet = Vector3.Lerp(CameraOffSet, newOffSet, Time.deltaTime * HeigthOffSetLaunchRecovery);

        Camera.transform.position = GameManager.Instance.CurrentPlayer.SelectedBall.transform.position + CameraOffSet;
        Camera.transform.LookAt(GameManager.Instance.CurrentPlayer.SelectedBall.transform.position + CameraHeigthOffSet);
        SideCamera.transform.position = GameManager.Instance.CurrentPlayer.SelectedBall.transform.position + CameraOffSet;
        SideCamera.transform.LookAt(GameManager.Instance.CurrentPlayer.SelectedBall.transform.position + CameraHeigthOffSet);

        Camera.fieldOfView = FieldOfView;
        SideCamera.fieldOfView = FieldOfView;
    }

    public void Init()
    {
        this.CameraOffSet = new Vector3(GameManager.Instance.CurrentPlayer.SelectedBall.transform.position.x + CameraOffSet.x,
                                    CameraOffSet.y,
                                    GameManager.Instance.CurrentPlayer.SelectedBall.transform.position.z + CameraOffSet.z);

        this.ofY = CameraOffSet.y;
        this.ohfY = CameraHeigthOffSet.y;
        this.defaultFieldOfView = FieldOfView;

        this._westOffSet = CameraOffSet;
        this._northOffSet = Quaternion.AngleAxis(90, Vector3.up) * CameraOffSet;
        this._eastOffSet = Quaternion.AngleAxis(180, Vector3.up) * CameraOffSet;
        this._southOffSet = Quaternion.AngleAxis(270, Vector3.up) * CameraOffSet;
    }
    public void LookDirection(CameraDirection direction)
    {
        switch (direction)
        {
            case CameraDirection.West:
                CameraOffSet = _westOffSet;
                break;
            case CameraDirection.East:
                CameraOffSet = _eastOffSet;
                break;
            case CameraDirection.South:
                CameraOffSet = _southOffSet;
                break;
            case CameraDirection.North:
                CameraOffSet = _northOffSet;
                break;
            default:
                break;
        }
    }
    public void LaunchEffect(float value)
    {
        FieldOfView = Mathf.Lerp((value * LaunchEffectMultiplier) + defaultFieldOfView, defaultFieldOfView, 0.5f);
    }
}
