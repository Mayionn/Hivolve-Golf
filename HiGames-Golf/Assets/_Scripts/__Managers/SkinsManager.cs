using Assets.Generics;
using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

//----- SKIN
[Serializable] public abstract class Skin
{
    public string Name;
    public int Cost_Coins;
    public int Cost_Diamonds;
    public bool IsUnlocked;
    public Sprite Sprite_Display;
    [HideInInspector] public int Index;

    public void Buy_Skin()
    {
        if (ProfileManager.Instance.Gold > Cost_Coins)
        {
            IsUnlocked = true;
            ProfileManager.Instance.Remove_Currency(Cost_Coins, 0);
        }
        else Debug.Log("not enough money");
    }
    public abstract void Load_Skin(Player p);
    public abstract void SaveCurrent_Skin();
    public abstract void Save_Skin();
}
//----- BALL SKIN
[Serializable] public class Skin_Ball : Skin
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
//----- HAT SKIN
[Serializable] public class Skin_Hat : Skin
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
//----- ARROW SKIN:
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

public class SkinsManager : Singleton<SkinsManager>
{
    public List<Skin_Ball> List_Skins_Balls;
    public List<Skin_Hat> List_Skins_Hats;
    public List<Skin_Arrow> List_Skins_Arrows;
    public List<Material> List_SkyBoxes;

    public Ball DefaultBall;
    public GameObject ArrowPrefab;

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