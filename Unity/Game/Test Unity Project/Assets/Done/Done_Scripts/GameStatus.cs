using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using System;

public class GameStatus : MonoBehaviour {

	private static GameStatus instance;
	public static int score = 0;
	public static int highScore = 0;
	
	private string username = null;

	public static GameStatus Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new GameObject("GameStatus").AddComponent<GameStatus> ();
			}
			
			return instance;
		}
	}  

	public static string Username
	{
		get { return Instance.username; }
		set { Instance.username = value; }
	}

	public static void LogCallback(string response) {
		Debug.Log(response);
	}

	public static void EndGame(int playerscore)
	{
		score = playerscore;

		if (score > highScore) 
		{
			FbDebug.Log ("New high score: " + highScore);
			highScore = score;
			PostScore();
		}	
	}
	
	public static void PostScore()
	{
			if (FB.IsLoggedIn)
			{
				var query = new Dictionary<string, string>();
				query["score"] = GameStatus.highScore.ToString();
				FB.API("/me/scores", Facebook.HttpMethod.POST, delegate(FBResult r) { FbDebug.Log("Result: " + r.Text); }, query);
			}
	}





}
