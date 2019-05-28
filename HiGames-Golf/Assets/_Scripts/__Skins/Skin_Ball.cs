using System;
using UnityEngine;

[Serializable]
public class Skin_Ball : Skin
{
    public Material Material;

    public override void Load_Skin(Player p)
    {
        p.Skin_Ball = this;
        p.SelectedBall.GetComponent<Renderer>().material = Material;
    }
    public override void SaveCurrent_Skin()
    {
        SaveManager.Instance.SaveCurrentSkin_Ball(Index);
    }
    public override void Save_Skin()
    {
        SaveManager.Instance.SaveUnlockedSkins_Balls();
    }
}
