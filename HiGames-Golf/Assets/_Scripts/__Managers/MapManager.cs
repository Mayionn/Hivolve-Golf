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
    public List<ChapterInfo> ChapterMaps;
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

        for (int i = 0; i < ChapterMaps.Count; i++)
        {
            Chapter c = new Chapter(Info: ChapterMaps[i], Number: i+1);
            Chapters.Add(c);
        }
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