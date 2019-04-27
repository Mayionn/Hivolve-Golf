using Assets.Generics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : Singleton<ProfileManager>
{
    public int Coins;
    public int Diamonds;

    public void Init()
    {
        Coins = 10;
        Diamonds = 2;
    }
}
