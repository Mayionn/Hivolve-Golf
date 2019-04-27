using Assets.Generics;
using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable] public class Skin_Ball
{
    public Material Material;
    public Image Displayimage;
    public int UnlockCost;
    public bool IsUnlocked;

    public void Buy_Skin()
    {
        if(ProfileManager.Instance.Coins > UnlockCost)
        {
            ProfileManager.Instance.Coins -= UnlockCost;
            IsUnlocked = true;
        }
    }
    public void Load_Skin()
    {
        if (IsUnlocked)
        {
            GameManager.Instance.CurrentPlayer.SelectedBall.GetComponent<Renderer>().material = Material;
        }
    }
}

public class SkinsManager : MonoBehaviour
{
    public List<Skin_Ball> List_Skins_Balls;
    public Ball DefaultBall;
}