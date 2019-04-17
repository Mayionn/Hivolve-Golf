﻿using Assets.Generics;
using Assets.Managers;
using Assets.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{
    //Structs
    [Serializable] public struct UIImages
    {
        public Sprite GoldMedal;
        public Sprite SilverMedal;
        public Sprite BronzeMedal;
        public Sprite StopWatch;
        public Sprite Waypoint;
        public Sprite Strikes;
        public Sprite Hidden;
        public Sprite Reset;
    }
    [Serializable] public struct UIBackgroundImages
    {
        public Sprite DefaultBackground;
    }
    [Serializable] public struct InfoInGame
    {
        public Text MedalGold;
        public Text MedalSilver;
        public Text MedalBronze;
        public Image ImgMedalGold;
        public Image ImgMedalSilver;
        public Image ImgMedalBronze;
        public Text CurrentTime;
        public Text CurrentStrikes;
        public Image ImgCurrentStrikes;
        public Text Waypoint;
        public Image ImgWaypoint;
        public Text BestTime;
        public Text BestStrikes;
        public Button ResetGame;
        public Text MapInfo;
        public Text CurrentPlayerInfo;
    }
    [Serializable] public struct InfoLocalGrid
    {
        public int PlayerNum;
        public string PlayerName;
        public Image Image;
        public Ball SelectedBall;
        public Text Txt_PlayerNum;
        public Text Txt_PlayerName;
    }
    [Serializable] public struct InfoCompletedMap
    {
        public GameObject Go;
        public Text Txt_Medal01;
        public Text Txt_Medal02;
        public Text Txt_Medal03;
        public Text Txt_PBStrikes;
        public Text Txt_PBTimer;
        public Text Txt_CurrStrikes;
        public Text Txt_CurrTimer;
        public Image Img_PBStrikes;
        public Image Img_CurrStrikes;
    }
    [Serializable] public struct InfoLocalScoreboard
    {
        public Text PlayerName;
        public Text PlayerNumber;
        public Text PlayerIndexNumber;
        public Text PlayerTimer;
        public Image ImgTimer;
        public Text PlayerStrikes;
        public Image ImgStrikes;
        public Text TotalPoints;
        public Image Medal;
    }
    
    //Struct Variables
    public UIImages UI_Images;
    public UIBackgroundImages UI_BackgroundImages;
    public InfoInGame UI_InGame;
    public InfoCompletedMap UI_CompletedMap;
    public List<InfoLocalScoreboard> UI_LocalScoreboard;

    public GameObject GO_InGame;
    public GameObject GO_MapSelector;
    public GameObject Go_MapDisplay;
    public GameObject GO_LocalScoreboard;
    public UI_LocalMultiplayer UI_LocalMultiplayer;
    
    //CurrentMap and Player
    private Map m;
    private Player p;

    public Canvas canvas;
    private float TOP_POSITION;
    private float MIDDLE_POSITION;
    private float DOWN_POSITION;

    Vector3 UpPos, DownPos;
    Chapter[] chaptersToMove = new Chapter[2];
    private readonly float maxSpeed = 40;
    private float speed;
    private float distanceSoFar = 0;
    private float chapterMoveDistance;
    bool down = false;
    bool activate = false;
    bool isMoving = false;
    readonly float SWIPE_DISTANCE = 50f;
    public bool detectSwipeOnlyAfterRelease = true;
    private Action ActUpdate;

    private void Start()
    {
        float height = canvas.GetComponent<RectTransform>().sizeDelta.y;
        TOP_POSITION = +height;
        MIDDLE_POSITION = 0;
        DOWN_POSITION = -height;
    }

    private void Update()
    {
        ActUpdate?.Invoke();
    }

    //Open / Close --- Interface
    public void OpenInterface_InGameHud()
    {
        m = GameManager.Instance.CurrentMap;
        switch (GameManager.Instance._GameState)
        {
            case GameManager.GameState.Singleplayer:
                GO_InGame.SetActive(true);
                TimerStart();
                IGH_SetCurrentPlayerInfo();
                IGH_SetMapInfo();
                IGH_SetMapInfoMedals();
                IGH_SetMapInfoWaypoints();
                IGH_SetMapInfoCurrentStrikes();
                break;
            case GameManager.GameState.Multiplayer:
                break;
            case GameManager.GameState.Localgame:
                GO_InGame.SetActive(true);
                TimerStart();
                IGH_SetCurrentPlayerInfo();
                IGH_SetMapInfo();
                IGH_SetMapInfoMedals();
                IGH_SetMapInfoWaypoints();
                IGH_SetMapInfoCurrentStrikes();
                break;
            default:
                break;
        }
    }
    public void OpenInterface_MapSelector()
    {
        GameManager.Instance.TimeScaleStop();
        GO_MapSelector.SetActive(true);
        MS_Init();
    }
    public void OpenInterface_LocalMultiplayer()
    {
        GameManager.Instance.TimeScaleStop();
        UI_LocalMultiplayer.gameObject.SetActive(true);
        UI_LocalMultiplayer.Init();
    }
    public void OpenInterface_CompletedMap()
    {
        GameManager.Instance.TimeScaleStop();
        CloseInterface_InGameHud();
        UI_CompletedMap.Go.SetActive(true);
        CM_Init();
    }
    public void OpenInterface_LocalScoreboard()
    {
        GameManager.Instance.TimeScaleStop();
        CloseInterface_InGameHud();
        LGS_CheckBestPlayerOnMap();
        LGS_GivePlayersScores();
        GO_LocalScoreboard.SetActive(true);
    }
    public void CloseInterface_InGameHud()
    {
        GO_InGame.SetActive(false);
        TimerStop();
    }
    public void CloseInterface_MapSelector()
    {
        GameManager.Instance.TimeScaleResume();
        MS_Destroy_Chapter(1);
        if (GameManager.Instance.CurrentMap != MapManager.Instance.Menu)
        {
            GameManager.Instance.SetupMenuMap();
        }
        else GameManager.Instance.CurrentPlayer.SelectedBall.GoStartingPosition();
        GO_MapSelector.SetActive(false);
        ActUpdate -= MS_DetectSwipe;
    }
    public void CloseInterface_LocalMultiplayer()
    {
        GameManager.Instance.TimeScaleResume();
        UI_LocalMultiplayer.gameObject.SetActive(false);
    }
    public void CloseInterface_CompletedMap()
    {
        GameManager.Instance.TimeScaleResume();
        UI_CompletedMap.Go.SetActive(false);
    }
    public void CloseInterface_LocalScoreboard()
    {
        GameManager.Instance.TimeScaleResume();
        GO_LocalScoreboard.SetActive(false);
    }

    //MS --- Map Selector
    public void MS_Init()
    {
        MS_Display_Chapter(1, MIDDLE_POSITION);
        MS_Display_Chapter(2, TOP_POSITION);
        MapManager.Instance.CurrentChapterNumber = 1;
        ActUpdate += MS_DetectSwipe;
    }
    public void MS_Display_Chapter(int num, float pos)
    {
        if (num <= MapManager.Instance.Chapters.Count)
        {
            Chapter c = MapManager.Instance.Chapters[num - 1];
            Vector2 posVec = new Vector3(0, pos);

            //Instantiate Background
            c.BackGround = new GameObject("Chapter" + num + "BackGround", typeof(RectTransform));
            c.BackGround.AddComponent<Image>();
            c.BackGround.GetComponent<Image>().sprite = c.Sprite;
            c.BackGround.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            c.BackGround.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            c.BackGround.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            c.BackGround.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            c.BackGround.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            c.BackGround.GetComponent<RectTransform>().position += new Vector3(posVec.x, posVec.y, 0); 

            c.BackGround.transform.SetParent(GO_MapSelector.transform.Find("Chapter"), false);
            c.BackGround.SetActive(true);

            //Instantiate Displays
            for (int i = 0; i < c.Displays.Length; i++)
            {
                int level = i + 1;
                Display d = c.Displays[i];
                d.GO_Copy = Go_MapDisplay;
                d.POS = c.DisplayInfos[i].pos;
                d.GO = Instantiate(d.GO_Copy, d.POS.anchoredPosition + posVec, Quaternion.identity);
                d.GO.transform.name = "Level: " + level;
                d.GO.transform.SetParent(GO_MapSelector.transform.Find("Chapter"), false);
                d.GO.SetActive(true);
            }
            c.SetDisplays(c.DisplayInfos);
        }
        else Debug.Log("Chapter number not available!");

        //217900790 - numero reaver codigo aixa direta
    }
    public bool MS_Destroy_Chapter(int num)
    {
        if (num > 0 && num <= MapManager.Instance.Chapters.Count)
        {
            Chapter c = MapManager.Instance.Chapters[num - 1];
            GameObject b = c.BackGround;

            Destroy(b);
            for (int i = 0; i < c.Displays.Length; i++)
            {
                Display d = c.Displays[i];
                Destroy(d.GO);
            }
            return true;
        }
        else return false;
    }
    public void MS_BUTTON_StartMap(GetMap gm)
    {
        if (!isMoving)
        {
            MapManager.Instance.SelectedMap = gm.map;
            CloseInterface_MapSelector();
            GameManager.Instance.SetupSingleplayer();
        }
    }
    private void MS_MoveDown()
    {
        int numberOfChapters = MapManager.Instance.Chapters.Count;
        int chapterNum = MapManager.Instance.CurrentChapterNumber;

        if (chapterNum == numberOfChapters) Debug.Log("Cant go down, no chapters up");
        else if (chapterNum < numberOfChapters)
        {
            MS_Display_Chapter(chapterNum + 2, TOP_POSITION);

            if(chapterNum != 1)
                MS_Destroy_Chapter(chapterNum - 1);
            MS_SetupDownAnimation();
            MapManager.Instance.CurrentChapterNumber++;

            ActUpdate += MS_Animate;
        }
        else if (chapterNum > numberOfChapters) Debug.Log("Something went wrong, current chapter above number of chapters");
    }
    private void MS_MoveUp()
    {
        int numberOfChapters = MapManager.Instance.Chapters.Count;
        int chapterNum = MapManager.Instance.CurrentChapterNumber;

        if (chapterNum == 1) Debug.Log("Cant go up, no chapters up");
        else if (chapterNum > 1)
        {
            if(chapterNum != 2)
            {
                MS_Display_Chapter(chapterNum - 2, DOWN_POSITION);
            }
            MS_Destroy_Chapter(chapterNum + 1);

            MS_SetupUpAnimation();
            MapManager.Instance.CurrentChapterNumber--;

            ActUpdate += MS_Animate;
        }
    }
    private void MS_SetupDownAnimation()
    {
        //Number active Chapters
        int curr = MapManager.Instance.CurrentChapterNumber;

        for (int i = 0; i < 2; i++)
        {
            Chapter c = MapManager.Instance.Chapters[(curr + i) - 1];
            chaptersToMove[i] = c;
            foreach (Display display in chaptersToMove[i].Displays)
            {
                display.LAST_POS = display.POS;
            }
        }
        chapterMoveDistance = -TOP_POSITION;
        distanceSoFar = 0;
        speed = -maxSpeed;
    }
    private void MS_SetupUpAnimation()
    {
        //Number active Chapters
        int curr = MapManager.Instance.CurrentChapterNumber;

        for (int i = 0; i < 2; i++)
        {
            Chapter c = MapManager.Instance.Chapters[(curr - i) -1];
            chaptersToMove[i] = c;
            foreach (Display display in chaptersToMove[i].Displays)
            {
                display.LAST_POS = display.POS;
            }
        }
        chapterMoveDistance = TOP_POSITION;
        distanceSoFar = 0;
        speed = maxSpeed;
    }
    private void MS_Animate()
    {
        distanceSoFar += maxSpeed;
        //Running Down
        if(distanceSoFar <= Math.Abs(chapterMoveDistance))
        {
            foreach (Chapter c in chaptersToMove)
            {
                c.BackGround.GetComponent<RectTransform>().localPosition += new Vector3(0, speed);

                foreach (Display d in c.Displays)
                {
                    d.GO.GetComponent<RectTransform>().offsetMax += new Vector2(0, speed);
                    d.GO.GetComponent<RectTransform>().offsetMin += new Vector2(0, speed);
                }
            }
        }
        //Sthap
        else
        {
            int t = 1;
            for (int i = 0; i < 2; i++)
            {
                chaptersToMove[i].BackGround.GetComponent<RectTransform>().localPosition = new Vector3(0, chapterMoveDistance * t, 0);

                foreach (Display d in chaptersToMove[i].Displays)
                {
                    d.GO.GetComponent<RectTransform>().localPosition = new Vector2(d.LAST_POS.localPosition.x, d.LAST_POS.localPosition.y + chapterMoveDistance * t);
                }

                t--;
            }
            StopAnimation();
        }
    }
    private void StopAnimation()
    {
        ActUpdate -= MS_Animate;
        isMoving = false;
    }
    private void MS_DetectSwipe()
    {
        if (Input.GetMouseButtonDown(0) && !down)
        {
            UpPos = Input.mousePosition;
            DownPos = Input.mousePosition;
            down = true;
        }
        if (down)
        {
            DownPos = Input.mousePosition;
            activate = true;
        }
        if (Input.GetMouseButtonUp(0) && activate)
        {
            down = false;
            activate = false;
            MS_CheckSwipe();
        }
    }
    private void MS_CheckSwipe()
    {
        if (verticalMove() > SWIPE_DISTANCE && verticalMove() > horizontalValMove())
        {
            Debug.Log("Vertical");
            if (DownPos.y - UpPos.y > 0)
            {
                //Swipe Up
                MS_MoveUp();
                Debug.Log("Swipe Up");
            }
            else if (DownPos.y - UpPos.y < 0)
            {
                //Swipe Down
                MS_MoveDown();
                Debug.Log("Swipe Down");
            }
            UpPos = DownPos;
        }
        //SIDE SWIPING - NOT USED
        ////Check if Horizontal swipe
        //else if (horizontalValMove() > SWIPE_DISTANCE && horizontalValMove() > verticalMove())
        //{
        //    //Debug.Log("Horizontal");
        //    if (DownPos.x - UpPos.x > 0)//Right swipe
        //    {
        //        //Swipe Right
        //        Debug.Log("Swipe Right");
        //    }
        //    else if (DownPos.x - UpPos.x < 0)//Left swipe
        //    {
        //        //Swipe Left
        //        Debug.Log("SwipeLeft");
        //    }
        //    UpPos = DownPos;
        //}
        else
        {
            Debug.Log("No Swipe at All");
        }
    }
    private float verticalMove()
    {
        return Mathf.Abs(DownPos.y - UpPos.y);
    }
    private float horizontalValMove()
    {
        return Mathf.Abs(DownPos.x - UpPos.x);
    }

    //IGH --- In Game Hud
    private void IGH_SetCurrentPlayerInfo()
    {
        p = GameManager.Instance.CurrentPlayer;
        UI_InGame.CurrentPlayerInfo.text = "Current Player: " + p.Name
                                                + "\nPlayer Number: " + p.PlayerNum;
    }
    private void IGH_SetMapInfo()
    {
        UI_InGame.MapInfo.text = m.gameObject.name + "\n" + m.Author;
    }
    private void IGH_SetMapInfoMedals()
    {
        UI_InGame.MedalGold.text = m.MedalGold.ToString();
        UI_InGame.ImgMedalGold.sprite = UI_Images.GoldMedal;
        UI_InGame.MedalSilver.text = m.MedalSilver.ToString();
        UI_InGame.ImgMedalSilver.sprite = UI_Images.SilverMedal;
        UI_InGame.MedalBronze.text = m.MedalBronze.ToString();
        UI_InGame.ImgMedalBronze.sprite = UI_Images.BronzeMedal;
    }
    private void IGH_SetMapInfoWaypoints()
    {
        if (m.Waypoints.Length > 0)
        {
            UI_InGame.Waypoint.gameObject.SetActive(true);
            UI_InGame.ImgWaypoint.sprite = UI_Images.Waypoint;
            UI_InGame.Waypoint.text = m.Waypoints.Length + " \\ " + 0;
        }
        else
        {
            UI_InGame.Waypoint.gameObject.SetActive(false);
        }
    }
    private void IGH_SetMapInfoCurrentStrikes()
    {
        UI_InGame.ImgCurrentStrikes.sprite = UI_Images.Strikes;
        UI_InGame.CurrentStrikes.text = "Strikes: 0";
    }

    //CM --- Completed Map
    private void CM_Init()
    {
        m = GameManager.Instance.CurrentMap;
        p = GameManager.Instance.CurrentPlayer;

        m.CheckPersonalBest();
        //UPDATE TEXT
        //Map Medals
        UI_CompletedMap.Txt_Medal01.text = m.MedalGold.ToString();
        UI_CompletedMap.Txt_Medal02.text = m.MedalSilver.ToString();
        UI_CompletedMap.Txt_Medal03.text = m.MedalBronze.ToString();
        //PB
        UI_CompletedMap.Txt_PBStrikes.text = m.PB.Strikes.ToString();
        UI_CompletedMap.Txt_PBTimer.text = m.PB.Time.ToString();
        //Current Score
        UI_CompletedMap.Txt_CurrStrikes.text = p.Strikes.ToString();
        UI_CompletedMap.Txt_CurrTimer.text = p.Timer.ToString();
        //UPDATE IMAGES
        CM_UpdateScoreImages();
    }
    public void CM_ButtonReset()
    {
        CloseInterface_CompletedMap();
        OpenInterface_InGameHud();
        GameManager.Instance.ResetGame();
    }
    public void CM_ButtonMenu()
    {
        CloseInterface_CompletedMap();
        OpenInterface_MapSelector();
        //GameManager.Instance.SetupMenuMap();
    }
    public void CM_UpdateScoreImages()
    {
        //UPDATE PERSONAL BEST - PB
        if (m.PB.Strikes <= m.MedalGold)
        {
            UI_CompletedMap.Img_PBStrikes.sprite = UI_Images.GoldMedal;
        }
        else if (m.PB.Strikes <= m.MedalSilver)
        {
            UI_CompletedMap.Img_PBStrikes.sprite = UI_Images.SilverMedal;
        }
        else if (m.PB.Strikes <= m.MedalBronze)
        {
            UI_CompletedMap.Img_PBStrikes.sprite = UI_Images.BronzeMedal;
        }

        //UPDATE CURRENT SCORE
        if (p.Strikes <= m.MedalGold)
        {
            UI_CompletedMap.Img_CurrStrikes.sprite = UI_Images.GoldMedal;
        }
        else if (p.Strikes <= m.MedalSilver)
        {
            UI_CompletedMap.Img_CurrStrikes.sprite = UI_Images.SilverMedal;
        }
        else if (p.Strikes <= m.MedalBronze)
        {
            UI_CompletedMap.Img_CurrStrikes.sprite = UI_Images.BronzeMedal;
        }
        else
        {
            UI_CompletedMap.Img_CurrStrikes.sprite = null;
        }
    }

    //LGS --- Local Game Scoreboard
    public void LGS_BUTTON_Menu()
    {
        CloseInterface_LocalScoreboard();

        GameManager.Instance.RemoveLocalgamePlayers();

        GameManager.Instance.SetupMenuMap();
    }
    public void LGS_SaveScore(Player p)
    {
        InfoLocalScoreboard i = UI_LocalScoreboard[p.PlayerNum];
        i.PlayerName.text = p.Name;
        i.PlayerNumber.text = "Player: " + p.PlayerNum.ToString();
        i.PlayerIndexNumber.text = p.PlayerNum.ToString();
        i.PlayerStrikes.text = p.Strikes.ToString();
        p.TruncateTimer();
        i.PlayerTimer.text = p.Timer.ToString();
        i.TotalPoints.text = p.LocalgamePoints.ToString();
    }
    public void LGS_Init()
    {
        LGS_SetImages();
        LGS_HideRows();


    }
    private void LGS_SetImages()
    {
        for (int i = 0; i < UI_LocalScoreboard.Count; i++)
        {
            LGS_ChangeImages(UI_LocalScoreboard[i]);
        }
    }
    private void LGS_HideRows()
    {
        for (int i = 0; i < UI_LocalScoreboard.Count; i++)
        {
            if (i >= GameManager.Instance.Players.Count)
            {
                LGS_Hide(UI_LocalScoreboard[i]);
            }
        }
    }
    private void LGS_ChangeImages(InfoLocalScoreboard i)
    {
        i.ImgTimer.sprite = UI_Images.StopWatch;
        i.ImgStrikes.sprite = UI_Images.Strikes;
    }
    private void LGS_Hide(InfoLocalScoreboard i)
    {
        i.PlayerName.text = "";
        i.PlayerNumber.text = "";
        i.PlayerTimer.text = "";
        i.ImgTimer.sprite = UI_Images.Hidden;
        i.PlayerStrikes.text = "";
        i.ImgStrikes.sprite = UI_Images.Hidden;
        i.TotalPoints.text = "";
        i.Medal.sprite = UI_Images.Hidden;
    }
    private void LGS_CheckBestPlayerOnMap()
    {
        //int count = GameManager.Instance.Players.Count;
        //List<InfoLocalScoreboard> li = new List<InfoLocalScoreboard>(count);
        //for (int i = 0; i < count; i++)
        //{
        //    li[i] = UI_LocalScoreboard[i];
        //}

        UI_LocalScoreboard.Sort(
            delegate (InfoLocalScoreboard p1, InfoLocalScoreboard p2)
            {
                if (p1.PlayerTimer.text != "" && p2.PlayerTimer.text != "")
                {
                    int compareStrikes = p1.PlayerStrikes.text.CompareTo(p2.PlayerStrikes.text);
                    if (compareStrikes == 0)
                    {
                        return p1.PlayerTimer.text.CompareTo(p2.PlayerTimer.text);
                    }
                    return compareStrikes;
                }
                else return 1;
            }
        );
    }
    private void LGS_GivePlayersScores()
    {
        /* Changes the medal icon on the scoreboard depending on the performance of the player on the current map;
         * Also adds points to the players depending on their position;
         * Follows the order of the players --- this happens after ordering the list by numberOfStrikes; */

        for (int i = 0; i < GameManager.Instance.Players.Count; i++)
        {
            int stPlace = 4;
            int ndPlace = 3;
            int rdPlace = 2;
            int thPlace = 1;
            if (UI_LocalScoreboard[i].PlayerTimer.text != "")
            {
                switch (i)
                {
                    case 0:
                        LGS_SetScore(UI_LocalScoreboard[i], UI_Images.GoldMedal, stPlace);
                        break;
                    case 1:
                        LGS_SetScore(UI_LocalScoreboard[i], UI_Images.SilverMedal, ndPlace);
                        break;
                    case 2:
                        LGS_SetScore(UI_LocalScoreboard[i], UI_Images.BronzeMedal, rdPlace);
                        break;
                    default:
                        LGS_SetScore(UI_LocalScoreboard[i], UI_Images.Hidden, thPlace);
                        break;
                }
            }
        }
    }
    private void LGS_SetScore(InfoLocalScoreboard i, Sprite s, int p)
    {
        Player lp = GameManager.Instance.Players[int.Parse(i.PlayerIndexNumber.text)];
        i.TotalPoints.text = lp.LocalgamePoints.ToString() + " + " + p;
        lp.LocalgamePoints += p;
        i.Medal.sprite = s;
    }

    //Update Info Methods
    public void UpdateCurrentPlayerName()
    {
        IGH_SetCurrentPlayerInfo();
    }
    public void UpdateMapInfoWaypoints()
    {
        p = GameManager.Instance.CurrentPlayer;
        m = GameManager.Instance.CurrentMap;
        p.WaypointCounter = 0;
        //Percorrer Waypoints!
        for (int i = 0; i < m.Waypoints.Length; i++)
        {
            List<int> rp = m.Waypoints[i].GetComponent<Waypoint>().ReachedPlayers;
            //Percorrer ReachedPlayers em cada Waypoint!
            for (int o = 0; o < rp.Count; o++)
            {
                //Se o Player num estiver na lista,
                //é porque o player já chegou ao determinado waypoint
                if (rp[o] == p.PlayerNum)
                {
                    //Incrementar o contador de strikes, no jogado atual
                    p.WaypointCounter++;
                }
            }
        }
        UI_InGame.Waypoint.text = m.Waypoints.Length + " \\ " + p.WaypointCounter;
    }
    public void UpdateMapInfoCurrentStrikes()
    {
        UI_InGame.CurrentStrikes.text = "Strikes: " + GameManager.Instance.CurrentPlayer.Strikes;
    }

    //Timer Functions
    private void TimerStart()
    {
        GameManager.Instance.ActUpdate += TimerCount;
    }
    private void TimerCount()
    {
        GameManager.Instance.CurrentPlayer.Timer += Time.deltaTime;
        float t = GameManager.Instance.CurrentPlayer.Timer;
        //UI_InGame.Time.text = timer.ToString();
        UI_InGame.CurrentTime.text = string.Format("{0:#0} : {1:00} : {2:00}", 
                                Mathf.Floor(t / 60),
                                Mathf.Floor(t) % 60,
                                Mathf.Floor((t * 100) % 100));
    }
    private void TimerReset()
    {
        GameManager.Instance.CurrentPlayer.Timer = 0;
    }
    private void TimerStop()
    {
        GameManager.Instance.ActUpdate -= TimerCount;
    }
}