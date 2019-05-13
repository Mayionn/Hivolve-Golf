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
        public Button SkinMenu;
        public Button MapSelector;
    }
    public InfoInGame UI_InGame;

    private Map m;
    private Player p;

    public void Init()
    {
        m = GameManager.Instance.CurrentMap;
        switch (GameManager.Instance._GameMode)
        {
            case GameManager.GameMode.Menu:
                UI.SetActive(true);
                Setup_MenuInfo();
                HideButtonRestart();
                HideMapInfo();
                HideMapInfo_Player();
                HideMapInfo_Waypoints();
                HideMapInfo_CurrentStrikes();
                HideMapInfo_Medals();
                HideTimer();
                HideButtonReturnMapSelector();
                break;
            case GameManager.GameMode.Singleplayer:
                UI.SetActive(true);
                Unset_MenuInfo();
                TimerStart();
                SetCurrentPlayerInfo();
                SetMapInfo();
                SetMapInfo_Medals();
                SetMapInfo_Waypoints();
                SetMapInfo_CurrentStrikes();
                SetButtonRestart();
                ShowButtonReturnMapSelector();
                break;
            case GameManager.GameMode.Multiplayer:
                break;
            case GameManager.GameMode.Localgame:
                UI.SetActive(true);
                Unset_MenuInfo();
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

    private void HideButtonReturnMapSelector()
    {
        UI_InGame.MapSelector.gameObject.SetActive(false);
    }
    private void ShowButtonReturnMapSelector()
    {
        UI_InGame.MapSelector.gameObject.SetActive(true);
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
        UI_InGame.CurrentStrikes.gameObject.SetActive(true);
        UI_InGame.CurrentStrikes.text = "Strikes: 0";
        UI_InGame.ImgCurrentStrikes.color = Color.white;
    }
    private void SetMapInfo_Waypoints()
    {
        if (m.Waypoints.Length > 0)
        {
            UI_InGame.ImgWaypoint.sprite = UiManager.Instance.UI_Images.Waypoint;
            UI_InGame.ImgWaypoint.color = Color.white;
            UI_InGame.Waypoint.text = m.Waypoints.Length + " \\ " + 0;
        }
        else
        {
            HideMapInfo_Waypoints();
        }
    }
    private void SetButtonRestart()
    {
        UI_InGame.ResetGame.gameObject.SetActive(true);
    }
    private void Setup_MenuInfo()
    {
        UI_InGame.SkinMenu.gameObject.SetActive(true);
    }
    private void HideMapInfo()
    {
        UI_InGame.MapInfo.text = "";
    }
    private void HideMapInfo_Player()
    {
        UI_InGame.CurrentPlayerInfo.text = "";
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
    private void HideMapInfo_Waypoints()
    {
        UI_InGame.Waypoint.text = "";
        UI_InGame.ImgWaypoint.color = Color.clear;
    }
    private void HideMapInfo_CurrentStrikes()
    {
        UI_InGame.CurrentStrikes.gameObject.SetActive(false);
        //UI_InGame.CurrentStrikes.text = "";
        UI_InGame.ImgCurrentStrikes.color = Color.clear;
    }
    private void HideButtonRestart()
    {
        UI_InGame.ResetGame.gameObject.SetActive(false);
    }
    private void HideTimer()
    {
        UI_InGame.CurrentTime.text = "";
    }
    private void Unset_MenuInfo()
    {
        UI_InGame.SkinMenu.gameObject.SetActive(false);
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