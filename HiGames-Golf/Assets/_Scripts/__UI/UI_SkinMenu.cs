using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable] public class Skin_Display
{
    public Text Cost_Coins;
    public Text Cost_Diamonds;
    public Image Image_Icon_Coin;
    public Image Image_Icon_Diamond;
    public Image Image_Display;
    public Image Image_Locked;
    [HideInInspector] public Skin Skin;
}

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

    public Text CurrencyGold;
    public Text CurrencyDiamonds;
    public Image ImageGold;
    public Image ImageDiamonds;

    public GameObject GO;
    public GameObject BUTTON_Next;
    public GameObject BUTTON_Prev;

    public Tab CurrentTab;
    private int currentPage;
    public Skin_Display[] Displays = new Skin_Display[6];

    public void Init()
    {
        GO.SetActive(true);
        Setup_Currency();
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
                Setup_Displays_Ball(1);
                break;
            case "Hat":
                CurrentTab = Tab.Hat;
                Setup_Displays_Hat(1);
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
    }

    public void BUTTON_InteractSkin(int displayIndex)
    {
        if (Displays[displayIndex].Skin.IsUnlocked)
        {
            //Load skin for main player
            Displays[displayIndex].Skin.Load_Skin(GameManager.Instance.Get_MainPlayer());
            Displays[displayIndex].Skin.SaveCurrent_Skin();
        }
        else
        {
            Displays[displayIndex].Skin.Buy_Skin();
            Displays[displayIndex].Skin.Save_Skin();
        }
    }
    public void BUTTON_NextPage()
    {
        switch (CurrentTab)
        {
            case Tab.Ball:
                Setup_Displays_Ball(currentPage + 1);
                break;
            case Tab.Hat:
                Setup_Displays_Hat(currentPage + 1);
                break;
            case Tab.Trail:
                break;
            case Tab.Hand:
                break;
            case Tab.Acessories:
                break;
            default:
                break;
        }
    }
    public void BUTTON_PrevPage()
    {
        switch (CurrentTab)
        {
            case Tab.Ball:
                Setup_Displays_Ball(currentPage - 1);
                break;
            case Tab.Hat:
                Setup_Displays_Hat(currentPage - 1);
                break;
            case Tab.Trail:
                break;
            case Tab.Hand:
                break;
            case Tab.Acessories:
                break;
            default:
                break;
        }
    }

    private void Setup_Currency()
    {
        CurrencyGold.text = ProfileManager.Instance.Gold.ToString();
        CurrencyDiamonds.text = ProfileManager.Instance.Diamonds.ToString();
    }
    private void Setup_Displays_Ball(int page)
    {
        var bl = SkinsManager.Instance.List_Skins_Balls;
        int offset = (page - 1) * 6;
        currentPage = page;

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
                Displays[i].Skin = SkinsManager.Instance.List_Skins_Balls[i + offset];
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

        if(bl.Count > 6)
        {
            BUTTON_Next.SetActive(true);
        }
        else BUTTON_Next.SetActive(false);

        if (page > 1)
        {
            BUTTON_Prev.SetActive(true);
        }
        else BUTTON_Prev.SetActive(false);
    }
    private void Setup_Displays_Hat(int page)
    {
        var bl = SkinsManager.Instance.List_Skins_Hats;
        int offset = (page - 1) * 6;
        currentPage = page;

        for (int i = 0; i < Displays.Length; i++)
        {
            if (i + offset < bl.Count)
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
                Displays[i].Skin = SkinsManager.Instance.List_Skins_Hats[i + offset];
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

        if (bl.Count > 6)
        {
            BUTTON_Next.SetActive(true);
        }
        else BUTTON_Next.SetActive(false);

        if (page > 1)
        {
            BUTTON_Prev.SetActive(true);
        }
        else BUTTON_Prev.SetActive(false);
    }

    //Not working, need more time to finish
    //public T GetSkins<T>()
    //{
    //    switch (CurrentTab)
    //    {
    //        case Tab.Ball:
    //            var r = (T)(object)SkinsManager.Instance.List_Skins_Balls;
    //            return r;
    //        case Tab.Hat:
    //            return (T)(object)SkinsManager.Instance.List_Skins_Hats;
    //        case Tab.Trail:
    //            return (T)(object)SkinsManager.Instance.List_Skins_Balls;
    //        case Tab.Hand:
    //            return (T)(object)SkinsManager.Instance.List_Skins_Balls;
    //        case Tab.Acessories:
    //            return (T)(object)SkinsManager.Instance.List_Skins_Balls;
    //        default:
    //            return (T)(object)SkinsManager.Instance.List_Skins_Balls;
    //    }
    //}
}