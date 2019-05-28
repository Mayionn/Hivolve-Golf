using Assets.Managers;
using System;
using UnityEngine;

[Serializable]
public class Skin_Arrow : Skin
{
    public Material Arrow;

    public override void Load_Skin(Player p)
    {
        p.Skin_Arrow = this;
        GameManager.Instance.Player_Arrow_Instantiate(p);
        p.Arrow.GetComponent<Renderer>().material = Arrow;
    }
    public override void SaveCurrent_Skin()
    {
        SaveManager.Instance.SaveCurrentSkin_Arrow(Index);
    }
    public override void Save_Skin()
    {
        SaveManager.Instance.SaveUnlockedSkins_Arrows();
    }
}
