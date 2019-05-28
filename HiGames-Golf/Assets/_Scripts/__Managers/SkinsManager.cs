using Assets.Generics;
using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class SkinsManager : Singleton<SkinsManager>
{
    public List<Skin_Ball> List_Skins_Balls;
    public List<Skin_Hat> List_Skins_Hats;
    public List<Skin_Arrow> List_Skins_Arrows;
    public List<Skin_ForceBar> List_Skins_ForceBars;
    public List<Material> List_SkyBoxes;

    public Ball DefaultBall;
    public GameObject ArrowPrefab;
    public GameObject ForceBar_Prefab;

    public void Init()
    {
        for (int i = 0; i < List_Skins_Balls.Count; i++)
        {
            List_Skins_Balls[i].Index = i;
        }
        for (int i = 0; i < List_Skins_Hats.Count; i++)
        {
            List_Skins_Hats[i].Index = i;
        }
        for (int i = 0; i < List_Skins_Arrows.Count; i++)
        {
            List_Skins_Arrows[i].Index = i;
        }
        for (int i = 0; i < List_Skins_ForceBars.Count; i++)
        {
            List_Skins_ForceBars[i].Index = i;
        }
    }

    public void SetSkybox(SkyboxType s)
    {
        switch (s)
        {
            case SkyboxType.Garage:
                RenderSettings.skybox = List_SkyBoxes[0];
                break;
            case SkyboxType.LivingRoom:
                RenderSettings.skybox = List_SkyBoxes[1];
                break;
            case SkyboxType.Outdoor:
                RenderSettings.skybox = List_SkyBoxes[2];
                break;
            default:
                break;
        }
    }
}