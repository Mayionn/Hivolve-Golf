using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Managers;

public class UI_LocalMultiplayer : MonoBehaviour
{
    public readonly int minPlayers = 2;
    public readonly int maxPlayers = 4;
    private int currentNumber;

    private Player[] newGamePlayers;
    public UI_PlayerInfo[] PlayerOnGrid;

    public void Init()
    {

    }


    private void SetMainPlayer()
    {
        Player stPlayer = GameManager.Instance.Players[0];
        PlayerOnGrid[0].PlayerName = stPlayer.name;
        PlayerOnGrid[0].PlayerNum = stPlayer.PlayerNum;
        PlayerOnGrid[0].Txt_PlayerName.text = PlayerOnGrid[0].PlayerName;
        PlayerOnGrid[0].Txt_PlayerNum.text = PlayerOnGrid[0].PlayerNum.ToString();
    }

    public void CurrentNumber(int num)
    {
        if(currentNumber + num > minPlayers && currentNumber + num < maxPlayers)
        {
            currentNumber += num;
        }
    }
}
