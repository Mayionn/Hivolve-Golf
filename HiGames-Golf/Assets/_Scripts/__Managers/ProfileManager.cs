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
    }

    public void Add_Currency(int gold, int diamonds)
    {
        Gold += gold;
        Diamonds += diamonds;
        SaveManager.Instance.SaveCurrency(Gold, Diamonds);
    }
    public void Remove_Currency(int gold, int diamonds)
    {
        Gold -= gold;
        Diamonds -= diamonds;
        SaveManager.Instance.SaveCurrency(Gold, Diamonds);
    }
}
