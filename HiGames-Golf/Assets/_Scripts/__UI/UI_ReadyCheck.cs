using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UI_ReadyCheck : MonoBehaviour
{
    [Serializable]
    public struct UILocalReadyCheck
    {
        public Image Background;
        public Image Image_Button;
        public Text Text_CurrentPlayer;
    }
    public GameObject UI;
    public UILocalReadyCheck ReadyCheckInfo;

    public void Init()
    {
        UI.SetActive(true);
        ReadyCheckInfo.Text_CurrentPlayer.text = "Player " + GameManager.Instance.CurrentPlayer.Name + " playing";
    }
    public void Terminate()
    {
        UI.SetActive(false);
    }
    public void Button_Ready()
    {
        UiManager.Instance.CloseInterface_InGameReadyCheck();
    }
}
