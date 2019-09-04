using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class InterstitialHandler : MonoBehaviour {

	private InterstitialAd interstitial;


	//private ToastMaker toastMaker;

	[SerializeField]
	private string appID = "ca-app-pub-7459369242075320~4147140130";
	[SerializeField]
	private string interstitialID = "ca-app-pub-7459369242075320/6185613132";



	// Use this for initialization
	void Start () 
	{
		MobileAds.Initialize (appID);	
		RequestInterstitial ();

	}

	public void showInterstitialAds()
	{
		//Show Ad
		if (this.interstitial.IsLoaded ()) {
			Debug.Log ("No Null Ref Excp");
			this.interstitial.Show ();
		}
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
