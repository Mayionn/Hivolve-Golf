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
        SkinsManager.Instance.SaveCurrentSkin_Ball(Index);
    }
    public override void Save_Skin()
    {
        SkinsManager.Instance.SaveUnlockedSkins_Balls();
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
        SkinsManager.Instance.SaveCurrentSkin_Hat(Index);
    }
    public override void Save_Skin()
    {
        SkinsManager.Instance.SaveUnlockedSkins_Hats();
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

        LoadUnlockedSkins_Balls();
        LoadUnlockedSkins_Hats();
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

    //Current Skin ---- BALL
    public void SaveCurrentSkin_Ball(int index)
    {
        SaveSystem.SaveCurrentSkin_Ball(index);
    }
    public void LoadCurrentSkin_Ball()
    {
        SaveData data = SaveSystem.LoadData();
        GameManager.Instance.CurrentPlayer.Skin_Ball = List_Skins_Balls[data.CurrentSkin_Ball_Index];
        Material material = List_Skins_Balls[data.CurrentSkin_Ball_Index].Material;
        GameManager.Instance.CurrentPlayer.SelectedBall.GetComponent<Renderer>().material = material;
    }
    //Current Skin ---- HAT
    public void SaveCurrentSkin_Hat(int index)
    {
        SaveSystem.SaveCurrentSkin_Hat(index);
    }
    public void LoadCurrentSkin_Hat()
    {
        SaveData data = SaveSystem.LoadData();
        GameManager.Instance.CurrentPlayer.Skin_Hat = List_Skins_Hats[data.CurrentSkin_Hat_Index];
        GameManager.Instance.CurrentPlayer.Skin_Hat.Load_Skin(GameManager.Instance.CurrentPlayer);
    }
    //Unlocked Skins ---- BALLS
    public void SaveUnlockedSkins_Balls()
    {
        int count = 0;
        List<int> indexesList = new List<int>();
        //GetIndexes of Unlocked Ball Skins
        for (int i = 0; i < List_Skins_Balls.Count; i++)
        {
            if (List_Skins_Balls[i].IsUnlocked)
            {
                count++;
                indexesList.Add(i);
            }
        }
        //transform list to array
        int[] indexes = new int[count];
        for (int i = 0; i < count; i++)
        {
            indexes[i] = indexesList[i];
        }
        //Send array to SaveSystem to save
        SaveSystem.SaveSkins_Ball(count, indexes);
    }
    public void LoadUnlockedSkins_Balls()
    {
        SaveData data = SaveSystem.LoadData();

        if(data.UnlockedSkins_Balls != null)
        {
            for (int i = 0; i < data.UnlockedSkins_Balls.Length; i++)
            {
                List_Skins_Balls[data.UnlockedSkins_Balls[i]].IsUnlocked = true;
            }
        }
    }
    //Unlocked Skins ---- HATS
    public void SaveUnlockedSkins_Hats()
    {
        int count = 0;
        List<int> indexesList = new List<int>();
        //GetIndexes of Unlocked Ball Skins
        for (int i = 0; i < List_Skins_Hats.Count; i++)
        {
            if (List_Skins_Hats[i].IsUnlocked)
            {
                count++;
                indexesList.Add(i);
            }
        }
        //transform list to array
        int[] indexes = new int[count];
        for (int i = 0; i < count; i++)
        {
            indexes[i] = indexesList[i];
        }
        //Send array to SaveSystem to save
        SaveSystem.SaveSkins_Hats(count, indexes);
    }
    public void LoadUnlockedSkins_Hats()
    {
        SaveData data = SaveSystem.LoadData();

        if (data.UnlockedSkins_Hats != null)
        {
            for (int i = 0; i < data.UnlockedSkins_Hats.Length; i++)
            {
                List_Skins_Hats[data.UnlockedSkins_Hats[i]].IsUnlocked = true;
            }
        }
    }
}