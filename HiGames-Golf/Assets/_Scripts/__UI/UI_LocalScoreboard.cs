using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Struct;

public class UI_LocalScoreboard : MonoBehaviour
{
    public GameObject UI;
    public GameObject UI_GameResults;
    public Text MapCounter;
    public Text Text_Button;
    public List<InfoScoreboard> UI_Scoreboard;
    public List<InfoResults> UI_ScoreResults;

    //LGS --- Local Game Scoreboard
    public void Init()
    {
        UI.SetActive(true);
        Check_BestPlayerOnMap();
        Setup_PlayerScores();
        Setup_MapCounter();
        Setup_NextMapButton();
    }
    public void Terminate()
    {
        UI.SetActive(false);
    }
    public void Init_Results()
    {
        UI_GameResults.SetActive(true);
        Setup_ResultsTableInfo();
    }
    public void Terminate_Results()
    {
        UI_GameResults.SetActive(false);
    }

    public void BUTTON_Menu()
    {
        UiManager.Instance.CloseInterface_LocalScoreboard();
        GameManager.Instance.Destroy_LocalGame_Players();
        GameManager.Instance.Setup_MenuMap();
    }
    public void BUTTON_Results_Menu()
    {
        UiManager.Instance.CloseInterface_LocalResults();
        GameManager.Instance.Destroy_LocalGame_Players();
        GameManager.Instance.Setup_MenuMap();
    }

    public void SetupRows()
    {
        LGS_SetImages();
        LGS_HideRows();
    }
    public void Setup_Score(Player p)
    {
        InfoScoreboard i = UI_Scoreboard[p.PlayerNum];
        i.PlayerName.text = p.Name;
        i.PlayerNumber.text = "Player: " + p.PlayerNum.ToString();
        i.PlayerIndexNumber.text = p.PlayerNum.ToString();
        i.PlayerStrikes.text = p.Strikes.ToString();
        p.TruncateTimer();
        i.PlayerTimer.text = p.Timer.ToString();
        i.TotalPoints.text = p.LocalgamePoints.ToString();
    }
    private void Setup_NextMapButton()
    {
        if(GameManager.Instance.MapIndex 
            == GameManager.Instance.LocalMultiplayerMaps.Count)
        {
            Text_Button.text = "Show Results";
        }
        else
        {
            Text_Button.text = "Next Map";
        }
    }
    private void Setup_MapCounter()
    {
        MapCounter.text = "Map Progress: " + (GameManager.Instance.MapIndex+1) + " / " + GameManager.Instance.LocalMultiplayerMaps.Count;
    }
    private void LGS_SetImages()
    {
        for (int i = 0; i < UI_Scoreboard.Count; i++)
        {
            LGS_ChangeImages(UI_Scoreboard[i]);
        }
    }
    private void LGS_HideRows()
    {
        for (int i = 0; i < UI_Scoreboard.Count; i++)
        {
            if (i >= GameManager.Instance.Players.Count)
            {
                LGS_Hide(UI_Scoreboard[i]);
            }
        }
    }
    private void LGS_ChangeImages(InfoScoreboard i)
    {
        i.ImgTimer.sprite = UiManager.Instance.UI_Images.StopWatch;
        i.ImgStrikes.sprite = UiManager.Instance.UI_Images.Strikes;
    }
    private void LGS_Hide(InfoScoreboard i)
    {
        i.PlayerName.text = "";
        i.PlayerNumber.text = "";
        i.PlayerTimer.text = "";
        i.ImgTimer.sprite = UiManager.Instance.UI_Images.Hidden;
        i.PlayerStrikes.text = "";
        i.ImgStrikes.sprite = UiManager.Instance.UI_Images.Hidden;
        i.TotalPoints.text = "";
        i.Medal.sprite = UiManager.Instance.UI_Images.Hidden;
    }
    private void Check_BestPlayerOnMap()
    {
        UI_Scoreboard.Sort(
            delegate (InfoScoreboard p1, InfoScoreboard p2)
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
    private void Setup_ResultsTableInfo()
    {
        GameManager.Instance.Players.Sort(
            delegate (Player p1, Player p2)
            {
                return p2.LocalgamePoints.CompareTo(p1.LocalgamePoints);
            }
        );
        List<Player> p = GameManager.Instance.Players;

        int count = GameManager.Instance.Players.Count;
        for (int i = 0; i < count; i++)
        {
            UI_ScoreResults[i].Text_PlayerName.text = p[i].Name;
            UI_ScoreResults[i].Text_PlayerScore.text = p[i].LocalgamePoints.ToString();
            UI_ScoreResults[i].Image_Medal.color = Color.white;
            switch (i)
            {
                case 0:
                    UI_ScoreResults[i].Image_Medal.sprite = UiManager.Instance.UI_Images.GoldMedal;
                    break;
                case 1:
                    UI_ScoreResults[i].Image_Medal.sprite = UiManager.Instance.UI_Images.SilverMedal;
                    break;
                case 2:
                    UI_ScoreResults[i].Image_Medal.sprite = UiManager.Instance.UI_Images.BronzeMedal;
                    break;
                default:
                    UI_ScoreResults[i].Image_Medal.color = Color.clear;
                    break;
            }
        }

        for (int i = count; i < 4; i++)
        {
            UI_ScoreResults[i].Text_PlayerName.text = "";
            UI_ScoreResults[i].Text_PlayerScore.text = "";
            UI_ScoreResults[i].Image_Medal.color = Color.clear;
        }
    }
    private void Setup_PlayerScores()
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
            if (UI_Scoreboard[i].PlayerTimer.text != "")
            {
                switch (i)
                {
                    case 0:
                        Setup_Score(UI_Scoreboard[i], UiManager.Instance.UI_Images.GoldMedal, stPlace);
                        break;
                    case 1:
                        Setup_Score(UI_Scoreboard[i], UiManager.Instance.UI_Images.SilverMedal, ndPlace);
                        break;
                    case 2:
                        Setup_Score(UI_Scoreboard[i], UiManager.Instance.UI_Images.BronzeMedal, rdPlace);
                        break;
                    default:
                        Setup_Score(UI_Scoreboard[i], UiManager.Instance.UI_Images.Hidden, thPlace);
                        break;
                }
            }
        }
    }
    private void Setup_Score(InfoScoreboard i, Sprite s, int p)
    {
        Player lp = GameManager.Instance.Players[int.Parse(i.PlayerIndexNumber.text)];
        i.TotalPoints.text = lp.LocalgamePoints.ToString() + " + " + p;
        lp.LocalgamePoints += p;
        i.Medal.sprite = s;
    }
}
