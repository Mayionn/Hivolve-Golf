﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Struct;

public class DisplayWaypoint : Display
{
    private Image Image;
    private Image Img_Level;
    private Image Img_MedalGold;
    private Image Img_MedalSilver;
    private Image Img_MedalBronze;
    private Image Img_BestScore_Strikes;
    private Image Img_BestScore_Time;
    private Text txt_Title;
    private Text txt_MedalGold;
    private Text txt_MedalSilver;
    private Text txt_MedalBronze;
    private Text txt_BestScore;
    private Text txt_BestScore_Strikes;
    private Text txt_BestScore_Time;

    public override void Init(Chapter chapter, Map map, int level, DisplayInfo di)
    {
        //Chapter
        Chapter = chapter;
        //Map
        Map = map;
        GO.GetComponent<GetMap>().map = Map;
        Map.Display = this as Display;
        //Level
        levelNumber = level;
        //DI - Display Info
        SpriteLocked = di.SpriteLocked;
        SpriteUnlocked = di.SpriteUnlocked;
        SpriteLevel = di.SpriteLevel;

        //Set Image Variables
        Image = GO.GetComponent<Image>();
        Img_Level = GO.transform.Find("MapImage").GetComponent<Image>();
        Img_MedalGold = GO.transform.Find("MedalGold").GetComponent<Image>();
        Img_MedalSilver = GO.transform.Find("MedalSilver").GetComponent<Image>();
        Img_MedalBronze = GO.transform.Find("MedalBronze").GetComponent<Image>();
        Img_BestScore_Strikes = GO.transform.Find("BestScore_Strikes").GetComponent<Image>();
        Img_BestScore_Time = GO.transform.Find("BestScore_Time").GetComponent<Image>();
        //Set Text Variables
        txt_Title = GO.transform.Find("Text").GetComponent<Text>();
        txt_Title.text = "Level: " + levelNumber;
        txt_MedalGold = GO.transform.Find("Text_MedalGold").GetComponent<Text>();
        txt_MedalSilver = GO.transform.Find("Text_MedalSilver").GetComponent<Text>();
        txt_MedalBronze = GO.transform.Find("Text_MedalBronze").GetComponent<Text>();
        txt_BestScore = GO.transform.Find("Text_BestScore").GetComponent<Text>();
        txt_BestScore_Strikes = GO.transform.Find("Text_BestScore_Strikes").GetComponent<Text>();
        txt_BestScore_Time = GO.transform.Find("Text_BestScore_Time").GetComponent<Text>();
        //Set Image Sprites
        Img_Level.sprite = SpriteLevel;
        Img_MedalGold.sprite = UiManager.Instance.UI_Images.GoldMedal;
        Img_MedalSilver.sprite = UiManager.Instance.UI_Images.SilverMedal;
        Img_MedalBronze.sprite = UiManager.Instance.UI_Images.BronzeMedal;
        Img_BestScore_Strikes.sprite = UiManager.Instance.UI_Images.Strikes;
        Img_BestScore_Time.sprite = UiManager.Instance.UI_Images.StopWatch;

        //Set Images
        if (Locked) SetLocked();
        else SetUnlocked();
    }
    public override void SetUnlocked()
    {
        //Set Images
        Image.sprite = SpriteUnlocked;

        //Turn Color On
        Img_Level.color = Color.white;
        Img_MedalGold.color = Color.white;
        Img_MedalSilver.color = Color.white;
        Img_MedalBronze.color = Color.white;
        Img_BestScore_Strikes.color = Color.white;
        Img_BestScore_Time.color = Color.white;

        //Set Text
        txt_MedalGold.text = Map.MedalGold.ToString();
        txt_MedalSilver.text = Map.MedalSilver.ToString();
        txt_MedalBronze.text = Map.MedalBronze.ToString();
        txt_BestScore.text = "Best Score:";
        txt_BestScore_Strikes.text = Map.PB.Strikes.ToString();
        txt_BestScore_Time.text = Map.PB.Time.ToString();
    }
    public override void SetLocked()
    {
        Image.sprite = SpriteLocked;

        Img_Level.color = Color.clear;
        Img_MedalGold.color = Color.clear;
        Img_MedalSilver.color = Color.clear;
        Img_MedalBronze.color = Color.clear;
        Img_BestScore_Strikes.color = Color.clear;
        Img_BestScore_Time.color = Color.clear;

        //Set Text Blank
        txt_MedalGold.text = "";
        txt_MedalSilver.text = "";
        txt_MedalBronze.text = "";
        txt_BestScore.text = "";
        txt_BestScore_Strikes.text = "";
        txt_BestScore_Time.text = "";
    }
}
