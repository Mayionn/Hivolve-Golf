using Assets.Generics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Struct;

public class ColorPaletteManager : Singleton<ColorPaletteManager>
{
    public List<ColorPalette> ColorPalettes;

    public void SetColors(GameObject map, int colorPalette)
    {
        ColorPalette cp = ColorPalettes[colorPalette];

        MeshRenderer[] Holes = map.transform.Find("Holes").GetComponentsInChildren<MeshRenderer>();
        MeshRenderer[] Floors = map.transform.Find("Floors").GetComponentsInChildren<MeshRenderer>();
        MeshRenderer[] MapObjects = map.transform.Find("MapObjects").GetComponentsInChildren<MeshRenderer>();
        MeshRenderer[] BackgroundObjects = map.transform.Find("BackgroundObjects").GetComponentsInChildren<MeshRenderer>();
        SpriteRenderer[] Background = map.transform.Find("Background").GetComponentsInChildren<SpriteRenderer>();

        //Hole
        foreach (MeshRenderer mesh in Holes)
        {
            mesh.material.SetColor("_Color", cp.GetHex(cp.Hole));
        }
        //Floors
        foreach (MeshRenderer mesh in Floors)
        {
            mesh.material.SetColor("_Color", cp.GetHex(cp.Floor));
        }
        //Map Objects
        foreach (MeshRenderer mesh in MapObjects)
        {
            mesh.material.SetColor("_Color", cp.GetHex(cp.MapObjects));
        }
        //Background Objects
        foreach (MeshRenderer mesh in BackgroundObjects)
        {
            mesh.material.SetColor("_Color", cp.GetHex(cp.BackgroundObjects));
        }
        //Background
        foreach (SpriteRenderer sprite in Background)
        {
            sprite.material.SetColor("_Color", cp.GetHex(cp.Background));
        }
    }
}
