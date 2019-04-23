using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGameHud : MonoBehaviour
{
    public GameObject UI;

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
    public InfoInGame UI_InGame;

    private Map m;
    private Player p;

    public void Init()
    {
        m = GameManager.Instance.CurrentMap;
        switch (GameManager.Instance._GameMode)
        {
            case GameManager.GameMode.Singleplayer:
                UI.SetActive(true);
                TimerStart();
                SetCurrentPlayerInfo();
                SetMapInfo();
                SetMapInfo_Medals();
                SetMapInfo_Waypoints();
                SetMapInfo_CurrentStrikes();
                SetButtonRestart();
                break;
            case GameManager.GameMode.Multiplayer:
                break;
            case GameManager.GameMode.Localgame:
                UI.SetActive(true);
                TimerStart();
                SetCurrentPlayerInfo();
                SetMapInfo();
                SetMapInfo_Waypoints();
                SetMapInfo_CurrentStrikes();
                HideMapInfo_Medals();
                HideButtonRestart();
                break;
            default:
                break;
        }

        UI_InGame.ImgCurrentStrikes.sprite = UiManager.Instance.UI_Images.Strikes;
        UI_InGame.ImgMedalGold.sprite = UiManager.Instance.UI_Images.GoldMedal;
        UI_InGame.ImgMedalSilver.sprite = UiManager.Instance.UI_Images.SilverMedal;
        UI_InGame.ImgMedalBronze.sprite = UiManager.Instance.UI_Images.BronzeMedal;
    }
    public void Terminate()
    {
        UI.SetActive(false);
        TimerStop();
    }

    //IGH --- In Game Hud
    public void SetCurrentPlayerInfo()
    {
        p = GameManager.Instance.CurrentPlayer;
        UI_InGame.CurrentPlayerInfo.text = "Current Player: " + p.Name
                                                + "\nPlayer Number: " + p.PlayerNum;
    }
    private void SetMapInfo()
    {
        UI_InGame.MapInfo.text = m.gameObject.name + "\n" + m.Author;
    }
    private void SetMapInfo_Medals()
    {
        UI_InGame.MedalGold.text = m.MedalGold.ToString();
        UI_InGame.ImgMedalGold.color = Color.white;
        UI_InGame.MedalSilver.text = m.MedalSilver.ToString();
        UI_InGame.ImgMedalSilver.color = Color.white;
        UI_InGame.MedalBronze.text = m.MedalBronze.ToString();
        UI_InGame.ImgMedalBronze.color = Color.white;
    }
    private void SetMapInfo_CurrentStrikes()
    {
        UI_InGame.CurrentStrikes.text = "Strikes: 0";
    }
    private void SetMapInfo_Waypoints()
    {
        if (m.Waypoints.Length > 0)
        {
            UI_InGame.Waypoint.gameObject.SetActive(true);
            UI_InGame.ImgWaypoint.sprite = UiManager.Instance.UI_Images.Waypoint;
            UI_InGame.Waypoint.text = m.Waypoints.Length + " \\ " + 0;
        }
        else
        {
            UI_InGame.Waypoint.gameObject.SetActive(false);
        }
    }
    private void SetButtonRestart()
    {
        UI_InGame.ResetGame.gameObject.SetActive(true);
    }
    private void HideMapInfo()
    {
        UI_InGame.MapInfo.text = "";
    }
    private void HideMapInfo_Medals()
    {
        UI_InGame.MedalGold.text = "";
        UI_InGame.ImgMedalGold.color = Color.clear;
        UI_InGame.MedalSilver.text = "";
        UI_InGame.ImgMedalSilver.color = Color.clear;
        UI_InGame.MedalBronze.text = "";
        UI_InGame.ImgMedalBronze.color = Color.clear;
    }
    private void HideMapInfo_CurrentStrikes()
    {
        UI_InGame.CurrentStrikes.text = "";
    }
    private void HideButtonRestart()
    {
        UI_InGame.ResetGame.gameObject.SetActive(false);
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