using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CompletedMap : MonoBehaviour
{
    [Serializable]
    public struct InfoCompletedMap
    {
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

    public GameObject UI;
    public InfoCompletedMap UI_CompletedMapInfo;

    public void Init()
    {
        UI.SetActive(true);

        Map m = GameManager.Instance.CurrentMap;
        Player p = GameManager.Instance.CurrentPlayer;

        m.CheckPersonalBest();
        if (m.PB.Strikes < m.MedalBronze)
        {
            UiManager.Instance.Update_MapSelector_UnlockNextLevel(m.Display.levelNumber);
        }
        //UPDATE TEXT
        //Map Medals
        UI_CompletedMapInfo.Txt_Medal01.text = m.MedalGold.ToString();
        UI_CompletedMapInfo.Txt_Medal02.text = m.MedalSilver.ToString();
        UI_CompletedMapInfo.Txt_Medal03.text = m.MedalBronze.ToString();
        //PB
        UI_CompletedMapInfo.Txt_PBStrikes.text = m.PB.Strikes.ToString();
        UI_CompletedMapInfo.Txt_PBTimer.text = m.PB.Time.ToString();
        //Current Score
        UI_CompletedMapInfo.Txt_CurrStrikes.text = p.Strikes.ToString();
        UI_CompletedMapInfo.Txt_CurrTimer.text = p.Timer.ToString();
        //UPDATE IMAGES
        Setup_ScoreImages(m,p);
    }
    public void Terminate()
    {
        UI.SetActive(false);
    }

    public void Button_Reset()
    {
        UiManager.Instance.CloseInterface_CompletedMap();
        UiManager.Instance.OpenInterface_InGameHud();
        GameManager.Instance.ResetGame();
    }
    public void Button_Menu()
    {
        UiManager.Instance.CloseInterface_CompletedMap();
        UiManager.Instance.OpenInterface_MapSelector();
        //GameManager.Instance.SetupMenuMap();
    }

    public void Setup_ScoreImages(Map m, Player p)
    {
        //UPDATE PERSONAL BEST - PB
        if (m.PB.Strikes <= m.MedalGold)
        {
            UI_CompletedMapInfo.Img_PBStrikes.sprite = UiManager.Instance.UI_Images.GoldMedal;
        }
        else if (m.PB.Strikes <= m.MedalSilver)
        {
            UI_CompletedMapInfo.Img_PBStrikes.sprite = UiManager.Instance.UI_Images.SilverMedal;
        }
        else if (m.PB.Strikes <= m.MedalBronze)
        {
            UI_CompletedMapInfo.Img_PBStrikes.sprite = UiManager.Instance.UI_Images.BronzeMedal;
        }

        //UPDATE CURRENT SCORE
        if (p.Strikes <= m.MedalGold)
        {
            UI_CompletedMapInfo.Img_CurrStrikes.sprite = UiManager.Instance.UI_Images.GoldMedal;
        }
        else if (p.Strikes <= m.MedalSilver)
        {
            UI_CompletedMapInfo.Img_CurrStrikes.sprite = UiManager.Instance.UI_Images.SilverMedal;
        }
        else if (p.Strikes <= m.MedalBronze)
        {
            UI_CompletedMapInfo.Img_CurrStrikes.sprite = UiManager.Instance.UI_Images.BronzeMedal;
        }
        else
        {
            UI_CompletedMapInfo.Img_CurrStrikes.sprite = null;
        }
    }
}