using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Managers;
using System;
using UnityEngine.UI;
using static Struct;

namespace Assets.UI
{
    public class UI_LocalMultiplayer : MonoBehaviour
    {
        public GameObject UI;
        public InfoLocalGrid[] GridInfos;
        public Text CurrentNumberText;
        public Text CurrentMapNumberText;

        private int currentNumber = 1;
        private int currentMapNumber = 3;
        private readonly int minPlayers = 2;
        private readonly int maxPlayers = 4;
        private readonly int minMaps = 1;
        private readonly int maxMaps = 5;

        public void Init()
        {
            UI.SetActive(true);
            currentNumber = 1;
            currentMapNumber = 3;
            CurrentMapNumberText.text = currentMapNumber.ToString();
            CreatePlayers(1);
        }

        public void Button_AddMap()
        {
            if(currentMapNumber < maxMaps)
            {
                currentMapNumber++;
                CurrentMapNumberText.text = currentMapNumber.ToString();
            }
        }
        public void Button_RemoveMap()
        {
            if(currentMapNumber > minMaps)
            {
                currentMapNumber--;
                CurrentMapNumberText.text = currentMapNumber.ToString();
            }
        }
        public void ButtonStart()
        {
            UiManager.Instance.CloseInterface_LocalMultiplayer();
            UiManager.Instance.OpenInterface_InGameHud();
            UiManager.Instance.Update_ScoreBoard_Rows();
            GameManager.Instance.Setup_LocalMultiplayer(currentMapNumber);
        }
        public void ButtonBack()
        {
            UiManager.Instance.CloseInterface_LocalMultiplayer();
            UiManager.Instance.OpenInterface_InGameHud();
            RemovePlayersAll();
            GameManager.Instance.CurrentPlayer.SelectedBall.GoStartingPosition(true); //Necessary cuz the map doesnt change
        }

        public void CreatePlayers(int num)
        {
            if(currentNumber < maxPlayers)
            {
                currentNumber += 1;
                UpdateCurrentNumberText();
            }
            if(GameManager.Instance.Players.Count < currentNumber)
            {
                GameManager.Instance.CreatePlayer();
                UpdatePlayerInfo();
            }
        }
        public void RemovePlayers()
        {
            if (currentNumber > minPlayers)
            {
                currentNumber -= 1;
                UpdateCurrentNumberText();
            }
            if (GameManager.Instance.Players.Count > minPlayers)
            {
                GameManager.Instance.RemovePlayer();
                UpdatePlayerInfo();
            }
        }
        public void RemovePlayersAll()
        {
            while(currentNumber != 1)
            {
                currentNumber -= 1;
                UpdateCurrentNumberText();
                GameManager.Instance.RemovePlayer();
                UpdatePlayerInfo();
            }
        }

        private void UpdatePlayerInfo()
        {
            for (int i = 0; i < currentNumber; i++)
            {
                Player p = GameManager.Instance.Players[i];
                if(i > 0)
                {
                    p.Skin_Ball = SkinsManager.Instance.List_Skins_Balls[i];
                }
                GridInfos[i].PlayerNum = p.PlayerNum;
                GridInfos[i].ImageBall.color = Color.white;
                GridInfos[i].ImageBall.sprite = p.Skin_Ball.Sprite_Display;
                GridInfos[i].Image.color = ColorPaletteManager.Instance.GetColor("#477FBE");
                GridInfos[i].Txt_PlayerNum.text = "Player: " + (GridInfos[i].PlayerNum + 1).ToString();
            }
            //Turn not used players blank
            for (int i = currentNumber; i < GridInfos.Length; i++)
            {
                SetInfoHidden(GridInfos[i]);
            }
        }
        private void SetInfoHidden(InfoLocalGrid g)
        {
            g.Image.color = Color.clear;
            g.ImageBall.color = Color.clear;
            g.Txt_PlayerNum.text = "";
        }
        private void UpdateCurrentNumberText()
        {
            CurrentNumberText.text = currentNumber.ToString();
        }
    }
}
