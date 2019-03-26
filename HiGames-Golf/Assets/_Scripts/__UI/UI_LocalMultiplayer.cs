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
        public readonly int minPlayers = 2;
        public readonly int maxPlayers = 4;

        public Text CurrentNumberText;
        private int currentNumber = 1;

        public UI_PlayerInfo[] PlayerOnGrid;

        public void Init()
        {
            CreatePlayers(1);
        }

        public void ButtonStart()
        {
            UiManager.Instance.CloseInterfaceLocalMultiplayer();
            GameManager.Instance.BuildLocalMap();
        }
        public void ButtonBack()
        {
            UiManager.Instance.CloseInterfaceLocalMultiplayer();
            //GameManager.Instance.CurrentPlayer.SelectedBall.GoStartingPosition();
            GameManager.Instance.ChooseCurrentPlayer(0);
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
                GameManager.Instance.CreateFakePlayers(num);
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

        private void UpdatePlayerInfo()
        {
            for (int i = 0; i < currentNumber; i++)
            {
                Player mainPlayer = GameManager.Instance.Players[i];
                PlayerOnGrid[i].gameObject.SetActive(true);
                PlayerOnGrid[i].PlayerName = mainPlayer.Name;
                PlayerOnGrid[i].PlayerNum = mainPlayer.PlayerNum;
                PlayerOnGrid[i].Txt_PlayerName.text = "Name: " + PlayerOnGrid[i].PlayerName;
                PlayerOnGrid[i].Txt_PlayerNum.text = "Jogador: " + (PlayerOnGrid[i].PlayerNum + 1).ToString();
            }
            //Turn not used players blank
            for (int i = currentNumber; i < PlayerOnGrid.Length; i++)
            {
                PlayerOnGrid[i].gameObject.SetActive(false);
            }
        }
        private void UpdateCurrentNumberText()
        {
            CurrentNumberText.text = currentNumber.ToString();
        }
    }
}