using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject Prefab;
    public GameObject SpawnedPrefab;
    public GameObject StartingPosition;
    public Waypoint[] Waypoints;
    
    public void StartMap(Ball _ball)
    {
        //Instantiate Map
        SpawnedPrefab = Instantiate(Prefab);
        //Hide StartingPosition
        StartingPosition.GetComponent<MeshRenderer>().enabled = false;
        StartingPosition.transform.Find("Direction").GetComponent<MeshRenderer>().enabled = false;
        //SetUp Ball
        _ball.transform.position = StartingPosition.transform.position;
        //Prepare Waypoints
        for (int i = 0; i < Waypoints.Length; i++)
        {
            Waypoints[i].PrepareWaypoint();
        }
    }
}
