using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Vector3 Position;
    public Vector3 Scale;
    public bool _Reached = false;

    public void UnPrepareWaypoint() => _Reached = false;
    public void SetReached() => _Reached = true;
}
