using Assets.Generics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

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
        public GameState _GameState;
        public GameMode _GameMode;

        public LayerMask PlayerLayer;

        [HideInInspector] public GameObject States;
        public State CurrentState;
        private State state_BallLaunch;
        private State state_BallMoving;

        [HideInInspector] public Map CurrentMap;
        [HideInInspector] public Player CurrentPlayer;
        [HideInInspector] public List<Player> Players;
        [HideInInspector] public List<Map> LocalMultiplayerMaps;
        [HideInInspector] public int MapIndex;

        public Canvas Canvas;
        public Action ActUpdate;

        //Base Methods
        void Start()
        {
            _GameMode = GameMode.Menu;
            _GameState = GameState.Resumed;

            MapIndex = 0; //???? dont remember why its here
            LocalMultiplayerMaps = new List<Map>();
            GetStates();

            //SaveManager.Instance.ClearData();
            SaveManager.Instance.Init();
            MapManager.Instance.Init();
            UiManager.Instance.Init();
            ProfileManager.Instance.Init();
            SkinsManager.Instance.Init();
            AudioManager.Instance.Init();

            Create_FirstPlayer();
            Create_StateMachine();

            SaveManager.Instance.LoadMapProgress();
            SaveManager.Instance.LoadUnlockedSkins_Balls();
            SaveManager.Instance.LoadUnlockedSkins_Hats();
            SaveManager.Instance.LoadUnlockedSkins_Arrows();
            SaveManager.Instance.LoadUnlockedSkins_ForceBars();
            SaveManager.Instance.LoadCurrentSkin_Ball();
            SaveManager.Instance.LoadCurrentSkin_Hat();
            SaveManager.Instance.LoadCurrentSkin_Arrow();
            SaveManager.Instance.LoadCurrentSkin_ForceBar();
            SaveManager.Instance.LoadCurrency();

            CameraManager.Instance.Init();
            Create_Menu();
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
        public void CreatePlayer() => Players.Add(new Player());
        public void RemovePlayer() => Players.RemoveAt(Players.Count - 1);
        public void ChoosePlayer(int index)
        {
            CurrentPlayer = Players[index];
            state_BallLaunch.Ball = CurrentPlayer.SelectedBall;
            state_BallMoving.Ball = CurrentPlayer.SelectedBall;
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
                state_BallLaunch.Ball = CurrentPlayer.SelectedBall;
                state_BallMoving.Ball = CurrentPlayer.SelectedBall;
                //UI
                UiManager.Instance.UpdateCurrentPlayerName();
                UiManager.Instance.UpdateMapInfoCurrentStrikes();
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
        private void Create_FirstPlayer()
        {
            //Create FirstPlayer
            Players = new List<Player>();
            CreatePlayer();
            CurrentPlayer = Players[0];
            //INSTANTIATE BALL
            Player_Ball_Instantiate(Players[0]);
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
            state_BallLaunch.Ball = CurrentPlayer.SelectedBall;
            state_BallMoving.Ball = CurrentPlayer.SelectedBall;

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
            AudioManager.Instance.Play(Sounds.InteractSucess);
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
            state_BallLaunch = States.GetComponent<State_BallLaunch>();
            state_BallMoving = States.GetComponent<State_BallMoving>();

            Setup_State(state_BallLaunch, state_BallMoving);
            Setup_State(state_BallMoving, state_BallLaunch);

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
        public void Player_Arrow_Instantiate(Player p)
        {
            if (p.Arrow != null)
            {
                Destroy(p.Arrow);
            }
            if (p.Arrow_Prefab == null)
            {
                p.Arrow_Prefab = SkinsManager.Instance.ArrowPrefab;
            }
            p.Arrow = Instantiate(p.Arrow_Prefab, Vector3.zero, Quaternion.identity);
            p.Arrow.transform.name = "Direction Arrow";
            p.Arrow.GetComponent<MeshRenderer>().enabled = false;
        }
        public void Player_Ball_Instantiate(Player p) 
        {
            if (p.SelectedBall != null) Destroy(p.SelectedBall.gameObject);
            if(p.Example != null)
            {
                p.SelectedBall = Instantiate(p.Example);
                p.Skin_Ball.Load_Skin(p);
                p.SelectedBall.Init();
                p.SelectedBall.transform.name = "Player: " + (p.PlayerNum + 1);
                p.SelectedBall.Player = p;
            }
        }
        public void Player_ForceBar_Instantiate(Player p)
        {
            if (p.ForceBar != null)
            {
                Destroy(p.ForceBar);
            }
            if (p.ForceBar_Prefab == null)
            {
                 p.ForceBar_Prefab = SkinsManager.Instance.ForceBar_Prefab;
            }
            p.ForceBar = Instantiate(p.ForceBar_Prefab, Vector3.zero, Quaternion.identity);
            p.ForceBar.transform.name = "Force Bar";
            p.ForceBar.GetComponent<MeshRenderer>().enabled = false;
            p.ForceBar.transform.localScale = new Vector3(0.2f, 1, 0.1f);
            p.ForceBar.layer = 9; //Player Layer
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
                p.Hat.layer = 9; //Player Layer
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
            AudioManager.Instance.Play(Sounds.InteractSucess);
            AdsManager.Instance.ShowAd();
        }
        public void Button_BackToMapSelector()
        {
            UiManager.Instance.CloseInterface_InGameHud();
            UiManager.Instance.OpenInterface_MapSelector();
            Destroy_LocalGame_Players();
            AudioManager.Instance.Play(Sounds.InteractSucess);
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