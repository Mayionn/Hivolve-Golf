using Assets.Generics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum GameState { Menu, Singleplayer, Multiplayer, Localgame };
    public GameState _GameState;

    [HideInInspector] public MapManager MapManager;
    [HideInInspector] public Player[] Players;
    [HideInInspector] public UiManager UiManager;
    [HideInInspector] public LocalGameManager LocalGameManager;
    [HideInInspector] public CameraManager CameraManager;
    [HideInInspector] public GameObject States;

    public int maxPlayers = 3;

    public Map CurrentMap;
    public Ball CurrentBall;
    public Player CurrentPlayer;

    public Action ActUpdate;
    public State CurrentState;

    //Base Methods
    void Start()
    {
        _GameState = GameState.Menu;

        GetManagers();

        //Create FirstPlayer
        Players = new Player[maxPlayers];
        //SetDefaultBall;
        Players[0].SelectedBall = Instantiate(SkinsManager.Instance.DefaultBall);
        Players[1].SelectedBall = Instantiate(SkinsManager.Instance.DefaultBall);
        Players[2].SelectedBall = Instantiate(SkinsManager.Instance.DefaultBall);
        CurrentBall = Players[0].SelectedBall;

        CameraManager.Init();

        UpdateBall();
        BuildMenu();

        BuildStateMachine();
    }

    private void GetManagers()
    {
        MapManager = transform.Find("_MapManager").GetComponent<MapManager>();
        UiManager = transform.Find("_UiManager").GetComponent<UiManager>();
        LocalGameManager = transform.Find("_LocalGameManager").GetComponent<LocalGameManager>();
        CameraManager = transform.Find("_CameraManager").GetComponent<CameraManager>();
        States = transform.Find("States").gameObject;
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
        CurrentMap.StartMap();
    }
    public void BuildLocalMap()
    {
        if (CurrentMap != null)
        {
            Destroy(CurrentMap.SpawnedPrefab);
        }
        CurrentMap = MapManager.LocalMap;
        CurrentMap.StartMap();
    }
    public void BuildSelectedMap()
    {
        if (CurrentMap != null)
        {
            Destroy(CurrentMap.SpawnedPrefab);
        }
        CurrentMap = MapManager.SelectedMap;
        CurrentMap.StartMap();
    }

    private void UpdateBall()
    {
        if (CurrentBall != null)
        {
            Destroy(CurrentBall);
        }
        CurrentBall = Instantiate(CurrentPlayer.SelectedBall);
    }
}