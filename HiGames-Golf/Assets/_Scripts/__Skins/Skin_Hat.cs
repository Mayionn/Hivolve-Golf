using Assets.Managers;
using System;
using UnityEngine;

[Serializable]
public class Skin_Hat : Skin
{
    public GameObject Hat;

    public override void Load_Skin(Player p)
    {
        p.Skin_Hat = this;
        p.Hat_Prefab = Hat;
        GameManager.Instance.Player_Hat_Instantiate(p);
    }
    public override void SaveCurrent_Skin()
    {
        SaveManager.Instance.SaveCurrentSkin_Hat(Index);
    }
    public override void Save_Skin()
    {
        SaveManager.Instance.SaveUnlockedSkins_Hats();
    }
}
