using Assets.Generics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public enum GameState { Menu, Singleplayer, Multiplayer, Localgame };
        public GameState _GameState;

        [HideInInspector] public MapManager MapManager;
        [HideInInspector] public LocalGameManager LocalGameManager;
        [HideInInspector] public CameraManager CameraManager;
        [HideInInspector] public SkinsManager SkinsManager;
        [HideInInspector] public GameObject States;

        private State State_BallLaunch;
        private State State_BallMoving;

        public int playernum;
        public Map CurrentMap;
        public Player CurrentPlayer;
        [HideInInspector] public Player[] Players;

        public Action ActUpdate;
        public State CurrentState;

        //Base Methods
        void Start()
        {
            _GameState = GameState.Menu;

            GetManagers();

            //Create FirstPlayer
            CreateFakePlayers(playernum);
            CurrentPlayer = Players[0];

            UiManager.Instance.Init();

            BuildStateMachine();
            BuildMenu();

            CameraManager.Init();
        }

        //---Action Update
        void Update()
        {
            ActUpdate?.Invoke();
        }
        //---Deterministic Physics
        void FixedUpdate()
        {
            Physics.autoSimulation = false;

            Physics.Simulate(0.02f);
        }

        //------Aux Methods
        private void GetManagers()
        {
            MapManager = transform.Find("_MapManager").GetComponent<MapManager>();
            LocalGameManager = transform.Find("_LocalGameManager").GetComponent<LocalGameManager>();
            CameraManager = transform.Find("_CameraManager").GetComponent<CameraManager>();
            SkinsManager = transform.Find("_SkinsManager").GetComponent<SkinsManager>();
            States = transform.Find("States").gameObject;
        }
        private void CreateFakePlayers(int num)
        {
            Players = new Player[num];
            for (int i = 0; i < num; i++)
            {
                Players[i] = new Player(i);
                Players[i].SelectedBall.Player = Players[i];
            }
        }
        public void ChooseCurrentPlayer(int index)
        {
            CurrentPlayer = Players[index];
            State_BallLaunch.Ball = CurrentPlayer.SelectedBall;
            State_BallMoving.Ball = CurrentPlayer.SelectedBall;
        }
        public void ChooseCurrentPlayerRandom()
        {
            int num = UnityEngine.Random.Range(0, Players.Length);
            CurrentPlayer = Players[num];
            State_BallLaunch.Ball = CurrentPlayer.SelectedBall;
            State_BallMoving.Ball = CurrentPlayer.SelectedBall;
            //UI
            UiManager.Instance.UpdateCurrentPlayerName();
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        private void BuildStateMachine()
        {
            State_BallLaunch = States.GetComponent<State_BallLaunch>();
            State_BallMoving = States.GetComponent<State_BallMoving>();

            SetupState(State_BallLaunch, State_BallMoving);
            SetupState(State_BallMoving, State_BallLaunch);

            CurrentState = States.GetComponent<State_BallMoving>();
            CurrentState.StartState();
        }
        private void SetupState(State state, params State[] connState)
        {
            state.GameManager = this;
            state.Ball = CurrentPlayer.SelectedBall;
            state.ConnectedStates = connState;
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
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
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        public void PlayerBall_Instantiate(Player player)
        {
            player.SelectedBall = Instantiate(player.Example);
            player.SelectedBall.Init();
        }
        public void PlayerBall_Update()
        {
            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i].SelectedBall != null)
                {
                    Destroy(Players[i].SelectedBall);
                }
                Players[i].SelectedBall = Instantiate(Players[i].Example);
            }
        }
        public void PlayerBall_Destroy(Player player)
        {
            if(player.SelectedBall.gameObject != null)
            Destroy(player.SelectedBall.gameObject);
        }
    }
}