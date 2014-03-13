using UnityEngine;
using System.Collections;
using System.IO;

public class UserInput_ExampleTextBox : MonoBehaviour {
	GUIContent[] comboBoxList;
	private ComboBox comboBoxControl = new ComboBox();
	public GUIStyle listStyle = new GUIStyle();
	public string stringToEdit;
	public string stringToEdit2;
	public GUIStyle textAreaStyle;
	private string enterButtonText = "Click to submit user information";
	private string applicationPath;

	void Start() {
		applicationPath = Application.persistentDataPath.ToString ();

		comboBoxList = new GUIContent[5];
		comboBoxList[0] = new GUIContent("Thing 1");
		comboBoxList[1] = new GUIContent("Thing 2");
		comboBoxList[2] = new GUIContent("Thing 3");
		comboBoxList[3] = new GUIContent("Thing 4");
		comboBoxList[4] = new GUIContent("Thing 5");
		
		listStyle.normal.textColor = Color.white; 
		listStyle.onHover.background = listStyle.hover.background = new Texture2D(2, 2);
		listStyle.padding.left = listStyle.padding.right = listStyle.padding.top = listStyle.padding.bottom = 4;
	}
	
	void OnGUI() {
		int screenHeight = Screen.height;
		int screenWidth = Screen.width;

		int selectedItemIndex = comboBoxControl.GetSelectedItemIndex();
		Rect comboboxPlacement = new Rect ((screenWidth * 4) / 10, (screenHeight * 7) / 10, 300, 50);
		Rect labelPlacement = new Rect ((screenWidth * 4) / 10, (screenHeight * 9) / 10, 300, 100);
		selectedItemIndex = comboBoxControl.List(comboboxPlacement, comboBoxList[selectedItemIndex].text, comboBoxList, listStyle );
		GUI.Label( labelPlacement, "You picked " + comboBoxList[selectedItemIndex].text + "!" );

		Rect textAreaPlacement = new Rect ((screenWidth * 4) / 10, (screenHeight * 4) / 10, 300f, 200f);
		Rect textAreaPlacement2 = new Rect ((screenWidth * 4) / 10, (screenHeight * 5) / 10, 300f, 200f);
		Rect enterButtonPlacement = new Rect ((screenWidth * 4) / 10, (screenHeight * 6) / 10, 300f, 150f);
		stringToEdit = GUI.TextArea(textAreaPlacement,stringToEdit,textAreaStyle);
		stringToEdit2 = GUI.TextArea(textAreaPlacement2,stringToEdit2,textAreaStyle);
		GUIContent enterButtonContent = new GUIContent (enterButtonText);
		if (GUI.Button (enterButtonPlacement, enterButtonContent)) {
			string filePath = applicationPath + "/test.txt";
			File.WriteAllText(filePath, stringToEdit);
			string textFile = File.ReadAllText(filePath);
			stringToEdit2 = textFile;
		}
	}
}