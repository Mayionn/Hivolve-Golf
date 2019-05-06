using Assets.Generics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : Singleton<ProfileManager>
{
    public int Gold;
    public int Diamonds;

    public void Init()
    {
        //SaveData data = SaveSystem.LoadData();
        //Gold = data.Gold;
        //Diamonds = data.Diamonds;

        ////TODO: REMOVE ADD CURRENCY
        //Add_Currency(1000, 1000);
    }

    public void Add_Currency(int gold, int diamonds)
    {
        Gold += gold;
        Diamonds += diamonds;
        SaveSystem.SaveCurrency(Gold, Diamonds);
    }
    public void Remove_Currency(int gold, int diamonds)
    {
        Gold -= gold;
        Diamonds -= diamonds;
        SaveSystem.SaveCurrency(Gold, Diamonds);
    }
}
