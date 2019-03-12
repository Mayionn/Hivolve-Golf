using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject Prefab;
    [HideInInspector] public GameObject SpawnedPrefab;
    public GameObject StartingPosition;

    public enum GameType { Menu, OneShot, Waypoint, FreeForm};
    public GameType _GameType;

    private UiManager uiManager;
    public int MedalGold;
    public int MedalSilver;
    public int MedalBronze;
    [HideInInspector] public int CurrentStrikes;
    [HideInInspector] public float CurrentTime;
    public Waypoint[] Waypoints;
    public string Author;

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
        //Prepare Strikes and Time
        CurrentStrikes = 0;
        CurrentTime = 0;
        //Prepare UI
        uiManager = GameManager.Instance.UiManager;
        uiManager.SetupInGameUI(this);
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
