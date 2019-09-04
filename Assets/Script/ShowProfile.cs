// Copyright 2016 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Firebase.Sample.Database {
	using Firebase;
	using Firebase.Database;
	using Firebase.Unity.Editor;
	using System;
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	// Handler for UI buttons on the scene.  Also performs some
	// necessary setup (initializing the firebase app, etc) on
	// startup.
	public class ShowProfile : MonoBehaviour {

		ArrayList leaderBoard = new ArrayList();
		Vector2 scrollPosition = Vector2.zero;
		private Vector2 controlsScrollViewVector = Vector2.zero;

		public GUISkin fb_GUISkin;

		private const int MaxScores = 7;
		private string logText = "";
		private Vector2 scrollViewVector = Vector2.zero;
		protected bool UIEnabled = true;

		public GameObject editProfile;
		public GameObject showProfile;
		public GameObject back;

		const int kMaxLogSize = 16382;
		DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

		// When the app starts, check to make sure that we have
		// the required dependencies to use Firebase, and if not,
		// add them if possible.
		public void Start() {
			leaderBoard.Clear();
			leaderBoard.Add("Firebase Top " + MaxScores.ToString() + " Info");

			FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
				dependencyStatus = task.Result;
				if (dependencyStatus == DependencyStatus.Available) {
					InitializeFirebase();
				} else {
					Debug.LogError(
						"Could not resolve all Firebase dependencies: " + dependencyStatus);
				}
			});
		}

		// Initialize the Firebase database:
		protected virtual void InitializeFirebase() {
			FirebaseApp app = FirebaseApp.DefaultInstance;
			// NOTE: You'll need to replace this url with your Firebase App's database
			// path in order for the database connection to work correctly in editor.
			app.SetEditorDatabaseUrl("https://fly-bee-game.firebaseio.com/");
			if (app.Options.DatabaseUrl != null)
				app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
			StartListener();
		}

		//read from firebase
		protected void StartListener() {
			FirebaseDatabase.DefaultInstance
				.GetReference("UserProfile").OrderByChild("score")
				.ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
				if (e2.DatabaseError != null) {
					Debug.LogError(e2.DatabaseError.Message);
					return;
				}
				Debug.Log("Received values for Leaders.");
				string title = leaderBoard[0].ToString();
				Debug.Log("title: "+title);
				leaderBoard.Clear();
				leaderBoard.Add(title);
				if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
					foreach (var childSnapshot in e2.Snapshot.Children) {
						if (childSnapshot.Child("score") == null || childSnapshot.Child("score").Value == null) {
							Debug.LogError("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
							break;
						} 
						else {
							Debug.Log("Leaders entry : " +
								childSnapshot.Child("email").Value.ToString() + Environment.NewLine +
								childSnapshot.Child("score").Value.ToString());
							leaderBoard.Insert(1,"Client ID : " + childSnapshot.Child("score").Value.ToString()
								+ Environment.NewLine + "Email :" + childSnapshot.Child("email").Value.ToString() 
								+ Environment.NewLine + "FullName :" +childSnapshot.Child("fullname").Value.ToString());
						}
					}
				}
			};
		}

		// Exit if escape (or back, on mobile) is pressed.
		protected virtual void Update() {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				Application.Quit();
			}
		}

		// Output text to the debug log text field, as well as the console.
		public void DebugLog(string s) {
			Debug.Log(s);
			logText += s + "\n";

			while (logText.Length > kMaxLogSize) {
				int index = logText.IndexOf("\n");
				logText = logText.Substring(index + 1);
			}

			scrollViewVector.y = int.MaxValue;
		}

		// Render the log output in a scroll view.
		void GUIDisplayLog() {
			scrollViewVector = GUILayout.BeginScrollView(scrollViewVector);
			GUILayout.Label(logText);
			GUILayout.EndScrollView();
		}

		void GUIDisplayLeaders() 
		{
			GUI.skin.box.fontSize = 36;
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);
			GUILayout.BeginVertical(GUI.skin.box);

			foreach (string item in leaderBoard) {
				GUILayout.Label(item, GUI.skin.box, GUILayout.ExpandWidth(true));
			}

			GUILayout.Space(250);

			if (GUILayout.Button("Edit")) {
				editProfile.gameObject.SetActive (true);
				showProfile.gameObject.SetActive (false);

			}

			GUILayout.Space(30);

			if (GUILayout.Button("Back")) {
				showProfile.gameObject.SetActive (false);
				back.gameObject.SetActive (true);
			}

			GUILayout.EndVertical();
			GUILayout.EndScrollView();
		}

		// Render the GUI:
		void OnGUI() {
			GUI.skin = fb_GUISkin;
			if (dependencyStatus != DependencyStatus.Available) {
				GUILayout.Label("One or more Firebase dependencies are not present.");
				GUILayout.Label("Current dependency status: " + dependencyStatus.ToString());
				return;
			}
			Rect logArea, leaderBoardArea;

			if (Screen.width < Screen.height) {
				// Portrait mode
				leaderBoardArea = new Rect(0.0f, 0.0f, Screen.width, Screen.height * 0.5f);
//				leaderBoardArea = new Rect(0, Screen.height * 0.5f, Screen.width * 0.5f, Screen.height * 0.5f);
				logArea = new Rect(Screen.width * 0.5f, Screen.height * 0.5f, Screen.width * 0.5f, Screen.height * 0.5f);
			} else {
				// Landscape mode
				leaderBoardArea = new Rect(0.0f, 0.0f, Screen.width * 0.5f, Screen.height);
//		        leaderBoardArea = new Rect(Screen.width * 0.5f, 0, Screen.width * 0.5f, Screen.height * 0.5f);
				logArea = new Rect(Screen.width * 0.5f, Screen.height * 0.5f, Screen.width * 0.5f, Screen.height * 0.5f);
			}

			GUILayout.BeginArea(leaderBoardArea);
			GUIDisplayLeaders();
			GUILayout.EndArea();

			GUILayout.BeginArea(logArea);
			GUIDisplayLog();
			GUILayout.EndArea();

		}
	}
}