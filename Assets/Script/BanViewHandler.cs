using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class BanViewHandler : MonoBehaviour {

    ArrayList bannerView = new ArrayList();
    private const int MaxScores = 7;

    private BannerView bannerview;

	[SerializeField]
	private string appID = "ca-app-pub-7459369242075320~4147140130";
	[SerializeField]
	private string bannerID = "ca-app-pub-7459369242075320/2370189887";

    private int openCount = 0;

	const int kMaxLogSize = 16382;
	DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

    void Start()
    {
        MobileAds.Initialize(appID);
        OnClickShowBanner();
        bannerView.Clear();
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                    "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    // Initialize the Firebase database:
    protected virtual void InitializeFirebase()
    {
        FirebaseApp app = FirebaseApp.DefaultInstance;
        // NOTE: You'll need to replace this url with your Firebase App's database
        // path in order for the database connection to work correctly in editor.
        app.SetEditorDatabaseUrl("https://flyingbird-81952.firebaseio.com/");
        if (app.Options.DatabaseUrl != null)
            app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
    }

	public void OnClickShowBanner()
	{
		this.RequestBanner ();
	}

	private void RequestBanner()
	{ 
		// Create a 320x50 banner at the top of the screen.
		this.bannerview = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the banner with the request.
		this.bannerview.LoadAd(request);
	}


		//Banner Handler
		public void BanHandleOnAdLoaded(object sender, EventArgs args)
		{
		    MonoBehaviour.print("HandleAdLoaded event received");
            //secondText.text = "HandleAdLoaded event received";
            openCount++;
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


    TransactionResult AddScoreTransaction(MutableData mutableData)
    {
        List<object> leaders = mutableData.Value as List<object>;

        // Now we add the new score as a new entry that contains the email address and score.
        Dictionary<string, object> newScoreMap = new Dictionary<string, object>();
        newScoreMap["OpenBan"] = openCount;
        leaders.Add(newScoreMap);

        // You must set the Value to indicate data at that location has changed.
        mutableData.Value = leaders;
        return TransactionResult.Success(mutableData);
    }

    public void AddScore()
    {

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("TotalBannerView");

        Debug.Log("Running Transaction...");
        // Use a transaction to ensure that we do not encounter issues with
        // simultaneous updates that otherwise might create more than MaxScores top scores.
        reference.RunTransaction(AddScoreTransaction)
            .ContinueWith(task => {
                if (task.Exception != null)
                {
                    Debug.Log(task.Exception.ToString());
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("Transaction complete.");
                }
            });
    }
}
