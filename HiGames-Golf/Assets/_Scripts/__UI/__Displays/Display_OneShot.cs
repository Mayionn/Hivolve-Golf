using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Struct;

public class DisplayOneShot : Display
{
    private Image Image;
    private Image Img_Level;
    private Image img_Star;
    private Image img_Time;
    private Text txt_Title;
    private Text txt_Time;

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

        //Set Image Components
        Image = GO.GetComponent<Image>();
        Img_Level = GO.transform.Find("MapImage").GetComponent<Image>();
        img_Star = GO.transform.Find("Star").GetComponent<Image>();
        img_Time = GO.transform.Find("BestScore_Time").GetComponent<Image>();
        //Set Text Variables
        txt_Title = GO.transform.Find("Text").GetComponent<Text>();
        txt_Title.text = "Level: " + levelNumber;
        txt_Time = GO.transform.Find("Text_BestScore_Time").GetComponent<Text>();
        //Set Image Sprites
        Img_Level.sprite = SpriteLevel;
        img_Time.sprite = UiManager.Instance.UI_Images.StopWatch;
        if (map.PB.Strikes == 1)
        {
            img_Star.sprite = UiManager.Instance.UI_Images.StarComplete;
        }
        else img_Star.sprite = UiManager.Instance.UI_Images.StarIncomplete;

        //Set Images
        if (Locked) SetLocked();
        else SetUnlocked();
    }

    public override void SetLocked()
    {
        //Set Images
        Image.sprite = SpriteLocked;

        //Turn Color On
        Img_Level.color = Color.clear;
        Img_Level.color = Color.clear;
        img_Star.color = Color.clear;
        img_Time.color = Color.clear;

        //Set Text
        txt_Time.text = "";
    }
    public override void SetUnlocked()
    {
        //Set Images
        Image.sprite = SpriteUnlocked;

        //Turn Color On
        Img_Level.color = Color.white;
        Img_Level.color = Color.white;
        img_Star.color = Color.white;
        img_Time.color = Color.white;

        //Set Text
        txt_Time.text = Map.PB.Time.ToString();
    }
}
