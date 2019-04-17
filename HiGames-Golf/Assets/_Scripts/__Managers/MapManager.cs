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

    public Sprite LockedImage;
    public Sprite UnlockedImage;

    public Image Image;
    public Image MapImage;
    public Image Img_MedalGold;
    public Image Img_MedalSilver;
    public Image Img_MedalBronze;
    public Image Img_BestScore_Strikes;
    public Image Img_BestScore_Time;    
    public Text Txt_Title;
    public Text Txt_MedalGold;
    public Text Txt_MedalSilver;
    public Text Txt_MedalBronze;
    public Text Txt_BestScore_Strikes;
    public Text Txt_BestScore_Time;

    public void Init(Map map, int level, MapManager.DisplayInfo di)
    {
        Map = map;
        GO.GetComponent<GetMap>().map = Map;

        //Set Image Variables
        Image = GO.GetComponent<Image>();
        MapImage = GO.transform.Find("MapImage").GetComponent<Image>();
        Img_MedalGold = GO.transform.Find("MedalGold").GetComponent<Image>();
        Img_MedalSilver = GO.transform.Find("MedalSilver").GetComponent<Image>();
        Img_MedalBronze = GO.transform.Find("MedalBronze").GetComponent<Image>();
        Img_BestScore_Strikes = GO.transform.Find("BestScore_Strikes").GetComponent<Image>();
        Img_BestScore_Time = GO.transform.Find("BestScore_Time").GetComponent<Image>();

        //Set Text Variables
        Txt_Title = GO.transform.Find("Text").GetComponent<Text>();
        Txt_MedalGold = GO.transform.Find("Text_MedalGold").GetComponent<Text>();
        Txt_MedalSilver = GO.transform.Find("Text_MedalSilver").GetComponent<Text>();
        Txt_MedalBronze = GO.transform.Find("Text_MedalBronze").GetComponent<Text>();
        Txt_BestScore_Strikes = GO.transform.Find("Text_BestScore_Strikes").GetComponent<Text>();
        Txt_BestScore_Time = GO.transform.Find("Text_BestScore_Time").GetComponent<Text>();

        //Set Images
        LockedImage = di.LockedImage;
        UnlockedImage = di.UnlockedImage;
        Image.sprite = UnlockedImage;
        MapImage.sprite = di.MapImage;
        Img_MedalGold.sprite = UiManager.Instance.UI_Images.GoldMedal;
        Img_MedalSilver.sprite = UiManager.Instance.UI_Images.SilverMedal;
        Img_MedalBronze.sprite = UiManager.Instance.UI_Images.BronzeMedal;
        Img_BestScore_Strikes.sprite = UiManager.Instance.UI_Images.Strikes;
        Img_BestScore_Time.sprite = UiManager.Instance.UI_Images.StopWatch;

        //Set Text
        Txt_Title.text = "Level: " + level;
        Txt_MedalGold.text = map.MedalGold.ToString();
        Txt_MedalSilver.text = map.MedalSilver.ToString();
        Txt_MedalBronze.text = map.MedalBronze.ToString();
        Txt_BestScore_Strikes.text = map.PB.Strikes.ToString();
        Txt_BestScore_Time.text = map.PB.Time.ToString();
    }
    public void SetLocked()
    {
        Image.sprite = LockedImage;
        MapImage = null;
        Img_MedalGold = null;
        Img_MedalSilver = null;
        Img_MedalBronze = null;
        Img_BestScore_Strikes = null;
        Img_BestScore_Time = null;

        Txt_MedalGold.text = "";
        Txt_MedalSilver.text = "";
        Txt_MedalBronze.text = "";
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
            Displays[i].Init(Maps[i], level, di[i]);
        }
    }
}

public class MapManager : Singleton<MapManager>
{
    [Serializable] public struct DisplayInfo
    {
        public RectTransform pos;
        public Sprite MapImage;
        public Sprite LockedImage;
        public Sprite UnlockedImage;
    }

    public Map[] Chapter1, Chapter2, Chapter3, Chapter4;
    public DisplayInfo[] Chapter1Displays, Chapter2Displays, Chapter3Displays, Chapter4Displays;
    public List<Chapter> Chapters;
    public int CurrentChapterNumber;

    public Map Menu;
    public Map SelectedMap;
    public Map LocalMap;

    //TriggerObjects
    public GameObject Waypoint;

    private void Start()
    {
        Chapters = new List<Chapter>();
        Chapter c = new Chapter(Chapter1, UiManager.Instance.UI_BackgroundImages.DefaultBackground, 1, Chapter1Displays); 
        Chapters.Add(c);
        c = new Chapter(Chapter2, UiManager.Instance.UI_BackgroundImages.DefaultBackground, 2, Chapter2Displays);
        Chapters.Add(c);
        c = new Chapter(Chapter3, UiManager.Instance.UI_BackgroundImages.DefaultBackground, 3, Chapter3Displays);
        Chapters.Add(c);
        c = new Chapter(Chapter4, UiManager.Instance.UI_BackgroundImages.DefaultBackground, 4, Chapter4Displays);
        Chapters.Add(c);
    }
}