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
