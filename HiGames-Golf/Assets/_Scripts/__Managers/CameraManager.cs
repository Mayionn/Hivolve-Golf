using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Managers;
using Assets.Generics;
using System;

public class CameraManager : Singleton<CameraManager>
{
    public GameObject Camera;
    public Vector3 CameraOffSet;
    public Vector3 CameraHeigthOffSet;

    public float OffSetLaunchRange;
    public float OffSetLaunchRecovery;
    public float HeigthOffSetLaunchRange;
    public float HeigthOffSetLaunchRecovery;


    private float ofY;
    private float ohfY;

    public void Init()
    {
        CameraOffSet = new Vector3(GameManager.Instance.CurrentPlayer.SelectedBall.transform.position.x + CameraOffSet.x,
                                    CameraOffSet.y,
                                    GameManager.Instance.CurrentPlayer.SelectedBall.transform.position.z + CameraOffSet.z);

        ofY = CameraOffSet.y;
        ohfY = CameraHeigthOffSet.y;
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
