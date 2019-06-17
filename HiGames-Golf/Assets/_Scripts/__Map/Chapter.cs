using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Struct;

public class Chapter
{
    public int Number;
    public GameObject BackGround;
    public Sprite BackgroundSprite;
    public Map[] Maps;
    public Display[] Displays;
    public DisplayInfo[] DisplayInfos;

    public Chapter(ChapterInfo Info, int Number)
    {
        this.Number = Number;
        //Set BackGround
        this.BackgroundSprite = Info.BackgroundSprite;
        this.Maps = Info.Maps;
        this.DisplayInfos = Info.DisplayInfos;

        Displays = new Display[Maps.Length];
        for (int i = 0; i < Displays.Length; i++)
        {
            switch (Maps[i]._GameType)
            {
                case Enums.GameType.OneShot:
                    Displays[i] = new DisplayOneShot();
                    break;
                case Enums.GameType.Waypoint:
                    //WORKING BUT NEEDS DESIGN REWORK
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
