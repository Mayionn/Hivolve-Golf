using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Managers;
using System;
using UnityEngine.UI;

namespace Assets.UI
{
    public class UI_LocalMultiplayer : MonoBehaviour
    {
        public GameObject UI;

        [Serializable] public struct InfoLocalGrid
        {
            public int PlayerNum;
            public string PlayerName;
            public Image Image;
            public Ball SelectedBall;
            public Text Txt_PlayerNum;
            public Text Txt_PlayerName;
        }
        public InfoLocalGrid[] GridInfos;

        public Text CurrentNumberText;
        private readonly int minPlayers = 2;
        private readonly int maxPlayers = 4;
        private int currentNumber = 1;

        public Text CurrentMapNumberText;
        private readonly int minMaps = 1;
        private readonly int maxMaps = 5;
        private int currentMapNumber = 3;

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
                GridInfos[i].PlayerName = p.Name;
                GridInfos[i].PlayerNum = p.PlayerNum;
                GridInfos[i].Txt_PlayerName.text = "Name: " + GridInfos[i].PlayerName;
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
            g.PlayerName = "";
            g.Txt_PlayerName.text = "";
            g.Txt_PlayerNum.text = "";
        }
        private void UpdateCurrentNumberText()
        {
            CurrentNumberText.text = currentNumber.ToString();
        }
    }
}