using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class RewardVHandler : MonoBehaviour {

    private RewardBasedVideoAd rewardBasedVideo;

    [SerializeField]
    private string appID = "ca-app-pub-7459369242075320~4147140130";
    [SerializeField]
    private string rewardBasedVideoID = "ca-app-pub-7459369242075320/4668038404";



    // Use this for initialization
    void Start()
    {
        MobileAds.Initialize(appID);

        this.rewardBasedVideo = RewardBasedVideoAd.Instance;
        rewardBasedVideo.OnAdLoaded += RewardHandleRewardBasedVideoLoaded;
        rewardBasedVideo.OnAdFailedToLoad += RewardHandleRewardBasedVideoFailedToLoad;
        rewardBasedVideo.OnAdLeavingApplication += RewardHandleRewardBasedVideoLeftApplication;
        rewardBasedVideo.OnAdClosed += RewardHandleRewardBasedVideoClosed;

        this.RequestRewardBasedVideo();

    }

    public void ShowRewardVideo()
    {
        if (this.rewardBasedVideo.IsLoaded())
        {
            this.rewardBasedVideo.Show();
            Debug.Log("RewardVideo SHowing....");
        }
    }

    public void RequestRewardBasedVideo()
    {
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded video ad with the request.
        this.rewardBasedVideo.LoadAd(request, rewardBasedVideoID);
    }

    //RewardBasedVideo Handler
    void RewardHandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        //VideoText.text = "Video Is Loaded";
    }

    void RewardHandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //VideoText.text = "Loading Failed";
    }

    void RewardHandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        //VideoText.text = "Video Is Complete";
    }

    void RewardHandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        //VideoText.text = "Video Is Closed";
    }

}
