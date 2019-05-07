using Assets.Generics;
using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//----- SKIN
[Serializable] public abstract class Skin
{
    public int Index;
    public string SkinName;
    public Sprite Sprite_Display;
    public int Cost_Coins;
    public int Cost_Diamonds;
    public bool IsUnlocked;

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
//----- COMIN SOON:

public class SkinsManager : Singleton<SkinsManager>
{
    public List<Skin_Ball> List_Skins_Balls;
    public List<Skin_Hat> List_Skins_Hats;
    public List<Material> List_SkyBoxes;
    public Ball DefaultBall;

    public void Init()
    {
        InitSkinIndexes();
    }

    //TODO: REWRITE THIS
    public void InitSkinIndexes()
    {
        for (int i = 0; i < List_Skins_Balls.Count; i++)
        {
            List_Skins_Balls[i].Index = i;
        }
        for (int i = 0; i < List_Skins_Hats.Count; i++)
        {
            List_Skins_Hats[i].Index = i;
        }
    }
    public void SetSkybox(Map.SkyboxType s)
    {
        switch (s)
        {
            case Map.SkyboxType.Garage:
                RenderSettings.skybox = List_SkyBoxes[0];
                break;
            case Map.SkyboxType.LivingRoom:
                RenderSettings.skybox = List_SkyBoxes[1];
                break;
            case Map.SkyboxType.Outdoor:
                RenderSettings.skybox = List_SkyBoxes[2];
                break;
            default:
                break;
        }
    }
}