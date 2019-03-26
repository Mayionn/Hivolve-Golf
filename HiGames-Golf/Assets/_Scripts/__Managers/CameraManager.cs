using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Managers;
using Assets.Generics;

public class CameraManager : Singleton<CameraManager>
{
    public GameObject Camera;
    public Vector3 CameraOffSet;
    public Vector3 CameraHeightOffSet;

    public void Init()
    {
        CameraOffSet = new Vector3(GameManager.Instance.CurrentPlayer.SelectedBall.transform.position.x + CameraOffSet.x,
                                    CameraOffSet.y,
                                    GameManager.Instance.CurrentPlayer.SelectedBall.transform.position.z + CameraOffSet.z);
    }


    void LateUpdate()
    {
        Camera.transform.position = GameManager.Instance.CurrentPlayer.SelectedBall.transform.position + CameraOffSet;
        Camera.transform.LookAt(GameManager.Instance.CurrentPlayer.SelectedBall.transform.position + CameraHeightOffSet);
    }

}
