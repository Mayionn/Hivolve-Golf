using System;
using UnityEngine;

[Serializable]
public abstract class Skin
{
    public string Name;
    public int Cost_Coins;
    public int Cost_Diamonds;
    public bool IsUnlocked;
    public Sprite Sprite_Display;
    [HideInInspector] public int Index;

    public bool Buy_Skin_Gold()
    {
        if (ProfileManager.Instance.Gold >= Cost_Coins)
        {
            IsUnlocked = true;
            ProfileManager.Instance.Remove_Currency(Cost_Coins, 0);
            return true;
        }
        else return false;
    }
    public bool Buy_Skin_Diamonds()
    {
        if (ProfileManager.Instance.Diamonds >= Cost_Diamonds)
        {
            IsUnlocked = true;
            ProfileManager.Instance.Remove_Currency(0, Cost_Diamonds);
            return true;
        }
        else return false;
    }
    public abstract void Load_Skin(Player p);
    public abstract void SaveCurrent_Skin();
    public abstract void Save_Skin();
}
