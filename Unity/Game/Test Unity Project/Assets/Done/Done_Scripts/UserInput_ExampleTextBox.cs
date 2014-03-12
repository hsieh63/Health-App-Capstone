using UnityEngine;
using System.Collections;
using System.IO;

public class UserInput_ExampleTextBox : MonoBehaviour {

	public string stringToEdit;
	public GUIStyle textAreaStyle;
	private string enterButtonText = "Click to enter user information";
	private string applicationPath = Application.persistentDataPath.ToString();
	
	void OnGUI() {
		int screenHeight = Screen.height;
		int screenWidth = Screen.width;
		Rect textAreaPlacement = new Rect (screenWidth / 2, screenHeight / 2, 300f, 300f);
		Rect enterButtonPlacement = new Rect ((screenWidth * 4) / 6, (screenHeight * 4) / 6, 300f, 150f);
		stringToEdit = GUI.TextArea(textAreaPlacement,stringToEdit,textAreaStyle);
		GUIContent enterButtonContent = new GUIContent (enterButtonText);
		if (GUI.Button (enterButtonPlacement, enterButtonContent)) {
			string filePath = applicationPath + "/test.txt";
			File.WriteAllText(filePath, stringToEdit);
		}
	}
}