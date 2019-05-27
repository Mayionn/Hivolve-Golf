using Assets.Generics;
using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Struct;



public class MapManager : Singleton<MapManager>
{
    public List<MapChapter> ChapterMaps;
    public DisplayInfo[] Chapter1Displays, Chapter2Displays, Chapter3Displays, Chapter4Displays;
    public List<Chapter> Chapters;

    public Map Menu;
    [HideInInspector] public int CurrentChapterNumber;
    [HideInInspector] public Map SelectedMap;

    //TriggerObjects
    [HideInInspector] public GameObject Waypoint;

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