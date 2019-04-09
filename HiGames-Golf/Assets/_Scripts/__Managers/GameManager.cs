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
        [HideInInspector] public CameraManager CameraManager;
        [HideInInspector] public SkinsManager SkinsManager;
        [HideInInspector] public GameObject States;

        private State State_BallLaunch;
        private State State_BallMoving;

        public int playernum;
        public Map CurrentMap;
        public Player CurrentPlayer;
        [HideInInspector] public List<Player> Players;
        public List<Player> LocalGamePlayerOrder;

        public Action ActUpdate;
        public State CurrentState;

        //Base Methods
        void Start()
        {
            _GameState = GameState.Menu;

            GetManagers();

            //Create FirstPlayer
            Players = new List<Player>();
            CreateFakePlayers(playernum);
            PlayerBall_Instantiate(Players[0]);
            CurrentPlayer = Players[0];

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
            CameraManager = transform.Find("_CameraManager").GetComponent<CameraManager>();
            SkinsManager = transform.Find("_SkinsManager").GetComponent<SkinsManager>();
            States = transform.Find("States").gameObject;
        }
        public void CreateFakePlayers(int num)
        {
            for (int i = 0; i < num; i++)
            {
                Player p = new Player();
                Players.Add(p);
            }
        }
        public void RemovePlayer()
        {
            //PlayerBall_Destroy(Players[Players.Count - 1]);
            Players.RemoveAt(Players.Count - 1);
        }
        public void RemovePlayers(int count)
        {
            if(count < Players.Count)
            {
                for (int i = 0; i < count; i++)
                {
                    PlayerBall_Destroy(Players[(Players.Count - 1) - i]);
                }
                Players.RemoveRange(Players.Count - count, count);
            }
            else
            {
                Debug.Log("Cant remove that many players");
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
            int num = UnityEngine.Random.Range(0, Players.Count);
            CurrentPlayer = Players[num];
            State_BallLaunch.Ball = CurrentPlayer.SelectedBall;
            State_BallMoving.Ball = CurrentPlayer.SelectedBall;
            //UI
            UiManager.Instance.UpdateCurrentPlayerName();
        }
        public void NextPlayer()
        {
            if(!IsAllPlayersOver())
            {
                int currentIndex = LocalGamePlayerOrder.IndexOf(CurrentPlayer);
                do
                {
                    if (currentIndex == LocalGamePlayerOrder.Count - 1)
                    {
                        currentIndex = 0;
                    }
                    else currentIndex++;
                }
                while (LocalGamePlayerOrder[currentIndex].EndedMap == true);

                CurrentPlayer = LocalGamePlayerOrder[currentIndex];
                State_BallLaunch.Ball = CurrentPlayer.SelectedBall;
                State_BallMoving.Ball = CurrentPlayer.SelectedBall;
                //UI
                UiManager.Instance.UpdateCurrentPlayerName(); 
            }
            else
            {
                UiManager.Instance.OpenInterface_LocalScoreboard();
            }
        }
        private bool IsAllPlayersOver()
        {
            foreach (Player p in LocalGamePlayerOrder)
            {
                if(p.EndedMap == false)
                {
                    return false;
                }
            }
            return true;
        }
        private void CreateLocalGamePlayerOrder()
        {
            LocalGamePlayerOrder = Players;
            // Loops through array
            for (int i = Players.Count-1; i > 0; i--)
            {
                // Randomize a number between 0 and i (so that the range decreases each time)
                int rnd = UnityEngine.Random.Range(0,i);
                
                // Save the value of the current i, otherwise it'll overright when we swap the values
                Player temp = LocalGamePlayerOrder[i];
                
                // Swap the new and old values
                LocalGamePlayerOrder[i] = LocalGamePlayerOrder[rnd];
                LocalGamePlayerOrder[rnd] = temp;
            }
            CurrentPlayer = LocalGamePlayerOrder[0];
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
        //--
        public void SetupMenuMap()
        {
            _GameState = GameState.Menu;
            BuildMenu();
        }
        public void SetupLocalMultiplayer()
        {
            _GameState = GameState.Localgame;
            //TODO: Create player order;
            CreateLocalGamePlayerOrder();
            //TODO: SELECT RANDOM MAP;1
            //TODO: BUILD SELECTED MAP;
            BuildLocalMap();
        }
        public void SetupSingleplayer()
        {
            _GameState = GameState.Singleplayer;
            //TODO: ON UI CHANGE SELECTED MAP;
            BuildSelectedMap();
        }
        private void BuildMenu()
        {
            DestroyCurrentMap();
            CurrentMap = MapManager.Menu;
            CurrentMap.StartMap();
        }
        private void BuildLocalMap()
        {
            DestroyCurrentMap();
            CurrentMap = MapManager.LocalMap;
            CurrentMap.StartMap();
        }
        private void BuildSelectedMap()
        {
            DestroyCurrentMap();
            CurrentMap = MapManager.SelectedMap;
            CurrentMap.StartMap();
        }
        private void DestroyCurrentMap()
        {
            if (CurrentMap != null)
            {
                if (CurrentMap.Waypoints.Length > 0)
                {
                    CurrentMap.WaypointsDestroy();
                }
                Destroy(CurrentMap.SpawnedPrefab);
            }
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        public void PlayerBall_Instantiate(Player p)
        {
            p.SelectedBall = Instantiate(p.Example);
            p.SelectedBall.transform.name = "Player: " + (p.PlayerNum + 1);
            p.SelectedBall.Init();
            p.SelectedBall.Player = p;
        }
        public void PlayerBall_Destroy(Player player)
        {
            if (player.SelectedBall != null)
            {
                Destroy(player.SelectedBall.gameObject);
            }
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        public void ResetGame()
        {
            CurrentPlayer.SelectedBall.LastPosition = CurrentPlayer.SelectedBall.StartingPosition;
            CurrentPlayer.SelectedBall.GoStartingPosition();
            CurrentPlayer.ResetScore();
            CurrentMap.WaypointsReset();
            UiManager.Instance.UpdateMapInfoCurrentStrikes();
            UiManager.Instance.UpdateMapInfoWaypoints();
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        public void TimeScaleStop()
        {
            Time.timeScale = 0;
        }
        public void TimeScaleResume()
        {
            Time.timeScale = 1;
        }
    }
}