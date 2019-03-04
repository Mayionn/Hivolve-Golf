using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject Prefab;
    [HideInInspector] public GameObject SpawnedPrefab;
    public GameObject StartingPosition;
    public Waypoint[] Waypoints;

    public enum GameType { Menu, OneShot, Waypoint, FreeForm};
    public GameType _GameType;

    public void StartMap(Ball _ball)
    {
        //Instantiate Map
        SpawnedPrefab = Instantiate(Prefab);
        //Hide StartingPosition
        StartingPosition.GetComponent<MeshRenderer>().enabled = false;
        StartingPosition.transform.Find("Direction").GetComponent<MeshRenderer>().enabled = false;
        //SetUp Ball
        SetupBall(_ball);
        //Prepare Waypoints
        SetupWaypoints();
    }

    private void SetupBall(Ball _ball)
    {
        _ball.StopAtPosition(StartingPosition.transform.position);
        _ball.StartingPosition = StartingPosition.transform.position;
        _ball.LastPosition = StartingPosition.transform.position;
    }

    private void SetupWaypoints()
    {
        if (Waypoints.Length != 0)
        {
            for (int i = 0; i < Waypoints.Length; i++)
            {
                Waypoints[i].PrepareWaypoint();
            }
        }
    }
}
