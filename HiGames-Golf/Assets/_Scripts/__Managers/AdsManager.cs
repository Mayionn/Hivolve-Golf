using Assets.Generics;
using Assets.Managers;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class AdsManager : Singleton<AdsManager>
{
    public int RewardAmount = 50;
    public bool IsReady;
    public float TimeTillAdvertise;
    public string addVideo = "rewardedVideo";
    [SerializeField] private float timer;

    private RewardBasedVideoAd rewardBasedVideoAd;

    public void Init()
    {
        rewardBasedVideoAd = RewardBasedVideoAd.Instance;

        rewardBasedVideoAd.OnAdClosed += HandleOnAdClosed;
        rewardBasedVideoAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        rewardBasedVideoAd.OnAdLeavingApplication += HandleOnAdLeavingApplication;
        rewardBasedVideoAd.OnAdLoaded += HandleOnAdLoaded;
        rewardBasedVideoAd.OnAdOpening += HandleOnAdOpening;
        rewardBasedVideoAd.OnAdRewarded += HandleOnAdRewarded;
        rewardBasedVideoAd.OnAdStarted += HandleOnAdStarted;
        rewardBasedVideoAd.OnAdCompleted += HandleOnAdCompleted;
    }

    private void ShowRewardBasedAd()
    {
        if(rewardBasedVideoAd.IsLoaded())
        {
            rewardBasedVideoAd.Show();
        }
        else
        {
            MonoBehaviour.print("Ad's not loaded"); 
        }
    }
    private void LoadRewardBaseAd()
    {
        #if UNITY_EDITOR
        string adUnitId = "unused";
        #elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/7325402514";
        #elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/6386090517";
        #else
        string adUnitId = "unexpected_platform";
        #endif

        rewardBasedVideoAd.LoadAd(new AdRequest.Builder().Build(), adUnitId);
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {

    }
    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //Try a reload
    }
    public void HandleOnAdOpening(object sender, EventArgs args)
    {
        //Pause the action
    }
    public void HandleOnAdStarted(object sender, EventArgs args)
    {
        //Mute audio?
    }
    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        //GameStart again
    }
    public void HandleOnAdRewarded(object sender, EventArgs args)
    {
        //Reward the user
        Debug.Log("rewarded");
    }
    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {

    }
    public void HandleOnAdCompleted(object sender, EventArgs args)
    {

    }




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

    public void ShowCompletedMapAd()
    {
        if (Advertisement.IsReady(addVideo))
        {
            var options = new ShowOptions { resultCallback = HandleShowResultCompletedMap };
            Advertisement.Show(addVideo, options);
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
    private void HandleShowResultCompletedMap(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");

                int totalEarned = (int)(UiManager.Instance.GetTotalEarned() * 0.5f);
                ProfileManager.Instance.Add_Currency(totalEarned, 0);
                UiManager.Instance.Update_Currency();
              
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

