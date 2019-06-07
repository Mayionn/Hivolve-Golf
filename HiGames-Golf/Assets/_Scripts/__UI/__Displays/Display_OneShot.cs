using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Struct;

public class DisplayOneShot : Display
{
    private Image image;
    private Image img_Level;
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
        image = GO.GetComponent<Image>();
        img_Level = GO.transform.Find("MapImage").GetComponent<Image>();
        img_Star = GO.transform.Find("Star").GetComponent<Image>();
        img_Time = GO.transform.Find("BestScore_Time").GetComponent<Image>();
        //Set Text Variables
        txt_Title = GO.transform.Find("Text").GetComponent<Text>();
        txt_Title.text = "Level: " + levelNumber;
        txt_Time = GO.transform.Find("Text_BestScore_Time").GetComponent<Text>();
        //Set Image Sprites
        img_Level.sprite = SpriteLevel;
        img_Time.sprite = UiManager.Instance.UI_Images.StopWatch;
        //SetColor
        if (map.PB.Strikes == 1)
        {
            img_Star.sprite = UiManager.Instance.UI_Images.StarComplete;
            image.color = ColorPaletteManager.Instance.GetColor(ColorPaletteManager.Instance.UIColors.OneShotComplete);
        }
        else
        {
            img_Star.sprite = UiManager.Instance.UI_Images.StarIncomplete;
            image.color = ColorPaletteManager.Instance.GetColor(ColorPaletteManager.Instance.UIColors.OneShotIncomplete);
        }
        //Set Images
        if (Locked) SetLocked();
        else SetUnlocked();
    }

    public override void SetLocked()
    {
        //Set Images
        image.sprite = SpriteLocked;

        //Turn Color On
        img_Level.color = Color.clear;
        img_Level.color = Color.clear;
        img_Star.color = Color.clear;
        img_Time.color = Color.clear;

        //Set Text
        txt_Time.text = "";
    }
    public override void SetUnlocked()
    {
        //Set Images
        image.sprite = SpriteUnlocked;

        //Turn Color On
        img_Level.color = Color.white;
        img_Level.color = Color.white;
        img_Star.color = Color.white;
        img_Time.color = Color.white;

        //Set Text
        txt_Time.text = Map.PB.Time.ToString();
    }
}
