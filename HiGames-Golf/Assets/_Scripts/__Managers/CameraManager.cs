using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject Camera;
    public Vector3 CameraOffSet;

    public void Init()
    {
        CameraOffSet = new Vector3(GameManager.Instance.CurrentBall.transform.position.x + CameraOffSet.x,
                                    CameraOffSet.y,
                                    GameManager.Instance.CurrentBall.transform.position.z + CameraOffSet.z);
    }

    void LateUpdate()
    {
        Camera.transform.position = GameManager.Instance.CurrentBall.transform.position + CameraOffSet;
        Camera.transform.LookAt(GameManager.Instance.CurrentBall.transform.position);
    }

}
