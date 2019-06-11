using Assets.Managers;
using UnityEngine;
using static Struct;

public class UI_CompletedMap : MonoBehaviour
{
    public GameObject UI;
    public InfoCompletedMap Info;
    public InfoOneShotMap InfoOneShot;
    public float totalEarned;

    public void Init()
    {
        UI.SetActive(true);

        Map m = GameManager.Instance.CurrentMap;
        Player p = GameManager.Instance.CurrentPlayer;

        #region GiveRewards

        totalEarned = 0;

        if (m._GameType == Enums.GameType.OneShot)
        {
            if(m.PB.Strikes == 0)
            {
                totalEarned = m.GoldForCompletion;
            }
            else
            {
                totalEarned = m.GoldForCompletion * 0.1f;
            }
        }
        else
        {
            if(p.Strikes <= m.MedalGold)
            {
                if (m.PB.Strikes != 0 && m.PB.Strikes <= m.MedalGold)
                {
                    totalEarned = m.GoldForCompletion * 0.1f;
                }
                else totalEarned = m.GoldForCompletion;
            }
            else if (p.Strikes <= m.MedalSilver)
            {
                if (m.PB.Strikes != 0 && m.PB.Strikes <= m.MedalSilver)
                {
                    totalEarned = (m.GoldForCompletion * 0.5f) * 0.1f;
                }
                else totalEarned = (m.GoldForCompletion * 0.5f);
            }
            else if (p.Strikes <= m.MedalBronze)
            {
                if (m.PB.Strikes != 0 && m.PB.Strikes <= m.MedalBronze)
                {
                    totalEarned = (m.GoldForCompletion * 0.2f) * 0.1f;
                }
                else totalEarned = (m.GoldForCompletion * 0.2f);
            }
        }
        ProfileManager.Instance.Add_Currency((int)totalEarned, 0);
        #endregion

        m.CheckPersonalBest();
        UiManager.Instance.Update_MapSelector_UnlockNextLevel(m.Display.levelNumber);

        InfoOneShot.Image_CurrentTimer.sprite = UiManager.Instance.UI_Images.StopWatch;
        InfoOneShot.Image_MoneyIcon.sprite = UiManager.Instance.UI_Images.Gold;
        InfoOneShot.Txt_CurrTimer.text = p.Timer.ToString();
        InfoOneShot.Txt_MoneyEarned.text = "+" + totalEarned.ToString();

        ////UPDATE TEXT
        ////Map Medals
        //Info.Txt_GoldMedal.text = m.MedalGold.ToString();
        //Info.Image_MapGoldMedal.sprite = UiManager.Instance.UI_Images.GoldMedal;
        //Info.Txt_SilverMedal.text = m.MedalSilver.ToString();
        //Info.Image_MapSilverMedal.sprite = UiManager.Instance.UI_Images.SilverMedal;
        //Info.Txt_BronzeMedal.text = m.MedalBronze.ToString();
        //Info.Image_MapBronzeMedal.sprite = UiManager.Instance.UI_Images.BronzeMedal;
        ////PB
        //p.TruncateTimer();
        //Info.Image_PBTimer.sprite = UiManager.Instance.UI_Images.StopWatch;
        //Info.Txt_PBStrikes.text = m.PB.Strikes.ToString();
        //Info.Txt_PBTimer.text = m.PB.Time.ToString();
        ////Current Score
        //Info.Image_CurrentTimer.sprite = UiManager.Instance.UI_Images.StopWatch;
        //Info.Txt_CurrStrikes.text = p.Strikes.ToString();
        //Info.Txt_CurrTimer.text = p.Timer.ToString();
        ////UPDATE IMAGES
        //Setup_ScoreImages(m,p);

        SaveManager.Instance.SaveMapProgress();
    }
    public void Terminate()
    {
        UI.SetActive(false);
    }

    public void Button_Reset()
    {
        UiManager.Instance.CloseInterface_CompletedMap();
        UiManager.Instance.OpenInterface_InGameHud();
        GameManager.Instance.ResetGame();
    }
    public void Button_Menu()
    {
        UiManager.Instance.CloseInterface_CompletedMap();
        UiManager.Instance.OpenInterface_MapSelector();
        AudioManager.Instance.Play(Sounds.InteractSucess);
    }

    public int GetTotalEarned()
    {
        return (int)totalEarned;
    }
    public void Setup_ScoreImages(Map m, Player p)
    {
        //UPDATE PERSONAL BEST - PB
        if (m.PB.Strikes <= m.MedalGold)
        {
            Info.Img_PBStrikes.sprite = UiManager.Instance.UI_Images.GoldMedal;
        }
        else if (m.PB.Strikes <= m.MedalSilver)
        {
            Info.Img_PBStrikes.sprite = UiManager.Instance.UI_Images.SilverMedal;
        }
        else if (m.PB.Strikes <= m.MedalBronze)
        {
            Info.Img_PBStrikes.sprite = UiManager.Instance.UI_Images.BronzeMedal;
        }

        //UPDATE CURRENT SCORE
        if (p.Strikes <= m.MedalGold)
        {
            Info.Img_CurrStrikes.sprite = UiManager.Instance.UI_Images.GoldMedal;
        }
        else if (p.Strikes <= m.MedalSilver)
        {
           Info.Img_CurrStrikes.sprite = UiManager.Instance.UI_Images.SilverMedal;
        }
        else if (p.Strikes <= m.MedalBronze)
        {
            Info.Img_CurrStrikes.sprite = UiManager.Instance.UI_Images.BronzeMedal;
        }
        else
        {
            Info.Img_CurrStrikes.sprite = null;
        }
    }
}