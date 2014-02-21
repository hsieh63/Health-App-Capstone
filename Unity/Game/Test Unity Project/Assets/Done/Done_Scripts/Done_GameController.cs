﻿using UnityEngine;
using System.Collections;

public class Done_GameController : MonoBehaviour
{
	public GameObject[] hazards;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	
	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameOverText;
	public GUIText levelText;
	
	private bool gameOver;
	private bool restart;
	private bool notLevelChange;
	private int waveCount;
	private int wavesPerLevel;
	private int levelCount;
	private int score;
	
	void Start ()
	{
		gameOver = false;
		restart = false;
		notLevelChange = true;
		score = 0;
		waveCount = 0;
		levelCount = 1;
		wavesPerLevel = 5;
		restartText.text = "";
		gameOverText.text = "";
		levelText.text = "Level " + levelCount.ToString();
		UpdateScore ();
		StartCoroutine (SpawnWaves ());
	}
	
	void Update ()
	{
		if (restart)
		{
			foreach (Touch screenClick in Input.touches) {
				if(screenClick.phase == TouchPhase.Began) {
					Application.LoadLevel (Application.loadedLevel);
				}
			}
		}
	}
	
	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
		levelText.text = "";
		while (notLevelChange)
		{
			if(waveCount >= wavesPerLevel) {
				waveCount = 0;
				wavesPerLevel =+ 2;
				levelText.text = "Level " + levelCount.ToString();
				yield return new WaitForSeconds(5);
				levelText.text = "";
			}
			for (int i = 0; i < hazardCount; i++)
			{
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
			
			if (gameOver)
			{
				restartText.text = "Touch the screen to restart";
				restart = true;
				break;
			}
			waveCount++;
		}
	}
	
	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}
	
	void UpdateScore ()
	{
		scoreText.text = "Score: " + score;
	}
	
	public void GameOver ()
	{
		gameOverText.text = "Game Over!";
		gameOver = true;
	}
}