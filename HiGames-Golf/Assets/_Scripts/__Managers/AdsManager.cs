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


    private readonly string appId = "ca-app-pub-1249591444731632~2490946798";
    private readonly string IdBannerAd = "ca-app-pub-3940256099942544/6300978111";
    //private readonly string IdInterstitialAd = "ca-app-pub-3940256099942544/1033173712";
    private readonly string IdRewardAd = "ca-app-pub-3940256099942544/5224354917";
    //private readonly string IdNativeAdvancedAd = "ca-app-pub-3940256099942544/2247696110";

    private BannerView bannerAd;
    private RewardBasedVideoAd rewardBasedVideoAd;

    public void Init()
    {
#if UNITY_EDITOR
        string adUnitId = appId;
#elif UNITY_ANDROID
        string adUnitId = appId;
#elif UNITY_IPHONE
        string adUnitId = appId;
#else
        string adUnitId = "unexpected_platform";
#endif
        MobileAds.Initialize(adUnitId);
        Debug.Log(adUnitId);
        //rewardBasedVideoAd.OnAdClosed += HandleOnAdClosed;
        //rewardBasedVideoAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        //rewardBasedVideoAd.OnAdLeavingApplication += HandleOnAdLeavingApplication;
        //rewardBasedVideoAd.OnAdLoaded += HandleOnAdLoaded;
        //rewardBasedVideoAd.OnAdOpening += HandleOnAdOpening;
        //rewardBasedVideoAd.OnAdStarted += HandleOnAdStarted;
        //rewardBasedVideoAd.OnAdCompleted += HandleOnAdCompleted;

        RequestBannerAd();
        RequestRewardAd();

        //OnClickShowBanner();
    }

    //----- Buttons
    public void OnClickShowBanner()
    {
        ShowBannerAd();
    }
    public void OnClickShowRewardVideo()
    {
        ShowRewardAd();
    }

    //----- Requests and shows
    private void RequestBannerAd()
    {
        bannerAd = new BannerView(IdBannerAd, AdSize.Banner, AdPosition.Center);

        bannerAd.LoadAd( new AdRequest.Builder()
            .AddTestDevice(AdRequest.TestDeviceSimulator)
            .AddTestDevice(SystemInfo.deviceUniqueIdentifier)
            .Build());

        bannerAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        bannerAd.OnAdLoaded += HandleOnAdLoaded;
    }
    private void RequestRewardAd()
    {
        rewardBasedVideoAd = RewardBasedVideoAd.Instance;

        rewardBasedVideoAd.OnAdRewarded += HandleOnAdRewarded;

        rewardBasedVideoAd.LoadAd(new AdRequest.Builder()
            .AddTestDevice(AdRequest.TestDeviceSimulator)
            .AddTestDevice(SystemInfo.deviceUniqueIdentifier)
            .Build()
            ,IdRewardAd);
    }
    private void ShowBannerAd()
    {
        if (bannerAd != null)
        {
            bannerAd.Show();
        }
        else Debug.Log("Banner is null");
    }
    private void ShowRewardAd()
    {
        if (rewardBasedVideoAd.IsLoaded())
        {
            rewardBasedVideoAd.Show();
        }
        else
        {
            MonoBehaviour.print("Ad's not loaded");
        }
    }

    //----- Handles
    public void HandleOnAdRewarded(object sender, Reward args)
    {
        //Reward the user
        Debug.Log("Rewarded");

        ProfileManager.Instance.Add_Currency(RewardAmount, 0);
        UiManager.Instance.Update_Currency();
        timer = 0;

        //----- Request next ad for the next time showing
        RequestRewardAd();
    }
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        print("HandleAdLoaded event received.");
        ShowBannerAd();
    }
    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
        RequestBannerAd();
    }

    //----- Update
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


    //---------- Old Code ----------//
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

