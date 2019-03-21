using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Managers;
using System;

public class UI_LocalMultiplayer : MonoBehaviour
{
    public readonly int minPlayers = 2;
    public readonly int maxPlayers = 4;
    private int currentNumber;

    private Player[] newGamePlayers;
    public UI_PlayerInfo[] PlayerOnGrid;

    public void Init()
    {
        currentNumber = 2;
        CreatePlayers();
        SetPlayerInfo();
    }

    private void SetPlayerInfo()
    {
        for (int i = 0; i < currentNumber; i++)
        {
            Player mainPlayer = GameManager.Instance.Players[i];
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

    private void CreatePlayers()
    {
        if(GameManager.Instance.Players.Count < currentNumber)
        {
            GameManager.Instance.CreateFakePlayers(1);
        }
    }

    public void CurrentNumber(int num)
    {
        if(currentNumber + num > minPlayers && currentNumber + num < maxPlayers)
        {
            currentNumber += num;
            CreatePlayers();
        }
    }
}
