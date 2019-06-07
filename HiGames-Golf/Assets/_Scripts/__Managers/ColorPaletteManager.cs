using Assets.Generics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Struct;

public class ColorPaletteManager : Singleton<ColorPaletteManager>
{
    public List<MapColor> MapColors;
    public UIColors UIColors;

    public void SetMapColors(GameObject map, int colorPalette)
    {
        MapColor cp = MapColors[colorPalette];

        MeshRenderer[] Holes = map.transform.Find("Holes").GetComponentsInChildren<MeshRenderer>();
        MeshRenderer[] Floors = map.transform.Find("Floors").GetComponentsInChildren<MeshRenderer>();
        MeshRenderer[] MapObjects = map.transform.Find("MapObjects").GetComponentsInChildren<MeshRenderer>();
        MeshRenderer[] BackgroundObjects = map.transform.Find("BackgroundObjects").GetComponentsInChildren<MeshRenderer>();
        SpriteRenderer[] Background = map.transform.Find("Background").GetComponentsInChildren<SpriteRenderer>();

        //Hole
        foreach (MeshRenderer mesh in Holes)
        {
            mesh.material.SetColor("_Color", GetColor(cp.Hole));
        }
        //Floors
        foreach (MeshRenderer mesh in Floors)
        {
            mesh.material.SetColor("_Color", GetColor(cp.Floor));
        }
        //Map Objects
        foreach (MeshRenderer mesh in MapObjects)
        {
            mesh.material.SetColor("_Color", GetColor(cp.MapObjects));
        }
        //Background Objects
        foreach (MeshRenderer mesh in BackgroundObjects)
        {
            mesh.material.SetColor("_Color", GetColor(cp.BackgroundObjects));
        }
        //Background
        foreach (SpriteRenderer sprite in Background)
        {
            sprite.material.SetColor("_Color", GetColor(cp.Background));
        }
        RenderSettings.skybox.SetColor("_Tint", GetColor(cp.Skybox));
    }

    public Color GetColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            return color;
        }
        return Color.magenta;
    }
}