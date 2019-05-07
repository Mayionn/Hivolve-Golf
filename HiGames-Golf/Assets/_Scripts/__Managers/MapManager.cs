using Assets.Generics;
using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display
{
    public GameObject GO;
    public GameObject GO_Copy;
    public RectTransform POS;
    public RectTransform LAST_POS;
    public Map Map;
    public Chapter Chapter;
    public bool Locked = true;
    public int levelNumber;

    private Sprite SpriteLocked;
    private Sprite SpriteUnlocked;
    private Sprite SpriteLevel;
    private Image Image;
    private Image Img_Level;
    private Image Img_MedalGold;
    private Image Img_MedalSilver;
    private Image Img_MedalBronze;
    private Image Img_BestScore_Strikes;
    private Image Img_BestScore_Time;
    private Text Txt_Title;
    private Text Txt_MedalGold;
    private Text Txt_MedalSilver;
    private Text Txt_MedalBronze;
    private Text Txt_BestScore;
    private Text Txt_BestScore_Strikes;
    private Text Txt_BestScore_Time;

    public void Init(Chapter chapter, Map map, int level, MapManager.DisplayInfo di)
    {
        //Chapter
        Chapter = chapter;
        //Map
        Map = map;
        GO.GetComponent<GetMap>().map = Map;
        Map.Display = this as Display;
        Locked = Map.IsLocked;
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
        Txt_Title = GO.transform.Find("Text").GetComponent<Text>();
        Txt_Title.text = "Level: " + levelNumber;
        Txt_MedalGold = GO.transform.Find("Text_MedalGold").GetComponent<Text>();
        Txt_MedalSilver = GO.transform.Find("Text_MedalSilver").GetComponent<Text>();
        Txt_MedalBronze = GO.transform.Find("Text_MedalBronze").GetComponent<Text>();
        Txt_BestScore = GO.transform.Find("Text_BestScore").GetComponent<Text>();
        Txt_BestScore_Strikes = GO.transform.Find("Text_BestScore_Strikes").GetComponent<Text>();
        Txt_BestScore_Time = GO.transform.Find("Text_BestScore_Time").GetComponent<Text>();
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
    public void SetUnlocked()
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
        Txt_MedalGold.text = Map.MedalGold.ToString();
        Txt_MedalSilver.text = Map.MedalSilver.ToString();
        Txt_MedalBronze.text = Map.MedalBronze.ToString();
        Txt_BestScore.text = "Best Score:";
        Txt_BestScore_Strikes.text = Map.PB.Strikes.ToString();
        Txt_BestScore_Time.text = Map.PB.Time.ToString();
    }
    public void SetLocked()
    {
        Image.sprite = SpriteLocked;

        Img_Level.color = Color.clear;
        Img_MedalGold.color = Color.clear;
        Img_MedalSilver.color = Color.clear;
        Img_MedalBronze.color = Color.clear;
        Img_BestScore_Strikes.color = Color.clear;
        Img_BestScore_Time.color = Color.clear;

        //Set Text Blank
        Txt_MedalGold.text = "";
        Txt_MedalSilver.text = "";
        Txt_MedalBronze.text = "";
        Txt_BestScore.text = "";
        Txt_BestScore_Strikes.text = "";
        Txt_BestScore_Time.text = "";
    }
}
public class Chapter
{
    public int Number;
    public GameObject BackGround;
    public Sprite Sprite;
    public Map[] Maps;
    public Display[] Displays;
    public MapManager.DisplayInfo[] DisplayInfos; 

    public Chapter(Map[] mArray, Sprite bg, int n, MapManager.DisplayInfo[] di)
    {
        Number = n;
        //Set BackGround
        Sprite = bg;
        Maps = mArray;
        Displays = new Display[Maps.Length];
        DisplayInfos = di;
        for (int i = 0; i < Displays.Length; i++)
        {
            Displays[i] = new Display();
        }
    }

    public void SetDisplays(MapManager.DisplayInfo[] di)
    {
        for (int i = 0; i < Maps.Length; i++)
        {
            int level = i + 1;
            Maps[i].Name = Number + " - " + i;
            Displays[i].Init(this, Maps[i], level, di[i]);
        }
    }
}

[Serializable]
public class MapChapter
{
    public string Name;
    public Map[] Maps;
}

public class MapManager : Singleton<MapManager>
{
    [Serializable] public struct DisplayInfo
    {
        public RectTransform pos;
        public Sprite SpriteLevel;
        public Sprite SpriteLocked;
        public Sprite SpriteUnlocked;
    }

    public List<MapChapter> ChapterMaps;
    public DisplayInfo[] Chapter1Displays, Chapter2Displays, Chapter3Displays, Chapter4Displays;
    public List<Chapter> Chapters;
    public int CurrentChapterNumber;

    public Map Menu;
    public Map SelectedMap;

    //TriggerObjects
    public GameObject Waypoint;

    public void Init()
    {

        CurrentChapterNumber = 1;
        Chapters = new List<Chapter>();
        Chapter c = new Chapter(ChapterMaps[0].Maps, UiManager.Instance.UI_BackgroundImages.DefaultBackground, 1, Chapter1Displays); 
        Chapters.Add(c);
        c = new Chapter(ChapterMaps[1].Maps, UiManager.Instance.UI_BackgroundImages.DefaultBackground, 2, Chapter2Displays);
        Chapters.Add(c);
        c = new Chapter(ChapterMaps[2].Maps, UiManager.Instance.UI_BackgroundImages.DefaultBackground, 3, Chapter3Displays);
        Chapters.Add(c);
        c = new Chapter(ChapterMaps[3].Maps, UiManager.Instance.UI_BackgroundImages.DefaultBackground, 4, Chapter4Displays);
        Chapters.Add(c);
    }

    /// <summary>
    /// Returns a defined amount of maps
    /// </summary>
    /// <param name="ammount"></param>
    /// <returns></returns>
    public List<Map> GetRandomMaps(int amount)
    {
        List<Map> lm = new List<Map>();
        for (int i = 0; i < amount; i++)
        {
            int c = UnityEngine.Random.Range(0, Chapters.Count);
            int n = UnityEngine.Random.Range(0, Chapters[c].Maps.Length);
            lm.Add(Chapters[c].Maps[n]);
        }
        return lm;
    }
}