using Assets.Generics;
using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable] public abstract class Skin
{
    public string SkinName;
    public Sprite Sprite_Display;
    public int Cost_Coins;
    public int Cost_Diamonds;
    public bool IsUnlocked;

    public void Buy_Skin()
    {
        if (ProfileManager.Instance.Coins > Cost_Coins)
        {
            ProfileManager.Instance.Coins -= Cost_Coins;
            IsUnlocked = true;
        }
        else Debug.Log("not enough money");
    }
    public abstract void Load_Skin(Player p);
}
[Serializable] public class Skin_Ball : Skin
{
    public Material Material;

    public override void Load_Skin(Player p)
    {
        p.Setup_Skin_Ball(this);
        p.SelectedBall.GetComponent<Renderer>().material = Material;
    }
}
[Serializable] public class Skin_Hat : Skin
{
    public GameObject Hat;

    public override void Load_Skin(Player p)
    {
        p.Setup_Skin_Hat(this);
        GameManager.Instance.PlayerBall_Hat_Instantiate(p);
    }
}

public class SkinsManager : Singleton<SkinsManager>
{
    public List<Skin_Ball> List_Skins_Balls;
    public List<Skin_Hat> List_Skins_Hats;
    public Ball DefaultBall;
}