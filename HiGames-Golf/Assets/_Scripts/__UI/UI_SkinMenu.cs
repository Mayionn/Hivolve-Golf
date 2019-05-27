using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;
using static Struct;

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
    public Tab CurrentTab;
    public Info_SkinMenu Menu;
    public Skin_Display[] Displays = new Skin_Display[6];
    public GameObject GO;
    public GameObject BUTTON_Next;
    public GameObject BUTTON_Prev;

    private int currentPage;


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
            SetupDisplay_Unlocked(displayIndex, Displays[displayIndex].Skin);
            Setup_Currency();
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
        Menu.Img_Coin.sprite = UiManager.Instance.UI_Images.Gold;
        Menu.Img_Diamond.sprite = UiManager.Instance.UI_Images.Diamonds;
        Menu.Text_Coin.text = ProfileManager.Instance.Gold.ToString();
        Menu.Text_Diamond.text = ProfileManager.Instance.Diamonds.ToString();
    }
    private void Setup_Displays_Ball(int page)
    {
        var bl = SkinsManager.Instance.List_Skins_Balls;
        int offset = (page - 1) * 6;
        currentPage = page;

        for (int i = 0; i < Displays.Length; i++)
        {
            if (i + offset < bl.Count)
            {
                Skin b = bl[i + offset];

                if (b.IsUnlocked)
                {
                    SetupDisplay_Unlocked(i, b);
                }
                else
                {
                    SetupDisplay_Locked(i, b);
                }
                Displays[i].Skin = SkinsManager.Instance.List_Skins_Balls[i + offset];
            }
            else
            {
                SetupDisplay_Hidden(i);
            }
        }

        Setup_PageButtons(page, bl, offset);
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
                    SetupDisplay_Unlocked(i, b);
                }
                else
                {
                    SetupDisplay_Locked(i, b);
                }
                Displays[i].Skin = SkinsManager.Instance.List_Skins_Hats[i + offset];
            }
            else
            {
                SetupDisplay_Hidden(i);
            }
        }

        Setup_PageButtons_Hat(page, bl, offset);
    }

    private void Setup_PageButtons(int page, List<Skin_Ball> bl, int offset)
    {
        if (bl.Count > offset + 6)
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
    private void Setup_PageButtons_Hat(int page, List<Skin_Hat> bl, int offset)
    {
        if (bl.Count > offset + 6)
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

    private void SetupDisplay_Hidden(int i)
    {
        Displays[i].Image_Display.transform.parent.GetComponent<Image>().color = Color.clear;
        Displays[i].Cost_Coins.text = "";
        Displays[i].Cost_Diamonds.text = "";
        Displays[i].Image_Icon_Coin.color = Color.clear;
        Displays[i].Image_Icon_Diamond.color = Color.clear;
        Displays[i].Image_Display.color = Color.clear;
        Displays[i].Image_Locked.color = Color.clear;
    }
    private void SetupDisplay_Locked(int i, Skin b)
    {
        Displays[i].Image_Display.transform.parent.GetComponent<Image>().color = Color.white;
        Displays[i].Cost_Coins.text = b.Cost_Coins.ToString();
        Displays[i].Cost_Diamonds.text = b.Cost_Diamonds.ToString();
        Displays[i].Image_Icon_Coin.color = Color.black;
        Displays[i].Image_Icon_Diamond.color = Color.black;
        Displays[i].Image_Icon_Coin.sprite = UiManager.Instance.UI_Images.Gold;
        Displays[i].Image_Icon_Diamond.sprite = UiManager.Instance.UI_Images.Diamonds;
        Displays[i].Image_Locked.sprite = UiManager.Instance.UI_Images.LockedSkin;
        Displays[i].Image_Display.sprite = b.Sprite_Display;
        Displays[i].Image_Display.color = Color.white;
        Displays[i].Image_Locked.color = Color.black;
    }
    private void SetupDisplay_Unlocked(int i, Skin b)
    {
        Displays[i].Image_Display.transform.parent.GetComponent<Image>().color = Color.white;
        Displays[i].Cost_Coins.text = "";
        Displays[i].Cost_Diamonds.text = "";
        Displays[i].Image_Icon_Coin.color = Color.clear;
        Displays[i].Image_Icon_Diamond.color = Color.clear;
        Displays[i].Image_Display.color = Color.white;
        Displays[i].Image_Locked.color = Color.clear;
        Displays[i].Image_Display.sprite = b.Sprite_Display;
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