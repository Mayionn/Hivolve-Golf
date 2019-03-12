using Assets.Generics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public MapManager MapManager;
    [HideInInspector] public PlayerManager PlayerManager;
    [HideInInspector] public UiManager UiManager;

    public Map CurrentMap;
    public Ball CurrentBall;

    public enum GameState { Menu, Singleplayer, Multiplayer, Localgame };
    public GameState _GameState;


    public GameObject Camera;
    public Vector3 CameraOffSet;

    public Action ActUpdate;
    public GameObject States;
    public State CurrentState;

    //Base Methods
    void Start()
    {
        _GameState = GameState.Menu;

        MapManager = transform.Find("_MapManager").GetComponent<MapManager>();
        PlayerManager = transform.Find("_PlayerManager").GetComponent<PlayerManager>();
        UiManager = transform.Find("_UiManager").GetComponent<UiManager>();

        UpdateBall();
        BuildMenu();

        BuildStateMachine();

        SetUpCamera();
    }

    void FixedUpdate()
    {
        Physics.autoSimulation = false;

        Physics.Simulate(0.02f);
    }

    void Update()
    {
        ActUpdate?.Invoke();
    }

    void LateUpdate()
    {
        Camera.transform.position = CurrentBall.transform.position + CameraOffSet;
        Camera.transform.LookAt(CurrentBall.transform.position);
    }

    //Aux Methods
    private void BuildStateMachine()
    {
        State State_BallLaunch = States.GetComponent<State_BallLaunch>();
        State State_BallMoving = States.GetComponent<State_BallMoving>();

        SetupState(State_BallLaunch, State_BallMoving);
        SetupState(State_BallMoving, State_BallLaunch);

        CurrentState = States.GetComponent<State_BallMoving>();
        CurrentState.StartState();
    }
    private void SetupState(State state, params State[] connState)
    {
        state.GameManager = this;
        state.Ball = CurrentBall;
        state.ConnectedStates = connState;
    }

    public void BuildMenu()
    {
        //Map 0 equals Menu
        CurrentMap = MapManager.Menu;
        CurrentMap.StartMap(CurrentBall);
    }
    public void BuildSelectedMap()
    {
        if (CurrentMap != null)
        {
            Destroy(CurrentMap.SpawnedPrefab);
        }
        CurrentMap = MapManager.SelectedMap;
        CurrentMap.StartMap(CurrentBall);
    }

    private void UpdateBall()
    {
        if (CurrentBall != null)
        {
            Destroy(CurrentBall);
        }
        CurrentBall = Instantiate(PlayerManager.SelectedBall);
    }

    private void SetUpCamera()
    {
        CameraOffSet = new Vector3(PlayerManager.SelectedBall.transform.position.x + CameraOffSet.x,
                                    CameraOffSet.y,
                                    PlayerManager.SelectedBall.transform.position.z + CameraOffSet.z);
    }
}