using Assets.Generics;
using Assets.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : Singleton<AdsManager>
{
    public int RewardAmount = 50;
    public bool IsReady;
    public float TimeTillAdvertise;
    public string addVideo = "rewardedVideo";
    [SerializeField]private float timer;

    public void Update()
    {
        if(GameManager.Instance._GameMode == Enums.GameMode.Singleplayer)
        {
            timer += Time.deltaTime;
            if (timer >= TimeTillAdvertise)
            {
                IsReady = true;
            }
            else IsReady = false;
        }
    }

    public void ShowAd()
    {
        if(IsReady || GameManager.Instance._GameMode == Enums.GameMode.Menu)
        {
            if (Advertisement.IsReady(addVideo))
            {
                var options = new ShowOptions { resultCallback = HandleShowResultNoReward };
                Advertisement.Show(addVideo, options);
                timer = 0;
            }
        }
    }
    public void ShowRewardedAd()
    {
        if (IsReady || GameManager.Instance._GameMode == Enums.GameMode.Menu)
        {
            if (Advertisement.IsReady(addVideo))
            {
                var options = new ShowOptions { resultCallback = HandleShowResult };
                Advertisement.Show(addVideo, options);
                timer = 0;
            }
        }
    }

    private void HandleShowResultNoReward(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                ProfileManager.Instance.Add_Currency(RewardAmount, 0);
                UiManager.Instance.Update_Currency();
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
}

