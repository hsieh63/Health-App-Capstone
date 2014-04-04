using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using System;

public class MainMenu : MonoBehaviour {
	
	//public Rect LoginButtonRect;                // Position of login button
	//public Rect ChallengeButtonRect;            // Position of challenge button
	//public Rect HighScoreRect;            // Position of challenge button
	public GUISkin MenuSkin;
	
	private MainMenu instance;
	private float   lastChallengeSentTime = 0;
	
	//private string username = null;
	private static Texture UserTexture;
	
	private static List<object>                 friends         = null;
	private static Dictionary<string, string>   profile         = null;
	private static List<object>                 scores          = null;
	private static Dictionary<string, Texture>  friendImages    = new Dictionary<string, Texture>();
	
	void Awake()
	{
		FbDebug.Log("Awake");
		
		// allow only one instance of the Main Menu
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}
		
		#if UNITY_WEBPLAYER
		// Execute javascript in iframe to keep the player centred
		string javaScript = @"
            window.onresize = function() {
              var unity = UnityObject2.instances[0].getUnity();
              var unityDiv = document.getElementById(""unityPlayerEmbed"");

              var width =  window.innerWidth;
              var height = window.innerHeight;

              var appWidth = " + CanvasSize.x + @";
              var appHeight = " + CanvasSize.y + @";

              unity.style.width = appWidth + ""px"";
              unity.style.height = appHeight + ""px"";

              unityDiv.style.marginLeft = (width - appWidth)/2 + ""px"";
              unityDiv.style.marginTop = (height - appHeight)/2 + ""px"";
              unityDiv.style.marginRight = (width - appWidth)/2 + ""px"";
              unityDiv.style.marginBottom = (height - appHeight)/2 + ""px"";
            }

