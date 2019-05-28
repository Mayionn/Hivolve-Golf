using Assets.Managers;
using System;
using UnityEngine;

[Serializable]
public class Skin_ForceBar : Skin
{
    public Material ForceBar;

    public override void Load_Skin(Player p)
    {
        p.Skin_ForceBar = this;
        GameManager.Instance.Player_ForceBar_Instantiate(p);
        p.ForceBar.GetComponent<Renderer>().material = ForceBar;
    }
    public override void SaveCurrent_Skin()
    {
        SaveManager.Instance.SaveCurrentSkin_ForceBar(Index);
    }
    public override void Save_Skin()
    {
        SaveManager.Instance.SaveUnlockedSkins_ForceBar();
    }
}