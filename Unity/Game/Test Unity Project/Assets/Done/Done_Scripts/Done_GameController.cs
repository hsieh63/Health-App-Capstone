using UnityEngine;
using System.Collections;
using System.IO;

/*
static class upgrades {
	public static bool bombUpgrade = false;
	public static bool trishooterUpgrade = false;
}
*/

public class Done_GameController : MonoBehaviour
{
	//public static bool bombUpgrade = false;
	//public static bool trishooterUpgrade = false;

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
	private bool spawnBoss;
	private bool notLevelChange;
	private int waveCount;
	private int wavesPerLevel;
	private int levelCount;
	private int score;
	private string applicationPath;
	private string text;
	public GameObject bomb;
	public GameObject player;
	
	void Start ()
	{
		gameOver = false;
		restart = false;
		spawnBoss = false;
		notLevelChange = true;
		score = 0;
		waveCount = 0;
		levelCount = 1;
		wavesPerLevel = 1;
		restartText.text = "";
		gameOverText.text = "";
		levelText.text = "Level " + levelCount.ToString();

		//bomb = GameObject.Find ("BombGui");
		//player = GameObject.Find ("Done_Player");

		/*
		string filePath = applicationPath + "/upgrades.txt";
		StreamReader sR = new StreamReader (filePath);
		string upgradeFile = sR.ReadToEnd ();
		sR.Close ();
		string[] upgradeText = upgradeFile.Split ("\n");
		foreach (string upgradeBool in upgradeText) {
			string[]upgradeLine = upgradeBool.Split (",");
			foreach (string upgrade in upgradeLine) {
				if(upgrade.CompareTo("bomb") == 1) {
					bombUpgrade = true;
				}
				else if(upgrade.CompareTo("trishooter") == 1) {
					trishooterUpgrade = true;
				}
			}
		}
		*/
		applicationPath = Application.persistentDataPath.ToString ();
		
		StreamReader file = new StreamReader (applicationPath + "/upgradeState.txt");
		while ((text = file.ReadLine ()) != null) {
			if (text == "0") {
				bomb.GetComponent<Done_BombGUI>().bombgui = false;
				player.GetComponent<Done_PlayerController>().updateTriShoot = false;
			}
			else if (text == "1"){
				bomb.GetComponent<Done_BombGUI>().bombgui = false;
				player.GetComponent<Done_PlayerController>().updateTriShoot = true;
			}
			else if(text == "2"){
				bomb.GetComponent<Done_BombGUI>().bombgui = true;
				player.GetComponent<Done_PlayerController>().updateTriShoot = true;
			}
			else {
				bomb.GetComponent<Done_BombGUI>().bombgui = false;
				player.GetComponent<Done_PlayerController>().updateTriShoot = false;
			}
		}
		file.Close ();


		UpdateScore ();
		StartCoroutine (SpawnWaves ());
	}
	
	void Update ()
	{
		if (restart) {
				foreach (Touch screenClick in Input.touches) {
						if (screenClick.phase == TouchPhase.Began) {
								Application.LoadLevel (Application.loadedLevel);
						}
				}
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.LoadLevel(0);
		}
	}
	
	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
		levelText.text = "";
		while (notLevelChange)
		{
			if(waveCount >= wavesPerLevel) {
				levelCount++;
				waveCount = 0;
				wavesPerLevel =+ 2;
				levelText.text = "Level " + levelCount.ToString();
				yield return new WaitForSeconds(5);
				levelText.text = "";
			}
			if((levelCount % 2) == 0 && spawnBoss == false) {
				spawnBoss = true;
				Vector3 spawnPositionBoss = new Vector3 (2.5f, 0.0f, 12f);
				Quaternion spawnRotationBoss = Quaternion.identity;
				Instantiate (hazards[4], spawnPositionBoss, spawnRotationBoss);
				yield return new WaitForSeconds (spawnWait);
			}
			else if((levelCount % 2) != 0) {
				spawnBoss = false;
				for (int i = 0; i < hazardCount; i++)
				{
					GameObject hazard = hazards [Random.Range (0, (hazards.Length - 1))];
					Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (hazard, spawnPosition, spawnRotation);
					yield return new WaitForSeconds (spawnWait);
				}
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
		//write score to file/database

		//call endgame, which saves score locally (and if logged in, via the fb api)
		GameStatus.EndGame(score);
		
		gameOverText.text = "Game Over!";
		gameOver = true;
	}
}