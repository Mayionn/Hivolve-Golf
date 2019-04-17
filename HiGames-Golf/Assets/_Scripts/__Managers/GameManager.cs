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

        [HideInInspector] public CameraManager CameraManager;
        [HideInInspector] public SkinsManager SkinsManager;
        [HideInInspector] public GameObject States;

        private State State_BallLaunch;
        private State State_BallMoving;

        public Map CurrentMap;
        public Player CurrentPlayer;
        [HideInInspector] public List<Player> Players;

        public Action ActUpdate;
        public State CurrentState;

        //Base Methods
        void Start()
        {
            _GameState = GameState.Menu;

            GetManagers();

            //Create FirstPlayer
            Players = new List<Player>();
            CreatePlayer();
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

        //------Aux Methods
        private void GetManagers()
        {
            CameraManager = transform.Find("_CameraManager").GetComponent<CameraManager>();
            SkinsManager = transform.Find("_SkinsManager").GetComponent<SkinsManager>();
            States = transform.Find("States").gameObject;
        }
        public void CreatePlayer() => Players.Add(new Player());
        public void RemovePlayer() => Players.RemoveAt(Players.Count - 1);
        public void RemoveAllPlayers()
        {
            //Removes everyplayer except the main one
            //The main one is always the one on index 0
            Players[0] = CurrentPlayer;
            while (Players.Count != 1)
            {
                RemovePlayer();
            }
        }
        public void RemoveLocalgamePlayers()
        {
            ChooseMainPlayer();

            PlayerBall_DestroyAllExtraBalls();
            RemoveAllPlayers();

            CurrentPlayer.ResetScore();
        }
        public void ChooseMainPlayer()
        {
            foreach (Player player in Players)
            {
                if (player.PlayerNum == 0) CurrentPlayer = player; 
            }
            State_BallLaunch.Ball = CurrentPlayer.SelectedBall;
            State_BallMoving.Ball = CurrentPlayer.SelectedBall;
        }
        public void ChoosePlayer(int index)
        {
            CurrentPlayer = Players[index];
            State_BallLaunch.Ball = CurrentPlayer.SelectedBall;
            State_BallMoving.Ball = CurrentPlayer.SelectedBall;
        }
        public void NextPlayer()
        {
            if(!IsAllPlayersOver())
            {
                int currentIndex = Players.IndexOf(CurrentPlayer);
                do
                {
                    if (currentIndex == Players.Count - 1)
                    {
                        currentIndex = 0;
                    }
                    else currentIndex++;
                }
                while (Players[currentIndex].EndedMap == true);

                CurrentPlayer = Players[currentIndex];
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
            foreach (Player p in Players)
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
            // Loops through array
            for (int i = Players.Count-1; i > 0; i--)
            {
                // Randomize a number between 0 and i (so that the range decreases each time)
                int rnd = UnityEngine.Random.Range(0,i);
                
                // Save the value of the current i, otherwise it'll overright when we swap the values
                Player temp = Players[i];
                
                // Swap the new and old values
                Players[i] = Players[rnd];
                Players[rnd] = temp;
            }
            CurrentPlayer = Players[0];
        }
        
        //State Machine Methods
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
        
        //Map Methods
        public void SetupMenuMap()
        {
            _GameState = GameState.Menu;
            BuildMenu();
        }
        public void SetupSingleplayer()
        {
            _GameState = GameState.Singleplayer;
            BuildSelectedMap();
        }
        public void SetupLocalMultiplayer()
        {
            _GameState = GameState.Localgame;
            CreateLocalGamePlayerOrder();
            //TODO: SELECT RANDOM MAP;1
            //TODO: BUILD SELECTED MAP;
            BuildLocalMap();
        }
        private void BuildMenu()
        {
            DestroyCurrentMap();
            CurrentMap = MapManager.Instance.Menu;
            CurrentMap.StartMap();
        }
        private void BuildLocalMap()
        {
            DestroyCurrentMap();
            CurrentMap = MapManager.Instance.LocalMap;
            CurrentMap.StartMap();
        }
        private void BuildSelectedMap()
        {
            DestroyCurrentMap();
            CurrentMap = MapManager.Instance.SelectedMap;
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
        
        //Player Ball Methods
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
        public void PlayerBall_DestroyAllExtraBalls()
        {
            foreach (Player p in Players)
            {
                if (p.PlayerNum != 0)
                {
                    PlayerBall_Destroy(p);
                }
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
        //Timescale Methods
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