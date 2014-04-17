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
		levelText.text = "Level " + levelCount.ToString ();

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

		if (File.Exists (applicationPath + "/upgradeState.txt")) {
			StreamReader file = new StreamReader (applicationPath + "/upgradeState.txt");
			int randomNumber = Random.Range (0, 2);
			Debug.Log ("Upgrade path : " + randomNumber.ToString());
			while ((text = file.ReadLine ()) != null) {
				if (text == "1") {
					if (randomNumber == 0) {
						player.GetComponent<Done_PlayerController> ().updateTriShoot = true;
					} else if (randomNumber == 1) {
						player.GetComponent<Done_PlayerController> ().updateBigShot = true;
					}
				} else if (text == "2") {
					bomb.GetComponent<Done_BombGUI> ().bombgui = true;
					bomb.GetComponent<Done_BombGUI> ().bombCount = 1;
					if (randomNumber == 0) {
						player.GetComponent<Done_PlayerController> ().updateTriShoot = true;
						player.GetComponent<Done_PlayerController>().shieldPoint = 1;
						player.renderer.material.color = Color.blue;
					} else if (randomNumber == 1) {
						player.GetComponent<Done_PlayerController> ().updateBigShot = true;
						player.GetComponent<Done_PlayerController> ().updateDualBigShot = true;
					}
				}
				else {
					bomb.GetComponent<Done_BombGUI> ().bombgui = false;
					bomb.GetComponent<Done_BombGUI> ().bombCount = 0;
					player.GetComponent<Done_PlayerController>().shieldPoint = 0;
					player.GetComponent<Done_PlayerController> ().updateTriShoot = false;
					player.GetComponent<Done_PlayerController> ().updateBigShot = false;
					player.GetComponent<Done_PlayerController> ().updateDualBigShot = false;
				}
			}
			file.Close ();
		} else {
			bomb.GetComponent<Done_BombGUI> ().bombgui = false;
			player.GetComponent<Done_PlayerController>().shieldPoint = 0;
			player.GetComponent<Done_PlayerController> ().updateTriShoot = false;
			player.GetComponent<Done_PlayerController> ().updateBigShot = false;
			player.GetComponent<Done_PlayerController> ().updateDualBigShot = false;
		}
		Debug.Log ("Health from game controller : " + player.GetComponent<Done_PlayerController> ().shieldPoint.ToString ());

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
			Application.LoadLevel (0);
		}
	}
	
	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
		levelText.text = "";
		while (notLevelChange) {
			if (waveCount >= wavesPerLevel) {
				levelCount++;
				waveCount = 0;
				wavesPerLevel = + 2;
				levelText.text = "Level " + levelCount.ToString ();
				yield return new WaitForSeconds (5);
				levelText.text = "";
			}
			if ((levelCount % 2) == 0 && spawnBoss == false) {
				spawnBoss = true;
				Vector3 spawnPositionBoss = new Vector3 (2.5f, 0.0f, 12f);
				Quaternion spawnRotationBoss = Quaternion.identity;
				Instantiate (hazards [4], spawnPositionBoss, spawnRotationBoss);
				yield return new WaitForSeconds (spawnWait);
			} else if ((levelCount % 2) != 0) {
				spawnBoss = false;
				for (int i = 0; i < hazardCount; i++) {
					GameObject hazard = hazards [Random.Range (0, (hazards.Length - 1))];
					Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (hazard, spawnPosition, spawnRotation);
					yield return new WaitForSeconds (spawnWait);
				}
			}
			yield return new WaitForSeconds (waveWait);
			
			if (gameOver) {
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

		gameOverText.text = "Game Over!";
		gameOver = true;
	}
}