            window.onresize(); // force it to resize now";
		Application.ExternalCall(javaScript);
		#endif
		DontDestroyOnLoad(gameObject);
		instance = this;
		
		
		// Initialize FB SDK
		enabled = false;
		FB.Init(SetInit, OnHideUnity);
		GameStatus.PostScore();
	}
	
	private void SetInit()
	{
		FbDebug.Log("SetInit");
		enabled = true; // "enabled" is a property inherited from MonoBehaviour
		if (FB.IsLoggedIn) 
		{
			FbDebug.Log("Already logged in.");
			OnLoggedIn();
		}
	}
	
	private void OnHideUnity(bool isGameShown)
	{
		FbDebug.Log("OnHideUnity");
		if (!isGameShown)
		{
			// pause the game - we will need to hide
			Time.timeScale = 0;
		}
		else
		{
			// start the game back up - we're getting focus again
			Time.timeScale = 1;
		}
	}
	
	void LoginCallback(FBResult result)
	{
		FbDebug.Log("LoginCallback");
		
		if (FB.IsLoggedIn)
		{
			OnLoggedIn();
		}
	}
	void OnLoggedIn()
	{
		FbDebug.Log("Logged in. ID: " + FB.UserId);
		
		// Reqest player info and profile picture
		FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);
		FB.API(Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, MyPictureCallback);
		
		QueryScores();
	}
	
	private void QueryScores()
	{
		FB.API("/app/scores?fields=score,user.limit(20)", Facebook.HttpMethod.GET, ScoresCallback);
	}
	
	void APICallback(FBResult result)
	{
		FbDebug.Log("APICallback");
		if (result.Error != null)
		{
			FbDebug.Error(result.Error);
			// Let's just try again
			FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);
			return;
		}
		
		profile = Util.DeserializeJSONProfile(result.Text);
		GameStatus.Username = profile["first_name"];
		friends = Util.DeserializeJSONFriends(result.Text);
	}
	
	void MyPictureCallback(FBResult result)
	{
		FbDebug.Log("MyPictureCallback");
		
		if (result.Error != null)
		{
			FbDebug.Error(result.Error);
			// Let's just try again
			FB.API(Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, MyPictureCallback);
			return;
		}
		
		UserTexture = result.Texture;
	}
	//Score Stuff (to be implemented when we have a global score variable)
	private int getScoreFromEntry(object obj)
	{
		Dictionary<string,object> entry = (Dictionary<string,object>) obj;
		return Convert.ToInt32(entry["score"]);
	}
	
	void ScoresCallback(FBResult result) 
	{
		FbDebug.Log("ScoresCallback");
		if (result.Error != null)
		{
			FbDebug.Error(result.Error);
			return;
		}
		
		scores = new List<object>();
		List<object> scoresList = Util.DeserializeScores(result.Text);
		
		foreach(object score in scoresList) 
		{
			var entry = (Dictionary<string,object>) score;
			var user = (Dictionary<string,object>) entry["user"];
			
			string userId = (string)user["id"];
			
			if (string.Equals(userId,FB.UserId))
			{
				// This entry is the current player
				int playerHighScore = getScoreFromEntry(entry);
				FbDebug.Log("Local players score on server is " + playerHighScore);
				if (playerHighScore < GameStatus.highScore)
				{
					FbDebug.Log("Locally overriding with just acquired score: " + GameStatus.score);
					playerHighScore = GameStatus.highScore;
				}
				
				entry["score"] = playerHighScore.ToString();
				GameStatus.highScore = playerHighScore;
			}
			
			scores.Add(entry);
			if (!friendImages.ContainsKey(userId))
			{
				// We don't have this players image yet, request it now
				FB.API(Util.GetPictureURL(userId, 128, 128), Facebook.HttpMethod.GET, pictureResult =>
				       {
					if (pictureResult.Error != null)
					{
						FbDebug.Error(pictureResult.Error);
					}
					else
					{
						friendImages.Add(userId, pictureResult.Texture);
					}
				});
			}
		}
		
		// Now sort the entries based on score
		scores.Sort(delegate(object firstObj,
		                     object secondObj)
		            {
			return -getScoreFromEntry(firstObj).CompareTo(getScoreFromEntry(secondObj));
		}
		);
	}
	
	void OnGUI()
	{
		GUI.skin = MenuSkin;
		
		//Facebook LOGIN Button
		if (!FB.IsLoggedIn)
		{
			Rect LoginButtonRect = new Rect(Screen.width/2 - 100, Screen.height/4 + 15, 200, 75);
			if (GUI.Button(LoginButtonRect, "", MenuSkin.GetStyle("button_login")))
			{
				FB.Login("email,publish_actions", LoginCallback);
			}
		}
		
		//Welcome Label/Profile Picture and Challenge Button
		if (FB.IsLoggedIn)
		{

			string panelText = "";
			
			
			panelText += (!string.IsNullOrEmpty(GameStatus.Username)) ? "Welcome " + string.Format("{0}!", GameStatus.Username) : "";
			
			if (UserTexture != null) 
				GUI.DrawTexture( (new Rect(8,10, 150, 150)), UserTexture);
			
			
			GUI.Label( (new Rect(179 , 11, 287, 160)), panelText, MenuSkin.GetStyle("text_only"));
			
			Rect ChallengeButtonRect = new Rect(Screen.width/2 - 75, Screen.height/4 + Screen.height/25, 100, 100);
			
			if (GUI.Button(ChallengeButtonRect, "Challenge", MenuSkin.GetStyle("text_only")))
			{
				onChallengeClicked();
			}
		}
		
		//High Score Label
		string scoretext = "";
		
		scoretext += (GameStatus.highScore > 0) ? "High Score: " + GameStatus.highScore.ToString() : "";
		if (!string.IsNullOrEmpty(scoretext))
		{
			Rect HighScoreRect = new Rect(179, 80, 100, 100);
			GUI.Label(HighScoreRect, scoretext, MenuSkin.GetStyle("text_only"));
		}
		
		//Top Scores Label
		
		if(scores != null)
		{
			string topfriends = "Top Scoring Friends";
			
			Rect TopScoreRect = new Rect(Screen.width - Screen.width/3, 10, 100, 100);
			GUI.Label(TopScoreRect, topfriends, MenuSkin.GetStyle("text_only"));
			
			var x = 0;
			var step = 200;
			
			foreach(object scoreEntry in scores)
			{
				Dictionary<string,object> entry = (Dictionary<string,object>) scoreEntry;
				Dictionary<string,object> user = (Dictionary<string,object>) entry["user"];

				string name     = ((string) user["name"]).Split(new char[]{' '})[0] + "\n";
				string score     = "Score: " + entry["score"];				
				
				
				Rect rankrect = new Rect(Screen.width - 200 - 130 - 30, 80 + (step*x + 20), 100, 100);
				GUI.Label(rankrect, (x+1)+".", MenuSkin.GetStyle("text_only"));
				
				Rect namerect = new Rect(Screen.width - 200, 80 + (step*x), 100, 100);
				GUI.Label(namerect, name, MenuSkin.GetStyle("text_only"));
				
				Rect scorerect = new Rect(Screen.width - 200, 80 + (step*x + 40), 100, 100);
				GUI.Label(scorerect, score, MenuSkin.GetStyle("text_only"));
				
				Texture picture;
				if (friendImages.TryGetValue((string) user["id"], out picture)) 
				{
					GUI.DrawTexture(new Rect(Screen.width - 200 - 130,80 + (step*x),115,115), picture);  // Profile picture
				}
				x++;
			}
			
		}

	}
	
	private void onChallengeClicked()
	{
		FbDebug.Log("onChallengeClicked");
		
		FB.AppRequest(
			message: "Space Shooter is awesome!!  .",
			title: "Play Space Shooter with me!",
			callback:appRequestCallback
			);
	}
	
	private void appRequestCallback (FBResult result)
	{
		FbDebug.Log("appRequestCallback");
		if (result != null)
		{
			var responseObject = Json.Deserialize(result.Text) as Dictionary<string, object>;
			object obj = 0;
			if (responseObject.TryGetValue ("cancelled", out obj))
			{
				FbDebug.Log("Request cancelled");
			}
			else if (responseObject.TryGetValue ("request", out obj))
			{
				// Record that we went sent a request so we can display a message
				lastChallengeSentTime = Time.realtimeSinceStartup;
				FbDebug.Log("Request sent");
			}
		}
	}
}