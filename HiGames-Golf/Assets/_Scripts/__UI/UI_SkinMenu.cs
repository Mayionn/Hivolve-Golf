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
    public enum Tab { Ball, Hat, Trail, Hand};

    public GameObject GO;
    public GameObject Tab_Ball;
    public GameObject Tab_Ball_Hat;
    public GameObject Tab_Ball_Trail;
    public GameObject Tab_Hand;

    public void Init()
    {
        GO.SetActive(true);
        Tab_Ball.SetActive(true);
    }
    public void Terminate()
    {
        CloseAllTabs();
        GO.SetActive(false);
    }


    public void BUTTON_OpenTab(GameObject go)
    {
        CloseAllTabs();
        go.SetActive(true);
    }

    private void CloseAllTabs()
    {
        Tab_Ball.SetActive(false);
        Tab_Ball_Hat.SetActive(false);
        Tab_Ball_Trail.SetActive(false);
        Tab_Hand.SetActive(false);
    }
}
