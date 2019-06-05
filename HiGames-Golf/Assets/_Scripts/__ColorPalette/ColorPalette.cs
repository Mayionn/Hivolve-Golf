using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ColorPalette
{
    [SerializeField] private string name;
    public string Hole;
    public string Floor;
    public string MapObjects;
    public string BackgroundObjects;
    public string Background;

    public Color GetHex(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            return color;
        }

        return Color.magenta;
    }
}
