using Assets.Generics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Begin - End
//Start - Stop
//Create - Destroy
//Setup - Unset
//Load - Unload
//Innitialize - Terminate

namespace Assets.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        //Saved Data
        public SaveData Data;

        public enum GameMode { Menu, Singleplayer, Multiplayer, Localgame };
        public GameMode _GameMode;
        public enum GameState { Resumed, Paused };
        public GameState _GameState;

        [HideInInspector] public GameObject States;

        private State State_BallLaunch;
        private State State_BallMoving;

        [HideInInspector] public Map CurrentMap;
        [HideInInspector] public Player CurrentPlayer;
        [HideInInspector] public List<Player> Players;
        [HideInInspector] public List<Map> LocalMultiplayerMaps;
        [HideInInspector] public int MapIndex;

        public Canvas Canvas;
        public Action ActUpdate;
        public State CurrentState;

        public Text DebugTxt;

        //Base Methods
        void Start()
        {
            _GameMode = GameMode.Menu;
            _GameState = GameState.Resumed;

            MapIndex = 0;
            LocalMultiplayerMaps = new List<Map>();
            GetStates();


            //SaveSystem.ClearData();
            //Data = SaveSystem.LoadData();
            MapManager.Instance.Init();
            UiManager.Instance.Init();
            ProfileManager.Instance.Init();
            SkinsManager.Instance.Init();

            Create_FirstPlayer();
            Create_StateMachine();
            Create_Menu();

            CameraManager.Instance.Init();
        }

        //---Action Update
        void Update()
        {
            ActUpdate?.Invoke();
        }

        //------Aux Methods
        private void GetStates()
        {
            States = transform.Find("States").gameObject;
        }

        //Player Methods
        private void Create_FirstPlayer()
        {
            //Create FirstPlayer
            Players = new List<Player>();
            CreatePlayer();
            CurrentPlayer = Players[0];
            //INSTANTIATE BALL
            PlayerBall_Instantiate(Players[0]);
            //SKIN BALL
            //SkinsManager.Instance.LoadCurrentSkin_Ball();
            //SKIN HAT
            //SkinsManager.Instance.LoadCurrentSkin_Hat();

        }
        public void CreatePlayer() => Players.Add(new Player());
        public void RemovePlayer() => Players.RemoveAt(Players.Count - 1);
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
                //Open Ready Check
                UiManager.Instance.OpenInterface_InGameReadyCheck();
            }
            else
            {
                //No Players playing then, end map and opem Local ScoreBoard();
                UiManager.Instance.OpenInterface_LocalScoreboard();
            }
        }
        public Player Get_MainPlayer()
        {
            foreach (Player p in Players)
            {
                if (p.PlayerNum == 0)
                {
                    return p;
                }
            }
            return null;
        }
        private void RemoveAllPlayers()
        {
            Players[0] = CurrentPlayer;
            while (Players.Count != 1)
            {
                RemovePlayer();
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

        //Local MultiPlayer Game
        public void Destroy_LocalGame_Players()
        {
            foreach (Player player in Players)
            {
                if (player.PlayerNum == 0) CurrentPlayer = player;
            }
            State_BallLaunch.Ball = CurrentPlayer.SelectedBall;
            State_BallMoving.Ball = CurrentPlayer.SelectedBall;

            PlayerBall_DestroyAllExtraBalls();
            RemoveAllPlayers();

            MapIndex = 0;
            CurrentPlayer.ResetScore();
        }
        public void Button_LocalGame_NextMap()
        {
            MapIndex++;
            UiManager.Instance.CloseInterface_LocalScoreboard();
            if (MapIndex < LocalMultiplayerMaps.Count)
            {
                MapManager.Instance.SelectedMap = LocalMultiplayerMaps[MapIndex];
                Create_SelectedMap();
            }
            else
            {
                UiManager.Instance.OpenInterface_LocalResults();
            }
        }
        private void Setup_LocalGame_Maps(int mapCount)
        {
            LocalMultiplayerMaps = MapManager.Instance.GetRandomMaps(mapCount);
        }
        private void Create_LocalGame_PlayerOrder()
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
            ChoosePlayer(0);
        }
        
        //State Machine Methods
        private void Create_StateMachine()
        {
            State_BallLaunch = States.GetComponent<State_BallLaunch>();
            State_BallMoving = States.GetComponent<State_BallMoving>();

            Setup_State(State_BallLaunch, State_BallMoving);
            Setup_State(State_BallMoving, State_BallLaunch);

            CurrentState = States.GetComponent<State_BallMoving>();
            CurrentState.StartState();
        }
        private void Setup_State(State state, params State[] connState)
        {
            state.GameManager = this;
            state.Ball = CurrentPlayer.SelectedBall;
            state.ConnectedStates = connState;
        }
        
        //Map Methods
        public void Setup_MenuMap()
        {
            _GameMode = GameMode.Menu;
            Create_Menu();
        }
        public void Setup_Singleplayer()
        {
            _GameMode = GameMode.Singleplayer;
            Create_SelectedMap();
        }
        public void Setup_LocalMultiplayer(int mapCount)
        {
            _GameMode = GameMode.Localgame;

            foreach (Player player in Players)
            {
                player.ResetLocalMultiplayerScore();
            }

            Create_LocalGame_PlayerOrder();
            Setup_LocalGame_Maps(mapCount);

            Create_LocalMultiplayer();
        }
        private void Create_Menu()
        {
            Destroy_CurrentMap();
            CurrentMap = MapManager.Instance.Menu;
            CurrentMap.StartMap();
        }
        private void Create_LocalMultiplayer()
        {
            Destroy_CurrentMap();
            CurrentMap = LocalMultiplayerMaps[0];
            CurrentMap.StartMap();
        }
        private void Create_SelectedMap()
        {
            Destroy_CurrentMap();
            CurrentMap = MapManager.Instance.SelectedMap;
            CurrentMap.StartMap();
        }
        private void Destroy_CurrentMap()
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
            p.Skin_Ball.Load_Skin(p);

            p.SelectedBall.transform.name = "Player: " + (p.PlayerNum + 1);
            p.SelectedBall.Init();
            p.SelectedBall.Player = p;
        }
        public void Player_Hat_Instantiate(Player p)
        {
            if(p.Hat != null) Destroy(p.Hat);
            if(p.Hat_Prefab != null)
            {
                p.Hat = Instantiate(p.Hat_Prefab, p.SelectedBall.transform.position + (Vector3.up * p.SelectedBall.SphereCollider.radius), Quaternion.Euler(-90, 0, 0));
                p.Hat.AddComponent<KeepHatInPlace>();
                p.Hat.GetComponent<KeepHatInPlace>().Player = p;
                p.Hat.transform.localScale *= 0.75f;
                p.Hat.transform.name = "Hat";
            }
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

        //ResetGame
        public void ResetGame()
        {
            CurrentPlayer.SelectedBall.LastPosition = CurrentPlayer.SelectedBall.StartingPosition;
            CurrentPlayer.SelectedBall.GoStartingPosition(true);
            CurrentPlayer.ResetScore();
            CurrentMap.WaypointsReset();
            UiManager.Instance.UpdateMapInfoCurrentStrikes();
            UiManager.Instance.UpdateMapInfoWaypoints();
        }

        //Timescale Methods
        public void TimeScaleStop()
        {
            Time.timeScale = 0;
            _GameState = GameState.Paused;
        }
        public void TimeScaleResume()
        {
            Time.timeScale = 1;
            _GameState = GameState.Resumed;
        }
    }
}