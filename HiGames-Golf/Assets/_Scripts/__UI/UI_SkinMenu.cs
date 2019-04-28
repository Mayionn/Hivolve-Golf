using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkinMenu : MonoBehaviour
{
    [Serializable] public struct Info_SkinManu
    {
        public Text Title;
        public Image Img_Coin;
        public Image Img_Diamond;
        public Text Text_Coin;
        public Text Text_Diamond;
        public Button Button_Back;
    }
    [Serializable] public struct Info_Displays
    {
        public Text Cost_Coins;
        public Text Cost_Diamonds;
        public Image Image_Icon_Coin;
        public Image Image_Icon_Diamond;
        public Image Image_Display;
        public Image Image_Locked;

        [HideInInspector] public Skin Skin;
    }
    public enum Tab { Ball, Hat, Trail, Hand, Acessories};

    public GameObject GO;
    public Tab CurrentTab;
    public Info_Displays[] Displays = new Info_Displays[6];

    public void Init()
    {
        GO.SetActive(true);
        Init_Tab("Ball");
    }
    public void Terminate()
    {
        GO.SetActive(false);
    }

    public void Init_Tab(string tab)
    {
        switch (tab)
        {
            case "Ball":
                CurrentTab = Tab.Ball;
                break;
            case "Hat":
                CurrentTab = Tab.Hat;
                break;
            case "Trail":
                CurrentTab = Tab.Trail;
                break;
            case "Hand":
                CurrentTab = Tab.Hand;
                break;
            default:
                break;
        }
        Setup_Displays(1);
    }

    public void BUTTON_InteractSkin(int displayIndex)
    {
        if (Displays[displayIndex].Skin.IsUnlocked)
        {
            //Load skin for main player
            Displays[displayIndex].Skin.Load_Skin(GameManager.Instance.Get_MainPlayer());
        }
        else Displays[displayIndex].Skin.Buy_Skin();
    }

    private void Setup_Displays(int page)
    {
        var bl = GetSkins<Skin>();
        int offset = (page - 1) * 6;

        for (int i = 0; i < Displays.Length; i++)
        {
            if(i + offset < bl.Count)
            {
                Skin b = bl[i + offset];

                if (b.IsUnlocked)
                {
                    Displays[i].Cost_Coins.text = "";
                    Displays[i].Cost_Diamonds.text = "";
                    Displays[i].Image_Icon_Coin.color = Color.clear;
                    Displays[i].Image_Icon_Diamond.color = Color.clear;
                    Displays[i].Image_Display.color = Color.white;
                    Displays[i].Image_Locked.color = Color.clear;
                    Displays[i].Image_Display.sprite = b.Sprite_Display;
                }
                else
                {
                    Displays[i].Cost_Coins.text = b.Cost_Coins.ToString();
                    Displays[i].Cost_Diamonds.text = b.Cost_Diamonds.ToString();
                    Displays[i].Image_Icon_Coin.color = Color.white;
                    Displays[i].Image_Icon_Diamond.color = Color.white;
                    Displays[i].Image_Icon_Coin.sprite = UiManager.Instance.UI_SkinMenuImages.Sprite_Icon_Coin;
                    Displays[i].Image_Icon_Diamond.sprite = UiManager.Instance.UI_SkinMenuImages.Sprite_Icon_Diamond;
                    Displays[i].Image_Locked.sprite = UiManager.Instance.UI_SkinMenuImages.LockedSkin;
                    Displays[i].Image_Display.color = Color.white;
                    Displays[i].Image_Locked.color = Color.white;
                }
                //Displays[i].Skin = SkinsManager.Instance.List_Skins_Balls[i + offset];
            }
            else
            {
                Displays[i].Cost_Coins.text = "";
                Displays[i].Cost_Diamonds.text = "";
                Displays[i].Image_Icon_Coin.color = Color.clear;
                Displays[i].Image_Icon_Diamond.color = Color.clear;
                Displays[i].Image_Display.color = Color.clear;
                Displays[i].Image_Locked.color = Color.clear;
            }
        }
    }
    public List<T> GetSkins<T>()
    {
        switch (CurrentTab)
        {
            case Tab.Ball:
                return (List<T>)(object)SkinsManager.Instance.List_Skins_Balls;
            case Tab.Hat:
                return (List<T>)(object)SkinsManager.Instance.List_Skins_Hats;
            case Tab.Trail:
                return (List<T>)(object)SkinsManager.Instance.List_Skins_Balls;
            case Tab.Hand:
                return (List<T>)(object)SkinsManager.Instance.List_Skins_Balls;
            case Tab.Acessories:
                return (List<T>)(object)SkinsManager.Instance.List_Skins_Balls;
            default:
                return (List<T>)(object)SkinsManager.Instance.List_Skins_Balls;
        }
    }
}