using UnityEngine;
using System.Collections;

public class Done_BombGUI : MonoBehaviour {

	public GameObject explosion;
	public GUIStyle bombButtonStyle;
	private string bombButtonText = "Click to use bomb";

	void OnGUI() {
		int screenHeight = Screen.height;
		int screenWidth = Screen.width;
		Rect buttonPlacement = new Rect ((screenWidth * 5) / 6, (screenHeight * 5) / 6, 150f, 150f);
		//Rect buttonPlacement = new Rect (100, 100, 150, 150);
		GUIContent bombButtonContent = new GUIContent (bombButtonText);
		//GUI.Button (buttonPlacement, bombButtonContent, bombButtonStyle);
		if (GUI.Button (buttonPlacement, bombButtonContent)) {
			//bombButtonText = "Height: " + Screen.height + ". Width: " + Screen.width;
			GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
			foreach (GameObject target in enemyObjects) {
				GameObject.Destroy (target);
				Instantiate(explosion, target.transform.position, target.transform.rotation);
			}
		}
	}
}
