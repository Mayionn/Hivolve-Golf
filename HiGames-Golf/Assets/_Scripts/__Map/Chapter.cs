using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Struct;

[Serializable]
public class MapChapter
{
    public string Name;
    public Map[] Maps;
}

public class Chapter
{
    public int Number;
    public GameObject BackGround;
    public Sprite Sprite;
    public Map[] Maps;
    public Display[] Displays;
    public DisplayInfo[] DisplayInfos;

    public Chapter(Map[] mArray, Sprite bg, int n, DisplayInfo[] di)
    {
        this.Number = n;
        //Set BackGround
        this.Sprite = bg;
        this.Maps = mArray;
        this.DisplayInfos = di;

        Displays = new Display[Maps.Length];
        for (int i = 0; i < Displays.Length; i++)
        {
            switch (Maps[i]._GameType)
            {
                case Enums.GameType.OneShot:
                    Displays[i] = new DisplayOneShot();
                    break;
                case Enums.GameType.Waypoint:
                    Displays[i] = new DisplayWaypoint();
                    break;
                case Enums.GameType.FreeForm:
                    break;
                default:
                    Debug.Log("No map type found");
                    break;
            }
        }
    }

}
