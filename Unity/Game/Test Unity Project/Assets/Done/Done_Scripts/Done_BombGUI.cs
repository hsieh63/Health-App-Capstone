using UnityEngine;
//using UnityEditor;
using System;
using System.IO;
using System.Collections;

public class Done_BombGUI : MonoBehaviour {

	public GameObject explosion;
	public GUIStyle bombButtonStyle;
	public TextAsset txtFile;
	protected FileInfo sourceFile = null;
	protected StreamReader sr = null;
	protected string text = " ";
	private string bombButtonText = "Click to use bomb";
	private bool bombgui = false;

	void Start() {
		/*
		//i think persistent path is the way to go since it allows real time editing?
		string applicationPath = Application.persistentDataPath.ToString ();
		//using assetdatabase is one way
		//sourceFile = new FileInfo (applicationPath + "/test.txt");
		//sourceFile = new FileInfo ("Assets/test.txt");
		sourceFile = new FileInfo ("/test.txt");
		//sr = sourceFile.OpenText ();
		using (sr) {
			while (text != null) {
				text = sr.ReadLine();
				if (text == "test1"){
					bombgui = true;
				}
				else if(text == "test2"){
					bombgui = false;
				}
			}
			sr.Close();
		}
		*/
		/*
		if (text != null) {
			text = sr.ReadLine();
			if (text == "test1"){
				bombgui = true;
			}
			else if(text = "test2"){
				bombgui == false;
			}
		}
		*/
		text = txtFile.text;
		if (text == "test1"){
			bombgui = true;
		}
		else if(text == "test2"){
			bombgui = false;
		}
	}
	
	void OnGUI() {
		int screenHeight = Screen.height;
		int screenWidth = Screen.width;
		Rect buttonPlacement = new Rect ((screenWidth * 5) / 6, (screenHeight * 5) / 6, 150f, 150f);
		//Rect buttonPlacement = new Rect (100, 100, 150, 150);
		if (bombgui) {
			GUIContent bombButtonContent = new GUIContent (bombButtonText);
			//GUI.Button (buttonPlacement, bombButtonContent, bombButtonStyle);
			if (GUI.Button (buttonPlacement, bombButtonContent)) {
				//bombButtonText = "Height: " + Screen.height + ". Width: " + Screen.width;
				GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag ("Enemy");
				foreach (GameObject target in enemyObjects) {
					GameObject.Destroy (target);
					Instantiate (explosion, target.transform.position, target.transform.rotation);
				}
			}
		}
	}
}
