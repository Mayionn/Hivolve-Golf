using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMainCamera : MonoBehaviour
{
    public Camera MainCamera;

    private void LateUpdate()
    {
        transform.position = MainCamera.transform.position;
        transform.rotation = MainCamera.transform.rotation;
    }
}
