using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Managers;
using Assets.Generics;
using System;

public class CameraManager : Singleton<CameraManager>
{
    public enum CameraDirection { West,East,South,North}
    public GameObject Camera;
    public Vector3 CameraOffSet;
    public Vector3 CameraHeigthOffSet;

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

    public void Init()
    {
        CameraOffSet = new Vector3(GameManager.Instance.CurrentPlayer.SelectedBall.transform.position.x + CameraOffSet.x,
                                    CameraOffSet.y,
                                    GameManager.Instance.CurrentPlayer.SelectedBall.transform.position.z + CameraOffSet.z);

        ofY = CameraOffSet.y;
        ohfY = CameraHeigthOffSet.y;

        _westOffSet = CameraOffSet;
        _northOffSet = Quaternion.AngleAxis(90, Vector3.up) * CameraOffSet;
        _eastOffSet = Quaternion.AngleAxis(180, Vector3.up) * CameraOffSet;
        _southOffSet = Quaternion.AngleAxis(270, Vector3.up) * CameraOffSet;
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

    void LateUpdate()
    {
        float velocity = GameManager.Instance.CurrentPlayer.SelectedBall.RigBody.velocity.magnitude;

        Vector3 newOffSet = new Vector3(CameraOffSet.x, ofY + (velocity / OffSetLaunchRange), CameraOffSet.z);
        Vector3 newCameraHeightOffSet = new Vector3(CameraHeigthOffSet.x, ohfY - (velocity / HeigthOffSetLaunchRange), CameraHeigthOffSet.z);


        CameraHeigthOffSet = Vector3.Lerp(CameraHeigthOffSet, newCameraHeightOffSet, Time.deltaTime * OffSetLaunchRecovery);
        CameraOffSet = Vector3.Lerp(CameraOffSet, newOffSet, Time.deltaTime * HeigthOffSetLaunchRecovery);


        Camera.transform.position = GameManager.Instance.CurrentPlayer.SelectedBall.transform.position + CameraOffSet;
        Camera.transform.LookAt(GameManager.Instance.CurrentPlayer.SelectedBall.transform.position + CameraHeigthOffSet);
    }

}
