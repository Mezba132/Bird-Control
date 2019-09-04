using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class ads : MonoBehaviour {

	private BannerView bannerview;
	private InterstitialAd interstitial;
	private InterstitialAd interstitial2;
    private RewardBasedVideoAd rewardBasedVideo;

	//private ToastMaker toastMaker;

    [SerializeField]
	private string appID = "ca-app-pub-6407342032788722~9801604724";
    [SerializeField]
	private string bannerID = "ca-app-pub-6407342032788722/1118488868";
    [SerializeField]
	private string interstitialID = "ca-app-pub-6407342032788722/1787329436";
	[SerializeField]
	private string aboutInterstitialID = "ca-app-pub-6407342032788722/8404937585";



    // Use this for initialization
    void Start () 
	{
		MobileAds.Initialize (appID);	
		OnClickShowBanner ();
		RequestInterstitial ();
		AboutRequestInterstitial ();

		this.rewardBasedVideo = RewardBasedVideoAd.Instance;
		//this.toastMaker = new ToastMaker ();
		rewardBasedVideo.OnAdLoaded += RewardHandleRewardBasedVideoLoaded;
		rewardBasedVideo.OnAdFailedToLoad += RewardHandleRewardBasedVideoFailedToLoad;
		rewardBasedVideo.OnAdLeavingApplication += RewardHandleRewardBasedVideoLeftApplication;
		rewardBasedVideo.OnAdClosed += RewardHandleRewardBasedVideoClosed;

		this.RequestRewardBasedVideo();

		//this.RequestBanner ();

    }
	private void initiateBannerVideoCallbacks(){
		// Called when an ad request has successfully loaded.
		this.bannerview.OnAdLoaded += BanHandleOnAdLoaded;
		// Called when an ad request failed to load.
		this.bannerview.OnAdFailedToLoad += BanHandleOnAdFailedToLoad;
		// Called when an ad is clicked.
		this.bannerview.OnAdOpening += BanHandleOnAdOpened;
		// Called when the user returned from the app after an ad click.
		this.bannerview.OnAdClosed += BanHandleOnAdClosed;
		// Called when the ad click caused the user to leave the application.
		this.bannerview.OnAdLeavingApplication += BanHandleOnAdLeavingApplication;

	}
	public void OnClickShowBanner()
	{
		this.RequestBanner ();
	}

	public void showInterstitialAds()
	{
		//Show Ad
		if (this.interstitial.IsLoaded ()) {
			Debug.Log ("No Null Ref Excp");
			this.interstitial.Show ();
		}
	}

	public void ShowInterstitial2Ads()
	{
		//Show Ad
		if (this.interstitial2.IsLoaded ()) {
			Debug.Log ("No Null Ref Excp");
			this.interstitial2.Show ();
		}
	}

	public void ShowRewardVideo()
    {
        if (this.rewardBasedVideo.IsLoaded())
        {
			//secondText.text = "Video Is aviailable to show";
            this.rewardBasedVideo.Show();
            Debug.Log("RewardVideo SHowing....");
        }
    }

    private void RequestBanner()
	{ 
		// Create a 320x50 banner at the top of the screen.
		this.bannerview = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);
		initiateBannerVideoCallbacks ();
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the banner with the request.
		this.bannerview.LoadAd(request);


	}

	private void AboutRequestInterstitial()
	{

		// Initialize an InterstitialAd.
		this.interstitial2 = new InterstitialAd(aboutInterstitialID);
		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		this.interstitial2.LoadAd(request);
	}

	private void RequestInterstitial()
	{

		// Initialize an InterstitialAd.
		this.interstitial = new InterstitialAd(interstitialID);

		// Called when an ad request has successfully loaded.
		this.interstitial.OnAdLoaded += HandleOnAdLoaded;
		// Called when an ad request failed to load.
		this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		// Called when an ad is shown.
		this.interstitial.OnAdOpening += HandleOnAdOpened;
		// Called when the ad is closed.
		this.interstitial.OnAdClosed += HandleOnAdClosed;
		// Called when the ad click caused the user to leave the application.
		this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		this.interstitial.LoadAd(request);
	}

	public void RequestRewardBasedVideo()
	{
		#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-6407342032788722/2177095686";
		#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-3940256099942544/1712485313";
		#else
		string adUnitId = "unexpected_platform";
		#endif

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the rewarded video ad with the request.
		this.rewardBasedVideo.LoadAd(request, adUnitId);
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


	 //Banner Handler
       public void BanHandleOnAdLoaded(object sender, EventArgs args)
			{
			    MonoBehaviour.print("HandleAdLoaded event received");
		//secondText.text = "HandleAdLoaded event received";
			}

	   public void BanHandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
			{
			    MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "+ args.Message);
		//secondText.text = "Failed To Load Banner";
			}

	   public void BanHandleOnAdOpened(object sender, EventArgs args)
			{
			    MonoBehaviour.print("HandleAdOpened event received");
		//secondText.text = "Banner ad opened";
			}

		public void BanHandleOnAdClosed(object sender, EventArgs args)
			{
			    MonoBehaviour.print("HandleAdClosed event received");
		//secondText.text = "Banner ad closed";
			}

		public void BanHandleOnAdLeavingApplication(object sender, EventArgs args)
			{
			    MonoBehaviour.print("HandleAdLeavingApplication event received");
		//secondText.text = "HandleOnAdLeavingApplication()";
			}


		//Interstatial Handler
		public void HandleOnAdLoaded(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleAdLoaded event received");
		//firstText.text = "HandleAdLoaded event received inter";
		}

		public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
		MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
		+ args.Message);
		//firstText.text = "Failed To Load Interstatial";
		}

		public void HandleOnAdOpened(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleAdOpened event received");
		}

		public void HandleOnAdClosed(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleAdClosed event received");
		}

		public void HandleOnAdLeavingApplication(object sender, EventArgs args)
		{
		MonoBehaviour.print("HandleAdLeavingApplication event received");
		}


}
