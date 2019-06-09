using Assets.Managers;
using UnityEngine;
using static Struct;

public class UI_ReadyCheck : MonoBehaviour
{
    public GameObject UI;
    public UILocalReadyCheck ReadyCheckInfo;

    public void Init()
    {
        UI.SetActive(true);
        ReadyCheckInfo.Text_CurrentPlayer.text = "Player " + (GameManager.Instance.CurrentPlayer.PlayerNum + 1);
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
